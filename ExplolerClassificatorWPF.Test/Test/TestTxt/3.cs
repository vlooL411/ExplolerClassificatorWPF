using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExplolerClassificatorWPF.Test.Test.TestTxt
{
    public class slahfiobsavubxoibvisafiubaufbspuahf92h3rno23of2oinf3
    {
        Page _CurrentPage;
        public Page CurrentPage { get => _CurrentPage; set { _CurrentPage = value; /*OnPropertyChanged(nameof(CurrentPage));*/ } }
        public slahfiobsavubxoibvisafiubaufbspuahf92h3rno23of2oinf3(NavigationService<Page> navigation)
        {
            navigation.NavigationChanged += page => CurrentPage = page;
            navigation.Navigate(new Exploler());
        }
    }
}
