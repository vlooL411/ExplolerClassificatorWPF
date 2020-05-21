using ExplolerClassificatorWPF.Properties;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ExplolerClassificatorWPF
{
    //изображения по умолчанию
    public class Images
    {
        public static byte[] File = Resources.text_file;
        public static byte[] Directory = Resources.folderBlack;
        public static byte[] Drive = Resources.drive;
        public static byte[] Any = Resources.application;
        public static byte[] Picture = Resources.pictures;
    }
    static class WorkFile
    {
        //расширения изображений
        static public string[] ImagesExt = { "jpeg", "jfif", "png", "bmp", "gif", "jpg" };
        //текстовые расширения
        static public string[] TxtExt = { "txt", "bat", "asm", "c", "cpp", "c++", "cs", "c#", "h", "sql", "mysql", "html", "xaml", "js", "jquery", "java", "php", "py" };

        //возвращает список файлов и директорий
        public static IEnumerable<object> GetListDirsFiles(string Directory, string searchPattern = "*")
        {
            var list = new List<object>();
            var d = new DirectoryInfo(Directory);
            try
            {
                list.AddRange(d.GetDirectories(searchPattern));
                list.AddRange(d.GetFiles(searchPattern));
                return list;
            }
            catch { return null; }
        }
        //возвращает список накопителей данных
        public static IEnumerable<object> GetListDrives() => DriveInfo.GetDrives();
        //возвращает папку
        public static IEnumerable<object> GetFolder(string path, string searchPattern = "*")
        {
            //проверка папки на существование, если нету возвращает список накопителей данных
            if (Directory.Exists(path)) return GetListDirsFiles(path, searchPattern);
            else return GetListDrives();
        }

        //возвращает иконку по расширению и полному пути к файлу
        public static byte[] GetIco(string extension = null, string fullName = null)
        {
            extension = extension?.Replace(".", "").ToLower();
            if (ImagesExt.Contains(extension))
                return fullName == null ? Images.Picture : File.ReadAllBytes(fullName);
            switch (extension)
            {
                case "exe": return Resources.exe;
                case "asm": return Resources.asm;
                case "h":
                case "c": return Resources.c;
                case "cs":
                case "c#": return Resources.c_;
                case "c++":
                case "cpp": return Resources.c__;
                case "sql":
                case "mysql": return Resources.sql;
                case "html": return Resources.html;
                case "css": return Resources.css;
                case "js": return Resources.js;
                case "asp.net": return Resources.asp;
                case "angulatjs": return Resources.angularjs;
                case "iphone": return Resources.swift;
                case "ruby-on-rails": return Resources.ruby;
                case "py":
                case "python": return Resources.python;
                case "php": return Resources.php;
                case "jquery": return Resources.jquery;
                case "xaml": return Resources.xaml;
                case "json": return Resources.text_file;
                case "jar":
                case "java": return Resources.java;
                case "txt":
                case "bat":
                case "doc":
                case "docx":
                case "csv": return Resources.text_file;
                case "directory": return Resources.folderBlack;
                case "drive": return Resources.drive;
            };
            return Resources.application;
        }
        //ресайз изображения пропорионально по ширине и высоте
        public static byte[] Resize(byte[] data, int width)
        {
            var image = System.Drawing.Image.FromStream(new MemoryStream(data));

            var height = width * image.Height / image.Width;
            var thumbnail = image.GetThumbnailImage(width, height, null, IntPtr.Zero);

            var thumbnailStream = new MemoryStream();
            thumbnail.Save(thumbnailStream, ImageFormat.Png);
            return thumbnailStream.ToArray();
        }
    }
}