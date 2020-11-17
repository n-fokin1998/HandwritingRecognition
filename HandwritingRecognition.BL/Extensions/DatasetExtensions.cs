using System.Collections.Generic;

namespace HandwritingRecognition.BL.Extensions
{
    public static class DatasetExtensions
    {
        public static string ToCommaSeparatedString(this IEnumerable<float> datasetValues)
        {
            return string.Join(",", datasetValues);
        }
    }
}
