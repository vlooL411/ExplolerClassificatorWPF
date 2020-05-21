using ExplolerClassificatorWPF.Properties;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.IO;
using System.Windows.Media;

namespace ExplolerClassificatorWPF.MLModel
{
    class MLProgrammSelectLanguage
    {
        //возможные классы, много-классового классификатора
        static public string[] labels
            => new string[] { "c#", "asp.net", "objective-c", ".net", "python", "angularjs", "iphone", "ruby-on-rails", "ios",
                              "c", "sql", "java", "jquery", "css", "c++", "php", "android", "mysql", "javascript", "html" };
        static public Color[] classColors => new Color[]
        {
            Colors.Khaki, Colors.Fuchsia, Colors.Silver, Colors.RoyalBlue, Colors.Green, Colors.DarkOrange,
            Colors.Purple, Colors.Gold, Colors.Red, Colors.Aquamarine, Colors.Lime, Colors.AliceBlue,
            Colors.Sienna, Colors.Orchid, Colors.Tan, Colors.LightPink, Colors.Yellow, Colors.HotPink,
            Colors.OliveDrab,  Colors.SandyBrown, Colors.DarkTurquoise
        };
        //класс для создания единичных предсказаний по ранее обученной модели
        static PredictionEngine<ModelInput, ModelOutput> predEngine;
        static MLProgrammSelectLanguage()
        {
            var ml = new MLContext();
            try
            {
                predEngine = ml.Model.CreatePredictionEngine<ModelInput, ModelOutput>(
                    ml.Model.Load(new MemoryStream(Resources.ProgrammerLanguageSelect),
                    out var modelInputSchema));
            }
            catch { }
        }
        //результат работы классификатора
        public static ModelOutput Result(string fileContent) => predEngine?.Predict(new ModelInput() { Post = fileContent });
        public class ModelInput
        {
            [ColumnName("post"), LoadColumn(0)]
            public string Post { get; set; }

            [ColumnName("tags"), LoadColumn(1)]
            public string Tags { get; set; }
        }
        public class ModelOutput
        {
            [ColumnName("PredictedLabel")]
            public string Prediction { get; set; }

            public float[] Score { get; set; }
        }
    }
}