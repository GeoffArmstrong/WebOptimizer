﻿using System.Collections.Generic;
using System.Globalization;
using BundlerSample;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bundler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddViewLocalization(options => options.ResourcesPath = "Resources");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            var cultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("da")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            });

            app.UseAssetPipeline(assets =>
            {
                assets.AddCss("all.css", "css/site.css", "lib/bootstrap/dist/css/bootstrap.css")
                      .MinifyCss();

                assets.AddJs("all.js", "js/site.js", "js/b.js")
                      .Localize<Strings>(app)
                      .MinifyJavaScript();

                // This file exist on disk and will now be minified
                assets.AddJs("js/site.js");
            });

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}

namespace BundlerSample
{
    public class Strings
    {

    }
}
