using System;
using System.Collections.Generic;
using System.Threading;

namespace ExplolerClassificatorWPF
{
    //типы содержимого
    public enum TypeInfo
    {
        File,
        Directory,
        Drive,
        Any
    }
    //состояния работы с файлом
    public enum ElemExplolerState
    {
        None,
        Load,
        Classification
    }
    //NotifyChanged - оповещает пользователя об изменении данных в классе
    public class InfoFile : NotifyChanged
    {
        IEnumerable<Classif> _Classes;
        byte[] _Image;
        byte[] _ImageExtension;
        ElemExplolerState _ElemExplolerState;

        public byte[] Image
        {
            get
            {
                if (_Image == null)
                {
                    ElemExplolerState = ElemExplolerState.Load;
                    _Image = Type switch
                    {
                        TypeInfo.File => WorkFile.Resize(WorkFile.GetIco(Extension, FullName), 70),
                        TypeInfo.Directory => Images.Directory,
                        TypeInfo.Drive => Images.Drive,
                        _ => Images.Any,
                    };
                    ElemExplolerState = ElemExplolerState.None;
                }
                return _Image;
            }
        }
        public byte[] ImageExtension
        {
            get
            {
                if (_ImageExtension == null)
                    _ImageExtension = Type switch
                    {
                        TypeInfo.File => WorkFile.GetIco(Extension),
                        TypeInfo.Directory => Images.Directory,
                        TypeInfo.Drive => Images.Drive,
                        _ => Images.Any
                    };
                return _ImageExtension;
            }
        }
        public ElemExplolerState ElemExplolerState { get => _ElemExplolerState; set { _ElemExplolerState = value; OnPropertyChanged(); } }
        public TypeInfo Type { get; set; } = TypeInfo.Any;
        public string FullName { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string Extension { get; set; }
        public long Length { get; set; } = -1;

        Thread Thread;
        //классы файла, используется только типом File
        public IEnumerable<Classif> Classes
        {
            get
            {
                if (_Classes != null) return _Classes;
                if (Thread == null && Type == TypeInfo.File)
                {
                    Thread = new Thread(() =>
                    {
                        ElemExplolerState = ElemExplolerState.Classification;
                        Classes = Classification.GetClasses(FullName);
                        ElemExplolerState = ElemExplolerState.None;
                    })
                    { IsBackground = true };
                    Thread.Start();
                }
                return null;
            }
            set { _Classes = value; OnPropertyChanged(); }
        }
    }
}