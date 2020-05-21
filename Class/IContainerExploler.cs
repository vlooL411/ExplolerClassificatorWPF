using ExplolerClassificatorWPF.Display;
using System.Collections.Generic;

namespace ExplolerClassificatorWPF
{
    public interface IContainerExploler
    {
        void Fill(IEnumerable<InfoFile> folders);
    }
}