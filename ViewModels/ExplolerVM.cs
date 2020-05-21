using ExplolerClassificatorWPF.Display;
using ExplolerClassificatorWPF.Display.Converter;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ExplolerClassificatorWPF.ViewModels
{
    //режимы системы
    public enum ExplolerMode
    {
        Display,
        Search,
        Classification
    }
    //модель представления Eploler
    public class ExplolerVM : NotifyChanged
    {
        //текущий режим системы
        public ExplolerMode ExplolerMode { get; set; } = ExplolerMode.Display;
        //путь по умолчанию
        public static string DefaultPath => "This PC";
        //идентификатор текущего пути
        int _CurrentPathId = 0;
        public int CurrentPathId
        {
            get => _CurrentPathId;
            set
            {
                if (value < 0) _CurrentPathId = 0;
                else if (HistrotyPaths.Any() && value > HistrotyPaths.Count - 1) _CurrentPathId = HistrotyPaths.Count - 1;
                else _CurrentPathId = value;
            }
        }
        //список файлового описания для поиска
        IEnumerable<InfoFile> _SearchPath;
        //текущий набор файлов и директорий
        int StatusBarLastElemCP = 0;
        public IEnumerable<InfoFile> CurrentPath
        {
            get
            {
                IEnumerable<InfoFile> current = null;
                var status = "";
                if (ExplolerMode == ExplolerMode.Display)
                {
                    current = HistrotyPaths.Any() && PathFiles.ContainsKey(HistrotyPaths[CurrentPathId])
                                ? PathFiles[HistrotyPaths[CurrentPathId]]
                                : null;
                    status = $"Items: {current?.Count()}";
                }
                else if (ExplolerMode == ExplolerMode.Search)
                {
                    current = _SearchPath;
                    status = $"Result search: {_SearchPath?.Count()}/{PathFiles[HistrotyPaths[CurrentPathId]]?.Count()}";
                }
                //передача состояния статус бару
                MainWindow.StatusBar.Dispatcher?.Invoke(()
                    => StatusBarLastElemCP = MainWindow.StatusBar.RemoveAdd(StatusBarLastElemCP, status));
                return current;
            }
        }
        //история переходов
        public List<string> HistrotyPaths { get; } = new List<string>();
        //словарь директорий
        public static Dictionary<string, IEnumerable<InfoFile>> PathFiles { get; } = new Dictionary<string, IEnumerable<InfoFile>>();

        //установка пути по умолчанию
        public ExplolerVM() => Path = DefaultPath;

        string _Path;
        string _SearchPattern = "";
        //текущий путь
        public string Path
        {
            get => _Path;
            set
            {
                ExplolerMode = ExplolerMode.Display;
                if (_Path == value) return;
                if (value != DefaultPath && !Directory.Exists(value)) return;
                if (!AddPathFiles(value)) return;
                _Path = value;

                if (HistrotyPaths.Any())
                {
                    HistrotyPaths.Insert(++_CurrentPathId, _Path);
                    HistrotyPaths.RemoveRange(CurrentPathId, HistrotyPaths.Count - CurrentPathId - 1);
                }
                else HistrotyPaths.Add(_Path);

                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentPath));
            }
        }
        //текущий путь поиска
        public string SearchPattern { get => _SearchPattern; set { _SearchPattern = value == "" || value == null ? "" : value; } }

        //ключ последнего переданого элемента в статус бар
        static int StatusBarLastElemAddPF = 0;
        //добавление файлов и директорий в словарь (PathFiles), используется как кэш
        public static bool AddPathFiles(string path)
        {
            if (!PathFiles.ContainsKey(path))
                try
                {
                    PathFiles.Add(path, WorkFile.GetFolder(path).AsParallel().AsOrdered()
                                                .Select(s => new FileConverter().Convert(s) as InfoFile));
                }
                catch
                {

                    MainWindow.StatusBar.Dispatcher?.Invoke(()
                        => StatusBarLastElemAddPF = MainWindow.StatusBar.RemoveAdd(StatusBarLastElemAddPF, $"Нет доступа {path}"));
                    return false;
                }
            return true;
        }

        //команда для изменения пути
        public ICommand ChangePath => new DelegateCommand((o) =>
        {
            if (o is TreeFolderItem tree)
            {
                if (tree?.DataContext is InfoFile infoFileTree)
                    Path = infoFileTree.FullName;
            }
            else if (o is InfoFile infoFile) Path = infoFile.FullName;
            else if (o is string path) Path = path;
        });
        //команда возврата на предыдущий путь в истории путей
        public ICommand Left => new DelegateCommand((o) =>
        {
            _Path = HistrotyPaths[--CurrentPathId];
            OnPropertyChanged(nameof(Path));
            OnPropertyChanged(nameof(CurrentPath));
        }, (o) => HistrotyPaths.Any() && CurrentPathId - 1 > 0);
        //команда перехода на следующий путь в истории путей
        public ICommand Right => new DelegateCommand((o) =>
        {
            _Path = HistrotyPaths[++CurrentPathId];
            OnPropertyChanged(nameof(Path));
            OnPropertyChanged(nameof(CurrentPath));
        }, (o) => HistrotyPaths.Any() && CurrentPathId < HistrotyPaths.Count - 1);
        //команда перезагрузки
        public ICommand Refresh => new DelegateCommand((o) => OnPropertyChanged(nameof(CurrentPath)),
                                                       (o) => HistrotyPaths.Any());

        //команда поиска по файлам и директориям
        public ICommand Search => new DelegateCommand((o) =>
        {
            if (o is string searchP) SearchPattern = searchP;
            var _SearchPattern = SearchPattern?.Replace(@"\", @"\\");
            _SearchPath = PathFiles[HistrotyPaths[CurrentPathId]].AsParallel().AsOrdered()
                          .Where(p => Regex.IsMatch(p?.Name, _SearchPattern) ||
                                      (p.Classes != null && p.Classes.Where(c => Regex.IsMatch(c?.Label, _SearchPattern)).Count() != 0))
                          .ToList();
            if (_SearchPath != null)
            {
                ExplolerMode = ExplolerMode.Search;
                OnPropertyChanged(nameof(CurrentPath));
            }
            else ExplolerMode = ExplolerMode.Display;
        }, (o) => HistrotyPaths.Any() && PathFiles.ContainsKey(HistrotyPaths[CurrentPathId]));
    }
}