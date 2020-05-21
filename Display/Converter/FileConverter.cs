using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace ExplolerClassificatorWPF.Display.Converter
{
    class FileConverter : IValueConverter
    {
        public object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            if (value is InfoFile) return value;
            if (value is FileInfo file)
                return new InfoFile()
                {
                    Type = TypeInfo.File,
                    FullName = file.FullName,
                    Name = file.Name,
                    CreateDate = file.CreationTime,
                    Extension = file.Extension,
                    Length = file.Length
                };
            if (value is DirectoryInfo dir)
            {
                var infoFile = new InfoFile()
                {
                    Type = TypeInfo.Directory,
                    FullName = dir.FullName,
                    Name = dir.Name,
                    CreateDate = dir.CreationTime,
                };
                try { infoFile.Length = WorkFile.GetListDirsFiles(dir.FullName).Count(); }
                catch { }
                return infoFile;
            }
            if (value is DriveInfo drive)
            {
                var infoFile = new InfoFile()
                {
                    Type = TypeInfo.Drive,
                    FullName = drive.RootDirectory.FullName,
                    Name = drive.Name,
                };
                try { infoFile.Length = drive.TotalSize; }
                catch { }
                return infoFile;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}