using System.Collections.Generic;

namespace HandwritingRecognition.BL.Models.Configuration
{
    public class DatasetConfiguration
    {
        public string DatasetFilePath { get; set; }

        public string ImagesFolder { get; set; }

        public string ImageExtension { get; set; }

        public int MinFontSize { get; set; }

        public int MaxFontSize { get; set; }

        public int MinAngle { get; set; }

        public int MaxAngle { get; set; }

        public string Charecters { get; set; }

        public List<string> Fonts { get; set; }
    }
}
