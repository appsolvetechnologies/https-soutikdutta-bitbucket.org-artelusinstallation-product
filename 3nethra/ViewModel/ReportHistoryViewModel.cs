using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artelus.ViewModel
{
    public class ReportHistoryViewModel : BaseViewModel
    {
        public ReportHistoryViewModel() {

        }
            
        public ReportHistoryViewModel(Model.PatientEntity patiententity)
        {

        }
        public Action CloseAction { get; set; }
    }
}
