using System.Windows.Media;

namespace ExplolerClassificatorWPF.ML.YoloParser
{
    public class YoloBoundingBox
    {
        public DimensionsBase Dimensions { get; set; }
        public string Label { get; set; }
        public float Confidence { get; set; }
        public Color BoxColor { get; set; }
    }
}