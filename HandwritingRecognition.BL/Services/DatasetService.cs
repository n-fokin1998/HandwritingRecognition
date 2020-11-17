using HandwritingRecognition.BL.Extensions;
using HandwritingRecognition.BL.Interfaces;
using HandwritingRecognition.BL.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace HandwritingRecognition.BL.Services
{
    public class DatasetService : IDatasetService
    {
        private readonly IImageService _imageService;
        private readonly ImageConfiguration _imageConfiguration;
        private readonly DatasetConfiguration _datasetConfiguration;

        public DatasetService(
            IImageService imageService,
            ImageConfiguration imageConfiguration,
            DatasetConfiguration datasetConfiguration)
        {
            _imageService = imageService;
            _imageConfiguration = imageConfiguration;
            _datasetConfiguration = datasetConfiguration;
        }

        public void GenerateDataset(bool saveImages)
        {
            var fontStyles = new List<FontStyle>() { FontStyle.Bold };

            var tw = new StreamWriter(_datasetConfiguration.DatasetFilePath);
            tw.WriteLine("Values----------------------------------------------------------------Label");

            if (saveImages)
            {
                var directoryInfo = new DirectoryInfo(_datasetConfiguration.ImagesFolder);

                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
            }

            foreach (var character in _datasetConfiguration.Charecters)
            {
                foreach (var fontName in _datasetConfiguration.Fonts)
                {
                    for (int size = _datasetConfiguration.MinFontSize; size < _datasetConfiguration.MaxFontSize; size++)
                    {
                        foreach (var fontStyle in fontStyles)
                        {
                            for (int angle = _datasetConfiguration.MinAngle; angle < _datasetConfiguration.MaxAngle; angle++)
                            {
                                var result = new Bitmap(_imageConfiguration.SizeOfImage, _imageConfiguration.SizeOfImage);

                                var drawFont = new Font(fontName, size, fontStyle);
                                var drawBrush = new SolidBrush(Color.Black);

                                float x = 1;
                                float y = 1;

                                var drawFormat = new StringFormat();

                                using (var graphic = Graphics.FromImage(result))
                                {
                                    graphic.Clear(Color.White);
                                    graphic.RotateTransform(angle);
                                    graphic.DrawString(character.ToString(), drawFont, drawBrush, x, y, drawFormat);
                                }

                                if (saveImages)
                                {
                                    var path = $"{_datasetConfiguration.ImagesFolder}\\{character}_{Guid.NewGuid()}.{_datasetConfiguration.ImageExtension}";
                                    result.Save(path);
                                }

                                var datasetValue = _imageService.GetPixelValuesFromImage(result);

                                tw.WriteLine($"{datasetValue.ToCommaSeparatedString()},{character - 'A'}");
                            }
                        }
                    }
                }
            }

            tw.Close();
        }
    }
}
