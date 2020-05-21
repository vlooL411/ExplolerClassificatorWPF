using ExplolerClassificatorWPF.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace sajbfkabfliabfkljbsajfbsaklfajkfbaskjbfkjlb
{
    public class InfoFile
    {
        string Name { get; set; }
        DateTime Date { get; set; }
        string Extension { get; set; }
        string Length { get; set; }
        string Class { get; set; }
        static double WName { get; set; }
        static double WDate { get; set; }
        static double WExtension { get; set; }
        static double WLength { get; set; }
        static double WClass { get; set; }
    }
    static class Work
    {
        static public string[] Images = new string[] { "jpeg", "jfif", "png", "bmp", "gif", "jpg" };
        static public string[] Txt = new string[] { "txt", "bat", "asm", "c", "cpp", "c++", "cs", "c#", "h", "sql", "mysql", "html", "xaml", "js", "jquery", "java", "php", "py" };
        public static IEnumerable<object> GetListDirsFiles(string Directory, string searchPattern)
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
        public static IEnumerable<object> GetListDrives() => DriveInfo.GetDrives();
        public static bool IsExistDrive(string Name) => DriveInfo.GetDrives().Where(d => d.Name == Name).Count() != 0;
        public static bool IsReadyDrive(string Name) => DriveInfo.GetDrives().Where(d => d.IsReady && d.Name == Name).Count() != 0;

        public static byte[] GetIco(string extention = null, string fullName = null)
        {
            switch (extention?.Replace(".", "").ToLower())
            {
                case "directory":
                    return Resources.folderBlack;
                case "drive":
                    return Resources.drive;
                case "png":
                case "bmp":
                case "jpg":
                case "jpeg":
                case "jfif":
                    if (fullName == null) return Resources.pictures;
                    else return File.ReadAllBytes(fullName);

                case "exe":
                    return Resources.exe;

                case "asm":
                    return Resources.asm;
                case "h":
                case "c":
                    return Resources.c;
                case "cs":
                case "c#":
                    return Resources.c_;
                case "c++":
                    return Resources.c__;
                case "sql":
                case "mysql":
                    return Resources.sql;
                case "html":
                    return Resources.html;
                case "asp.net":
                    return Resources.asp;
                case "angulatjs":
                    return Resources.angularjs;
                case "iphone":
                    return Resources.swift;
                case "ruby-on-rails":
                    return Resources.ruby;
                case "py":
                case "python":
                    return Resources.python;
                case "php":
                    return Resources.php;
                case "jquery":
                    return Resources.jquery;
                case "xaml":
                    return Resources.xaml;
                case "jar":
                case "java":
                    return Resources.java;

                case "txt":
                case "bat":
                case "doc":
                case "docx":
                    return Resources.text_file;

                case "":
                case null:
                default:
                    return Resources.application;
            }
        }
    }
}