using AutoMapper;
using HandwritingRecognition.BL.Interfaces;
using HandwritingRecognition.BL.Models.Configuration;
using HandwritingRecognition.BL.Services;
using HandwritingRecognition.Model;
using HandwritingRecognition.Web.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ML;

namespace HandwritingRecognition.Web
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
            var imageConfiguration = new ImageConfiguration();
            Configuration.GetSection(nameof(ImageConfiguration)).Bind(imageConfiguration);

            var modelConfiguration = new ModelConfiguration();
            Configuration.GetSection(nameof(ModelConfiguration)).Bind(modelConfiguration);

            services.AddSingleton(imageConfiguration);
            services.AddSingleton(modelConfiguration);

            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPredictionService, PredictionService>();

            services.AddAutoMapper(cfg => cfg.AddProfile(typeof(ApplicationMappingProfile)));

            services.AddControllersWithViews();
            services.AddPredictionEnginePool<ModelInput, ModelOutput>()
                .FromFile(modelName: modelConfiguration.Name, filePath: modelConfiguration.Path, watchForChanges: true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
