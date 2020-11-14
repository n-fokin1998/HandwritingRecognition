using HandwritingRecognition.BL.Interfaces;
using HandwritingRecognition.BL.Models;
using HandwritingRecognition.BL.Models.Configuration;
using HandwritingRecognition.Model;
using Microsoft.Extensions.ML;

namespace HandwritingRecognition.BL.Services
{
    public class PredictionService : IPredictionService
    {
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

        public PredictionResultDto PredictCharacterFromImage(string base64Image)
        {
            var bitmap = _imageService.GetBitmapFromBase64(base64Image);
            var pixelValues = _imageService.GetPixelValuesFromImage(bitmap);
            var input = new ModelInput { Values = pixelValues.ToArray() };

            var result = _predictionEnginePool.Predict(modelName: _modelConfiguration.Name, example: input);

            return new PredictionResultDto
            {
                Prediction = (char)(result.Prediction + 'A'),
                PixelValues = string.Join(",", pixelValues)
            };
        }
    }
}
