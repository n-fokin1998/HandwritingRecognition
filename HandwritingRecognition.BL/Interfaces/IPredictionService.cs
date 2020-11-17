using HandwritingRecognition.BL.Models;
using System.Threading.Tasks;

namespace HandwritingRecognition.BL.Interfaces
{
    public interface IPredictionService
    {
        Task<PredictionResultDto> PredictCharacterFromImageAsync(string base64Image);
    }
}
