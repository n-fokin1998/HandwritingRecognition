using HandwritingRecognition.BL.Extensions;
using HandwritingRecognition.BL.Interfaces;
using HandwritingRecognition.BL.Models;
using HandwritingRecognition.BL.Models.Configuration;
using HandwritingRecognition.Model;
using Microsoft.Extensions.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HandwritingRecognition.BL.Services
{
    public class PredictionService : IPredictionService
    {
        private const string ResultsFileNameFormat = "dd-MM-yyyy";

        private readonly IImageService _imageService;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;
        private readonly ModelConfiguration _modelConfiguration;

        public PredictionService(
            IImageService imageService,
            ModelConfiguration modelConfiguration,
            PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool)
        {
            _imageService = imageService;
            _modelConfiguration = modelConfiguration;
            _predictionEnginePool = predictionEnginePool;
        }

        public async Task<PredictionResultDto> PredictCharacterFromImageAsync(string base64Image)
        {
            var bitmap = _imageService.GetBitmapFromBase64(base64Image);
            var pixelValues = _imageService.GetPixelValuesFromImage(bitmap);
            var input = new ModelInput { Values = pixelValues.ToArray() };

            await SavePixelValuesAsync(pixelValues);

            var result = _predictionEnginePool.Predict(modelName: _modelConfiguration.Name, example: input);

            return new PredictionResultDto
            {
                Prediction = (char)(result.Prediction + 'A')
            };
        }

        private async Task SavePixelValuesAsync(List<float> pixelValues)
        {
            var filePath = $"{_modelConfiguration.ResultFolder}/{DateTime.UtcNow.Date.ToString(ResultsFileNameFormat)}.txt";

            if (!Directory.Exists(_modelConfiguration.ResultFolder))
            {
                Directory.CreateDirectory(_modelConfiguration.ResultFolder);
            }

            await File.AppendAllLinesAsync(
                filePath,
                new List<string> { pixelValues.ToCommaSeparatedString() });
        }
    }
}
