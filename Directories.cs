using System;

namespace ExplolerClassificatorWPF
{
    public class Directories
    {
        public static string Desktop => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string Documents => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string Downloads => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads";
        public static string Music => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        public static string Pictures => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        public static string Videos => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
    }
}