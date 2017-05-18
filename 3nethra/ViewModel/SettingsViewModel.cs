using Helpers;
using System;
using Artelus.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artelus.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public Action CloseAction { get; set; }
        public IdNameCollection LocationCollection { get; set; }
        public SettingsViewModel()
        {
        }
    }
}
