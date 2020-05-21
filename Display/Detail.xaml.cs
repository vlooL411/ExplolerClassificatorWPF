using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ExplolerClassificatorWPF.Display
{
    public partial class Detail : IContainerExploler
    {
        public Detail()
        {
            InitializeComponent();
        }
        public void Fill(IEnumerable<InfoFile> folders)
        {
            Explol.ItemsSource = null;
            Explol.ItemsSource = folders;
        }
    }
}