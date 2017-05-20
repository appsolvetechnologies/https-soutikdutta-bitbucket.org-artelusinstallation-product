using Artelus.Model;
using Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Artelus.ViewModel
{
    public class ProfileViewModel : BaseViewModel
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
        private ObservableCollection<ReportData> _OSReportDatas = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> OSReportDatas
        {
            get { return _OSReportDatas; }
            set
            {
                if (value != _OSReportDatas)
                {
                    _OSReportDatas = value;
                    RaisePropertyChange("OSReportDatas");
                }
            }
        }
        private ObservableCollection<ReportData> _ODReportDatas = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> ODReportDatas
        {
            get { return _ODReportDatas; }
            set
            {
                if (value != _ODReportDatas)
                {
                    _ODReportDatas = value;
                    RaisePropertyChange("ODReportDatas");
                }
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
        public DelegateCommand EditProfileDataCommand { get; set; }
        public DelegateCommand ViewReportDataCommand { get; set; }
        public DelegateCommand ViewAllImagesCommand { get; set; }
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

        public DelegateCommand PreviousReportCommand { get; set; }

        public ProfileViewModel(PatientEntity model)
        {
            PreviousReportCommand = new DelegateCommand(OnPreviousReportCommand);
            EditProfileDataCommand = new DelegateCommand(OnEditProfileDataCommand);
            ViewReportDataCommand = new DelegateCommand(OnViewReportDataCommand);
            ViewAllImagesCommand = new DelegateCommand(OnViewAllImagesCommand);
            PatientEntity = model;
            if (PatientEntity.Sex == "m")
                PatientEntity.Sex = "Male";
            else
                PatientEntity.Sex = "Female";

            if (PatientEntity.MaritalStatus == "no")
                PatientEntity.MaritalStatus = "Single";
            else
                PatientEntity.MaritalStatus = "Married";

            if (PatientEntity.AllergyDrugs == "yes")
                ShowAllergyOption = true;

            if (PatientEntity.IfResidentOfM == "yes")
            {
                PatientEntity.OtherOption = "IC Number:";
                PatientEntity.OthersID = PatientEntity.IcNumber;
            }

            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(rootPath, "Uploads");
            PatientReports = new ObservableCollection<PatientReport>();
            PatientReport = new Patient().GetLastestReport(model.Id);
            if (PatientReport != null)
            {
                var osResult = new Patient().GetPosteriorOSReportData(PatientReport.Id,false);
                foreach (var data in osResult)
                {
                    PatientReport.OSPosteriorReportDatas = new ObservableCollection<ReportData>();
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    PatientReport.OSPosteriorReportDatas.Add(data);
                }
                var odResult = new Patient().GetPosteriorODReportData(PatientReport.Id,false);
                foreach (var data in odResult)
                {
                    PatientReport.ODPosteriorReportDatas = new ObservableCollection<ReportData>();
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    PatientReport.ODPosteriorReportDatas.Add(data);
                }
            }
            PatientReports.Add(PatientReport);
        }

        private void OnPreviousReportCommand(object args)
        {

            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var reportView = (win) as Artelus.MainWindow;
                    reportView.ContentSource = new Uri("Views/ReportHistoryView.xaml", UriKind.Relative);
                    reportView.DataContext = new ReportHistoryViewModel(PatientEntity);
                }
            }            
        }


        private void OnEditProfileDataCommand(object args)
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var profileView = (win) as Artelus.MainWindow;
                    profileView.ContentSource = new Uri("Views/PatientEditView.xaml", UriKind.Relative);
                    profileView.DataContext = new PatientViewModel(PatientEntity);
                }
            }
        }

        private void OnViewReportDataCommand(object args)
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var reportView = (win) as Artelus.MainWindow;
                    reportView.ContentSource = new Uri("Views/ReportView.xaml", UriKind.Relative);
                    reportView.DataContext = new ReportViewModel(PatientEntity,null);
                }
            }
        }

        private void OnViewAllImagesCommand(object args)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(rootPath, "Uploads");
            PatientReports = new ObservableCollection<PatientReport>();
            var patientReports = new Patient().GetAllReport(PatientEntity.Id);
            foreach (var item in patientReports)
            {
                var osResult = new Patient().GetPosteriorOSReportData(item.Id, false);
                foreach (var data in osResult)
                {
                    item.OSPosteriorReportDatas = new ObservableCollection<ReportData>();
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    item.OSPosteriorReportDatas.Add(data);
                }
                var odResult = new Patient().GetPosteriorODReportData(item.Id, false);
                foreach (var data in odResult)
                {
                    item.ODPosteriorReportDatas = new ObservableCollection<ReportData>();
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    item.ODPosteriorReportDatas.Add(data);
                }

                PatientReports.Add(item);
            }

        }
    }
}
