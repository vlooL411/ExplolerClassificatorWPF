using ExplolerClassificatorWPF.MLModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace ExplolerClassificatorWPF
{
    public class Classif
    {
        //используется для определения цвета на экране
        public Color BoxColor { get; set; }
        //заголовок класса
        public string Label { get; set; }
    }
    public class Classification
    {
        //возвращает набор классов, в виде списка<(Color: цвет, Label: заголовок)>
        public static IEnumerable<Classif> GetClasses(string path)
        {
            IEnumerable<Classif> classes = null;
            var Extension = Path.GetExtension(path); //получаю расширение файла
            if (CheckTxt(Extension)) classes = ClassificationTxt(path); //классификация текстовых файлов
            else if (CheckImg(Extension)) classes = ClassificationImg(path); //классификация изоьражений
            return classes;
        }

        //проверка на расширение тектового файла
        static public bool CheckTxt(string extention) => WorkFile.TxtExt.Contains(extention.Replace(".", "").ToLower());
        //проверка на расширение изображения
        static public bool CheckImg(string extention) => WorkFile.ImagesExt.Contains(extention.Replace(".", "").ToLower());

        //локеры нужны, для контроля классификации, так как класс PredictionEngine, 
        //не позволяет работать больше чем с одним файлом
        static object lockResultYolo = new object();
        static object lockResultTxt = new object();
        //Classification don't do check need file extension
        static public IEnumerable<Classif> ClassificationTxt(string txtPath)
        {
            MLProgrammSelectLanguage.ModelOutput classification = null;
            lock (lockResultTxt)
                try
                {
                    //получение результата классификации
                    classification = MLProgrammSelectLanguage.Result(txtPath + File.ReadAllText(txtPath));
                }
                catch { return null; }
            //создаётся список классов
            var classifs = new List<Classif>();
            if (classification != null)
            {
                //берётся максимальный вес
                var maxGood = classification.Score.Max() - 0.01;
                //сравнивается с максимально апроксимированными значениями
                for (int i = 0; i < classification.Score.Length; i++)
                    if (classification.Score[i] >= maxGood)
                        classifs.Add(new Classif()
                        {
                            BoxColor = MLProgrammSelectLanguage.classColors[i],
                            Label = MLProgrammSelectLanguage.labels[i]
                        });
            }
            return classifs;
        }
        static public IEnumerable<Classif> ClassificationImg(string imagePath)
        {
            YoloML.ImagePrediction imagePrediction = null;
            lock (lockResultYolo)
                //получение результата классификации
                try { imagePrediction = YoloML.Result(imagePath); }
                catch { return null; }

            //получение обработанных весов
            var classification = YoloML.Parser(imagePrediction);
            //создаётся список классов
            var classifs = new List<Classif>();
            if (classification != null)
            {
                //пересчёт классов по повторяемости
                var yo = classification.Select(y => y.Label).Distinct();
                foreach (var y in yo)
                {
                    var yl = classification.Where(yl => yl.Label == y);
                    var yol = yl.First();
                    if (yl.Count() != 1)
                        yol.Label += $" {yl.Count()}";
                    classifs.Add(new Classif() { BoxColor = yol.BoxColor, Label = yol.Label });
                }
            }
            return classifs;
        }
    }
}