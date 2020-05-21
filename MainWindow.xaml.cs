using ExplolerClassificatorWPF.Display;
using System.Reflection;
using System.Windows;

namespace ExplolerClassificatorWPF
{
    public partial class MainWindow : Window
    {
        static public StatusBar StatusBar { get; set; } = new StatusBar() { VerticalAlignment = VerticalAlignment.Bottom };
        public MainWindow()
        {
            InitializeComponent();
            grid.Children.Add(StatusBar);
            StatusBar.TextRight.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}