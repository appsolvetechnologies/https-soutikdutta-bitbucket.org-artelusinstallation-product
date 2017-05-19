using Artelus.Model;
using Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artelus.ViewModel
{
    public class PredictionViewModel : BaseViewModel
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
        private PatientReport patientReport;
        public PatientReport PatientReport
        {
            get { return patientReport; }
            set
            {
                patientReport = value;
                RaisePropertyChange("PatientReport");
            }
        }
        private string hansanet = "Disable Hansanet";
        public string Hansanet
        {
            get { return hansanet; }
            set
            {
                hansanet = value;
                RaisePropertyChange("Hansanet");
            }
        }
        public DelegateCommand SelectAllCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand SetHansanetCommand { get; set; }

        public PredictionViewModel(PatientEntity model)
        {
            SetHansanetCommand = new DelegateCommand(OnSetHansanetCommand);
            SelectAllCommand = new DelegateCommand(OnSelectAllComand);
            SaveCommand = new DelegateCommand(OnSaveCommand);

            PatientEntity = model;
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(rootPath, "Uploads");
            PatientReport = new PatientReport();
            PatientReport = new Patient().GetLastestReport(model.Id);
            if (PatientReport != null)
            {
                var osResult = new Patient().GetOSReportData(PatientReport.Id, true);
                foreach (var data in osResult)
                {
                    PatientReport.OSPosteriorReportDatas = new ObservableCollection<ReportData>();
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    PatientReport.OSPosteriorReportDatas.Add(data);
                }
                var odResult = new Patient().GetODReportData(PatientReport.Id, true);
                foreach (var data in odResult)
                {
                    PatientReport.ODPosteriorReportDatas = new ObservableCollection<ReportData>();
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    PatientReport.ODPosteriorReportDatas.Add(data);
                }
            }
        }

        private void OnSetHansanetCommand(object args)
        {
            if (this.hansanet == "Disable Hansanet")
                this.Hansanet = "Enable Hansanet";
            else
                this.Hansanet = "Disable Hansanet";
        }


        private void OnSelectAllComand(object args)
        {
            foreach (var item in PatientReport.OSPosteriorReportDatas)
                item.IsChecked = true;

            foreach (var item in PatientReport.ODPosteriorReportDatas)
                item.IsChecked = true;
        }
        private void OnSaveCommand(object args)
        {

        }
    }
}
