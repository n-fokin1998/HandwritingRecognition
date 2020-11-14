using HandwritingRecognition.BL.Interfaces;
using HandwritingRecognition.BL.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace HandwritingRecognition.BL.Services
{
    public class ImageService : IImageService
    {
        private const string Base64Payload = "data:image/png;base64,";

        private readonly ImageConfiguration _imageConfiguration;

        public ImageService(ImageConfiguration imageConfiguration)
        {
            _imageConfiguration = imageConfiguration;
        }

        public List<float> GetPixelValuesFromImage(Bitmap image)
        {
            var result = new List<float>();

            for (var i = 0; i < _imageConfiguration.SizeOfImage; i += _imageConfiguration.SizeOfArea)
            {
                for (var j = 0; j < _imageConfiguration.SizeOfImage; j += _imageConfiguration.SizeOfArea)
                {
                    var sum = 0;

                    for (var k = i; k < i + _imageConfiguration.SizeOfArea; k++)
                    {
                        for (var l = j; l < j + _imageConfiguration.SizeOfArea; l++)
                        {
                            if (image.GetPixel(l, k).Name != _imageConfiguration.BackgoundColor) 
                            {
                                sum++;
                            }
                        }
                    }

                    result.Add(sum);
                }
            }

            return result;
        }

        public Bitmap GetBitmapFromBase64(string base64Image)
        {
            var imageStr = base64Image.Replace(Base64Payload, string.Empty);

            var imageBytes = Convert.FromBase64String(imageStr).ToArray();

            var bitmap = new Bitmap(_imageConfiguration.SizeOfImage, _imageConfiguration.SizeOfImage);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                using (var stream = new MemoryStream(imageBytes))
                {
                    var png = Image.FromStream(stream);

                    g.DrawImage(png, 0, 0, _imageConfiguration.SizeOfImage, _imageConfiguration.SizeOfImage);
                }
            }

            return bitmap;
        }
    }
}
