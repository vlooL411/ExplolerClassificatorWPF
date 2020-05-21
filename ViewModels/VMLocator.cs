using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace ExplolerClassificatorWPF.ViewModels
{
    public class VMLocator : Locator
    {
        static ServiceCollection services = new ServiceCollection();
        static VMLocator()
        {
            services.AddSingleton<MainVM>();
            services.AddSingleton<ExplolerVM>();
            services.AddSingleton<NavigationService<Page>>();
        }
        public VMLocator() : base(services) { }
        public MainVM MainVM => Resolve<MainVM>();
        public ExplolerVM ExplolerVM => Resolve<ExplolerVM>();
        public NavigationService<Page> NavigationServicePage => Resolve<NavigationService<Page>>();
    }
}