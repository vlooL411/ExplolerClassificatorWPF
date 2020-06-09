using System.Collections.Generic;

namespace ExplolerClassificatorWPF.Display
{
    public partial class Detail
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