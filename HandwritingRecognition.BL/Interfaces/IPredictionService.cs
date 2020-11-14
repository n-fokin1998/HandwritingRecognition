using HandwritingRecognition.BL.Models;

namespace HandwritingRecognition.BL.Interfaces
{
    public interface IPredictionService
    {
        PredictionResultDto PredictCharacterFromImage(string base64Image);
    }
}
