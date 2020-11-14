using System.Collections.Generic;
using System.Drawing;

namespace HandwritingRecognition.BL.Interfaces
{
    public interface IImageService
    {
        List<float> GetPixelValuesFromImage(Bitmap image);

        Bitmap GetBitmapFromBase64(string base64Image);
    }
}
