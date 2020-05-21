using System;

namespace ExplolerClassificatorWPF
{
    public class NavigationService<T>
    {
        public event Action<T> NavigationChanged;
        public void Navigate(T nav) => NavigationChanged?.Invoke(nav);
    }
}