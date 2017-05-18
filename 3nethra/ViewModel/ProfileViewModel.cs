using Artelus.Model;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artelus.ViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        private PatientEntity patient;
        public PatientEntity PatientEntity
        {
            get { return patient; }
            set
            {
                patient = value;
                RaisePropertyChange("PatientEntity");

            }
        }
        private ReportData reportData;
        public ReportData ReportData
        {
            get { return reportData; }
            set
            {
                reportData = value;
                RaisePropertyChange("ReportData");
            }
        }
        //public ProfileViewModel()
        //{

        //}
        public ProfileViewModel(PatientEntity model)
        {
            PatientEntity = model;
        }
    }
}
