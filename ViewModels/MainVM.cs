using System.Windows.Controls;

namespace ExplolerClassificatorWPF.ViewModels
{
    public class MainVM : NotifyChanged
    {
        Page _CurrentPage;
        public Page CurrentPage { get => _CurrentPage; set { _CurrentPage = value; OnPropertyChanged(nameof(CurrentPage)); } }
        public MainVM(NavigationService<Page> navigation)
        {
            navigation.NavigationChanged += page => CurrentPage = page;
            navigation.Navigate(new Exploler());
        }
    }
}