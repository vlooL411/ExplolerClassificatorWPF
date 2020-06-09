using ExplolerClassificatorWPF.ML.YoloParser;
using ExplolerClassificatorWPF.MLModel.Yolo.YoloParser;
using ExplolerClassificatorWPF.Properties;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;
using System.IO;

namespace ExplolerClassificatorWPF.MLModel
{
    static class YoloML
    {
        //класс для создания единичных предсказаний по ранее обученной модели
        static PredictionEngine<ImageData, ImagePrediction> predEngine;
        //Обрабатывает результат нейронной сети
        static YoloOutputParser parser = new YoloOutputParser();
        static YoloML()
        {
            var ml = new MLContext();
            try
            {
                predEngine = ml.Model.CreatePredictionEngine<ImageData, ImagePrediction>(
                    //загрузка модели из ресурсов
                    ml.Model.Load(new MemoryStream(Resources.Yolo_model),
                    out var modelInputSchema));
            }
            catch { }
        }
        //принимает путь к изображению, для передачи в предикат
        static public ImagePrediction Result(string imagePath)
            => predEngine?.Predict(new ImageData { ImagePath = imagePath });
        //получает классы изображения, на основе предиката
        static public IEnumerable<YoloBoundingBox> Parser(ImagePrediction imagePrediction)
            => parser.FilterBoundingBoxes(parser.ParseOutputs(imagePrediction.PredictedLabels), 5, .5F);
        public class ImageData
        {
            [LoadColumn(0)]
            public string ImagePath;

            [LoadColumn(1)]
            public string Label;
        }
        public class ImagePrediction
        {
            [ColumnName("grid")]
            public float[] PredictedLabels;
        }
    }
}