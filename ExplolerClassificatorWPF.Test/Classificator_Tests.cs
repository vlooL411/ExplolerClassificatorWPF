using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExplolerClassificatorWPF.Test
{
    public class Classificator_Tests
    {
        static string FullName = Directory.GetCurrentDirectory() + "/../../../";
        static string Test = FullName + "Test/";
        static string TestImages = Test + "TestImages/";
        static string TestTxt = Test + "TestTxt/";

        [Test] //тест 5 изображений котов
        public void Classification_FiveCats_Test()
        {
            var cats = new string[] { "0.jpg", "1.jpg", "2.jpg", "3.jpg", "image2.jpg" };
            Assert.IsTrue(Classification_T(cats, TestImages, "cat", Classification.ClassificationImg) > 75);
        }
        [Test] //тест 5 изображений собак
        public void Classification_FiveDogs_Test()
        {
            var dogs = new string[] { "18.jpg", "19.jpg", "20.jpg", "image4.jpg", "image2.jpg" };
            Assert.IsTrue(Classification_T(dogs, TestImages, "dog", Classification.ClassificationImg) > 75);
        }
        [Test] //тест 5 изображений людей
        public void Classification_FivePerson_Test()
        {
            var dogs = new string[] { "Geralt_sword-size_1920x1080.png",
                                      "image4.jpg",
                                      "dual_monitor_wallpapers_geralt&ciri_pack_left_EN.png",
                                      "dual_monitor_wallpapers_geralt&ciri_pack_right_EN.png",
                                      "Geralt_Yennefer-size_1920x1080.png" };
            Assert.IsTrue(Classification_T(dogs, TestImages, "person", Classification.ClassificationImg) > 75);
        }

        [Test] //тест 5 файлов c# кода
        public void Classification_FiveCS_Test()
        {
            var CSs = new string[] { "0.cs", "1.cs", "2.cs", "3.cs", "4.cs" };
            Assert.IsTrue(Classification_T(CSs, TestTxt, "cs|c#|.net|asp.net", Classification.ClassificationTxt) > 75);
        }
        [Test] //тест 5 файлов xaml разметки
        public void Classification_FiveXAML_Test()
        {
            var xamls = new string[] { "0.xaml", "1.xaml", "2.xaml", "3.xaml", "4.xaml" };
            Assert.IsTrue(Classification_T(xamls, TestTxt, "xaml|cs|c#|.net|asp.net", Classification.ClassificationTxt) > 75);
        }

        public float Classification_T(string[] paths, string root, string pattern, Func<string, IEnumerable<Classif>> func)
        {
            if (paths.Length == 0) return 0;
            var count = 0f;
            foreach (var path in paths)
            {
                var classes = func?.Invoke(root + path);
                if (classes != null)
                    if (classes.Where(c => Regex.IsMatch(c.Label.ToLower(), $"{pattern}")).ToList().Any())
                        count++;
            }
            return count / paths.Length * 100;
            //(cs)(c#)(.net)(asp.net)
        }
    }
}