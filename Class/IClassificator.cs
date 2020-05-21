using System;
using System.Collections.Generic;

namespace ExplolerClassificatorWPF
{
    public interface IClassificator
    {
        event EventHandler BeginClassification;
        event EventHandler EndClassification;
        event EventHandler AddClassification;
        event EventHandler RemoveClassification;

        void GetClasses(string path);
        void Classification(IEnumerable<InfoFile> folders);
    }
}