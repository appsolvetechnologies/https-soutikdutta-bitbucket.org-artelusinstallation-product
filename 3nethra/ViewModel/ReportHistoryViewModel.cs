using Artelus.Common;
using Artelus.Model;
using Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Artelus.ViewModel
{
    public class ReportHistoryViewModel : BaseViewModel
    {

        private bool showAllergyOption;
        public bool ShowAllergyOption
        {
            get { return showAllergyOption; }
            set
            {
                showAllergyOption = value;
                RaisePropertyChange("ShowAllergyOption");
            }
        }
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
        private ObservableCollection<PatientReport> patientReports = new ObservableCollection<PatientReport>();
        public ObservableCollection<PatientReport> PatientReports
        {
            get { return patientReports; }
            set
            {
                if (value != patientReports)
                {
                    patientReports = value;
                    RaisePropertyChange("PatientReports");
                }
            }
        }
        public ICommand LogOffCommand { get; set; }
        public DelegateCommand ShowImageCommand { get; set; }

        private void OnLogOffCommand(object args)
        {
            Helper.LogOff();
        }
        public DelegateCommand ViewReportDataCommand { get; set; }

        public ReportHistoryViewModel(PatientEntity model)
        {
            ShowImageCommand = new DelegateCommand(OnShowImageCommand);
            LogOffCommand = new DelegateCommand(OnLogOffCommand);
            PatientEntity = model;

            if (PatientEntity.MaritalStatus == "No")
                PatientEntity.MaritalStatus = "Single";
            else
                PatientEntity.MaritalStatus = "Married";

            if (PatientEntity.Sex == "m")
                PatientEntity.Sex = "Male";
            else
                PatientEntity.Sex = "Female";

            if (PatientEntity.AllergyDrugs == "Yes")
                ShowAllergyOption = true;

            if (PatientEntity.IfResidentOfM == "Yes")
            {
                PatientEntity.OtherOption = "IC Number:";
                PatientEntity.OthersID = PatientEntity.IcNumber;
            }
            else
                PatientEntity.OtherOption = PatientEntity.OtherOption + ":";

            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(rootPath, "Uploads");
            var report = new Patient().GetAllReport(model.Id);
            foreach (var item in report)
            {
                var result = new Patient().GetAllReportData(item.Id);
                item.ReportDatas = new ObservableCollection<ReportData>();
                foreach (var data in result)
                {
                    data.ImageUrl = Path.Combine(path, data.Img);
                    item.ReportDatas.Add(data);
                }
                PatientReports.Add(item);
            }
            ViewReportDataCommand = new DelegateCommand(OnViewReportDataCommand);
        }
        private void OnShowImageCommand(object args)
        {
            string imagePath = args as string;
            Process.Start(imagePath);
        }
        private void OnViewReportDataCommand(object args)
        {

            var model = args as PatientReport;
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var cameraView = (win) as Artelus.MainWindow;
                    cameraView.ContentSource = new Uri("Views/ReportView.xaml", UriKind.Relative);
                    cameraView.DataContext = new ReportViewModel(PatientEntity,model);
                }
            }
        }
        public Action CloseAction { get; set; }
    }
}
