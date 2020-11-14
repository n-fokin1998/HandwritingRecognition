using HandwritingRecognition.BL.Interfaces;
using HandwritingRecognition.BL.Models.Configuration;
using HandwritingRecognition.BL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HandwritingRecognition.DatasetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = SetupDependencyInjection();

            var datasetService = serviceProvider.GetService<IDatasetService>();

            datasetService.GenerateDataset(true);
        }

        private static ServiceProvider SetupDependencyInjection()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            var configuration = builder.Build();

            var imageConfiguration = new ImageConfiguration();
            configuration.GetSection(nameof(ImageConfiguration)).Bind(imageConfiguration);

            var datasetConfiguration = new DatasetConfiguration();
            configuration.GetSection(nameof(DatasetConfiguration)).Bind(datasetConfiguration);

            return new ServiceCollection()
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IDatasetService, DatasetService>()
                .AddSingleton(imageConfiguration)
                .AddSingleton(datasetConfiguration)
                .BuildServiceProvider();
        }
    }
}
