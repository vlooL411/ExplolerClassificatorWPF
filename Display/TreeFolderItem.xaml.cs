using ExplolerClassificatorWPF.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExplolerClassificatorWPF.Display
{
    public partial class TreeFolderItem : TreeViewItem, IContainerExploler
    {
        public TreeFolderItem() => InitializeComponent();
        void TreeFolderItem_Expanded(object o, RoutedEventArgs e)
        {
            if (e?.OriginalSource == o)
                if (o is TreeFolderItem treeFolderItem)
                    if (treeFolderItem.DataContext is InfoFile infoFile)
                    {
                        ExplolerVM.AddPathFiles(infoFile.FullName);
                        treeFolderItem.Fill(ExplolerVM.PathFiles[infoFile.FullName]);
                    }
        }
        public void Fill(IEnumerable<InfoFile> infoFiles)
        {
            ItemsSource = null;
            ItemsSource = infoFiles.Select(info => new TreeFolderItem() { DataContext = info });
        }
        void TreeViewItem_DataContextChanged(object o, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is InfoFile infoFile)
                if (infoFile.Type == TypeInfo.Any ||
                    ((infoFile.Type == TypeInfo.Drive ||
                    (infoFile.Type == TypeInfo.Directory && infoFile.Length != 0)) && infoFile.Length != -1))
                    ItemsSource = new InfoFile[] { new InfoFile() };
        }
    }
}