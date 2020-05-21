using Microsoft.Extensions.DependencyInjection;

namespace ExplolerClassificatorWPF
{
    public class Locator
    {
        static ServiceProvider provider;
        public Locator(ServiceCollection services) => provider = services.BuildServiceProvider();
        public T Resolve<T>() => provider.GetRequiredService<T>();
    }
}