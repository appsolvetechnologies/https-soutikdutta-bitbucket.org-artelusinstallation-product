using Artelus.Common;
using Artelus.Model;
using Artelus.Views;
using FirstFloor.ModernUI.Windows.Controls;
using Helpers;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Input;

namespace Artelus.ViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        public string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    RaisePropertyChange("SearchText");
                }
            }
        }
        private IdName selectedOption;
        public IdName SelectedOption
        {
            get { return selectedOption; }
            set
            {
                selectedOption = value;
                RaisePropertyChange("SelectedOption");
            }
        }
        private ObservableCollection<PatientEntity> patients = new ObservableCollection<PatientEntity>();
        public ObservableCollection<PatientEntity> Patients
        {
            get { return patients; }
            set
            {
                if (value != patients)
                {
                    patients = value;
                    RaisePropertyChange("Patients");
                }
            }
        }
        public DelegateCommand ViewProfileCommand { get; set; }
        public DelegateCommand FtpTransferCommand { get; set; }
        public DelegateCommand ViewReportCommand { get; set; }
        public IdNameCollection FilterCollection { get; set; } = new IdNameCollection();
        public DelegateCommand SearchCommand { get; set; }
        public ICommand LogOffCommand { get; set; }
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
        private bool isProgressActive = false;
        public bool IsProgressActive
        {
            get { return isProgressActive; }
            set
            {
                isProgressActive = value;
                RaisePropertyChange("IsProgressActive");
            }
        }
        private void OnLogOffCommand(object args)
        {
            Helper.LogOff();
        }
        public SearchViewModel()
        {
            LogOffCommand = new DelegateCommand(OnLogOffCommand);
            Patients = new ObservableCollection<PatientEntity>();
            ViewProfileCommand = new DelegateCommand(OnViewProfileCommand);
            ViewReportCommand = new DelegateCommand(OnViewReportCommand);
            SearchCommand = new DelegateCommand(OnSearchCommand);
            Patients = new Patient().GetAll();
            FtpTransferCommand = new DelegateCommand(OnFtpTransferCommand);
            FilterCollection.Add(new IdName() { Id = "p_id", Name = "Patient ID" });
            FilterCollection.Add(new IdName() { Id = "name", Name = "Patient Name" });
            FilterCollection.Add(new IdName() { Id = "p_email", Name = "Email" });
            FilterCollection.Add(new IdName() { Id = "mobile", Name = "Mobile" });
        }

        private void OnViewProfileCommand(object args)
        {
            var model = args as PatientEntity;
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var cameraView = (win) as Artelus.MainWindow;
                    cameraView.ContentSource = new Uri("Views/PatientProfileView.xaml", UriKind.Relative);
                    cameraView.DataContext = new ProfileViewModel(model);
                }
            }
        }

        private void OnSearchCommand(object args)
        {
            if (SelectedOption.Id.ToString() == "p_id" && !string.IsNullOrEmpty(SearchText))
            {
                int n;
                if (!int.TryParse(SearchText, out n))
                {
                    ModernDialog.ShowMessage("Patient ID should be a number", "Error", MessageBoxButton.OK);
                    return;
                }
            }
            Patients = new Patient().GetAll(SelectedOption.Id.ToString(), SearchText);
        }

        private void OnViewReportCommand(object args)
        {
            var model = args as PatientEntity;
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var cameraView = (win) as Artelus.MainWindow;
                    cameraView.ContentSource = new Uri("Views/ReportView.xaml", UriKind.Relative);
                    cameraView.DataContext = new ReportViewModel(model, null);
                }
            }
        }
        private void OnFtpTransferCommand(object args)
        {
            this.IsProgressActive = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (sender, e) =>
            {

                string path = Path.Combine(Program.BaseDir(), "Uploads");
                foreach (var patient in Patients)
                {
                    PatientReports = new ObservableCollection<PatientReport>();
                    var report = new Patient().GetAllReport(patient.Id);
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
                    UploadData(patient, PatientReports);
                }
            };

            bw.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                    ModernDialog.ShowMessage("Internal Server Error. Please contact your administrator", "Alert", MessageBoxButton.OK);
                else
                    ModernDialog.ShowMessage("File transfer completed successfully", "Alert", MessageBoxButton.OK);
                this.IsProgressActive = false;
            };

            bw.RunWorkerAsync();
        }

        void UploadData(PatientEntity model, ObservableCollection<PatientReport> reports)
        {
            //try
            //{
            APIResult result = new APIResult();
            if (model.PatientId == 0)
                result = SyncPatientDetails(model, reports, false);
            else
                result = SyncPatientDetails(model, reports, true);

            if (result.status == "ok")
            {
                foreach (var item in reports)
                {
                    item.Sync = true;
                    new Patient().UpdateSyncStatus(item.Id);
                }
            }

            string localPath = Path.Combine(Program.BaseDir(), "Uploads", model.UniqueID.ToString());
            string ftpHost = ConfigurationManager.AppSettings["ftpHost"].ToString();
            string ftpUserName = ConfigurationManager.AppSettings["ftpUserName"].ToString();
            string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"].ToString();
            using (var client = new SftpClient(ftpHost, ftpUserName, ftpPassword))
            {
                client.Connect();
                client.ChangeDirectory("/home/ftpuser11/ftp/files");
                string rootDir = client.WorkingDirectory + "/" + model.UniqueID.ToString();

                if (!client.Exists(rootDir))
                    client.CreateDirectory(rootDir);
                foreach (var item in PatientReports)
                {
                    string rootDataDir = rootDir + "/" + item.UniqueID.ToString();
                    if (!client.Exists(rootDataDir))
                    {
                        client.CreateDirectory(rootDataDir);
                        client.ChangeDirectory(rootDataDir);
                        UploadDirectory(client, localPath, rootDir);
                    }
                }
                client.Disconnect();
            }
            //}
            //catch (Exception ex)
            //{
            //    ModernDialog.ShowMessage("An error has occurred on the server", "Alert", MessageBoxButton.OK);
            //    throw ex;
            //}
        }

        void UploadDirectory(SftpClient client, string localPath, string remotePath)
        {
            Console.WriteLine("Uploading directory {0} to {1}", localPath, remotePath);

            IEnumerable<FileSystemInfo> infos =
                new DirectoryInfo(localPath).EnumerateFileSystemInfos();
            foreach (FileSystemInfo info in infos)
            {
                if (info.Attributes.HasFlag(FileAttributes.Directory))
                {
                    string subPath = remotePath + "/" + info.Name;
                    if (!client.Exists(subPath))
                        client.CreateDirectory(subPath);

                    UploadDirectory(client, info.FullName, remotePath + "/" + info.Name);
                }
                else
                {
                    using (Stream fileStream = new FileStream(info.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        Console.WriteLine(
                            "Uploading {0} ({1:N0} bytes)", info.FullName, ((FileInfo)info).Length);
                        if (info.Name != "report.html")
                            client.UploadFile(fileStream, remotePath + "/" + info.Name);
                    }
                }
            }
        }

        private APIResult SyncPatientDetails(PatientEntity patient, ObservableCollection<PatientReport> reports, bool isUpdate)
        {
            APIResult result = new APIResult();
            string token = new User().GetToken(Program.UserId());
            string url = string.Empty;
            if (isUpdate)
                url = ConfigurationManager.AppSettings["updatePatientAPI"].ToString() + "/" + patient.PatientId + "?token=" + token;
            else
                url = ConfigurationManager.AppSettings["addPatientAPI"].ToString() + "?token=" + token;

            object objct = new
            {
                p_id = patient.Id,
                name = patient.Nm,
                pMName = patient.MNm,
                pLName = patient.LNm,
                notResident = patient.NotResident,
                ifResidentOfM = patient.IfResidentOfM,
                IcNumber = patient.IcNumber,
                otherOption = patient.OtherOption,
                othersID = patient.OthersID,
                doctosName = patient.DocNm,
                hospitalScreening = patient.HospitalScreening,
                hospitalName = patient.HospitalNm,
                hospitalID = patient.HospitalID,
                p_email = patient.Email,
                marital_status = patient.MaritalStatus == "Married" ? "Yes" : "No",
                age = patient.Age,
                sex = patient.Sex == "Male" ? "m" : "f",
                permanent_address = patient.PerAdr,
                area = patient.Area,
                phone_res = patient.ResidentPh,
                mobile = patient.Mob,
                occupation = patient.Occupation,
                working_at = patient.WorkingAt,
                currentMedications = patient.CurrentMedications,
                laser_treatment = patient.LaserTreatment,
                have_cataract = patient.Cataract,
                have_hypertension = patient.Hypertension,
                allergy_to_drugs = patient.AllergyDrugs,
                have_diabetes = patient.Diabetic,
                additional_info = patient.Info,
                emg_contact_name = patient.EmergContactNm,
                emg_phone = patient.EmergPh,
                name_of_the_stated_onsent = patient.StatedConsentPerson,
                relation_with_patient = patient.Relation,
                term_conditation = patient.TermsCondition,
                collection_id = patient.CollectionID,
                install_id = patient.InstallID,
                update_at = patient.MDt.Ticks,
                create_at = patient.CDt.Ticks,
                UniqueID = patient.UniqueID,
                allergy_drugs_details = patient.AllergyDrugsDtl,
                MedicalInsurance = patient.MedicalInsurance
            };
            var json = new JavaScriptSerializer().Serialize(objct);
            //try
            //{
            result = RestCalls.SyncReport(url, json, isUpdate);
            if (result.status == "ok")
            {
                if (isUpdate)
                {
                    bool newReport = reports.Any(x => x.Sync == false);
                    if (newReport && reports.Count > 0)
                        result = SyncPatientReport(reports, patient.PatientId);
                }
                else
                {
                    patient.PatientId = result.user;
                    new Patient().UpdatePatientId(patient.Id, result.user);
                    if (reports.Count > 0)
                        SyncPatientReport(reports, result.user);
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.InnerException.Message);
            //    ModernDialog.ShowMessage("An error has occurred on the server", "Alert", MessageBoxButton.OK);
            //}
            return result;
        }
        private APIResult SyncPatientReport(ObservableCollection<PatientReport> reports, int patientId)
        {
            APIResult result = new APIResult();
            string token = new User().GetToken(Program.UserId());
            string url = ConfigurationManager.AppSettings["patientReportAPI"].ToString() + "?token=" + token;
            List<object> reportList = new List<object>();

            foreach (var item in reports)
            {
                object objct = new
                {
                    PatientId = patientId,
                    Dt = item.Dt.Ticks,
                    Location = item.Location,
                    InstallID = item.InstallID,
                    UniqueID = item.UniqueID,
                    ReportData = ReportDataObject(item.ReportDatas, patientId)
                };
                reportList.Add(objct);
            }
            var json = new JavaScriptSerializer().Serialize(reportList);
            //try
            //{
            result = RestCalls.SyncReport(url, json, false);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.InnerException.Message);
            //    ModernDialog.ShowMessage("An error has occurred on the server", "Alert", MessageBoxButton.OK);
            //}
            return result;
        }

        private List<object> ReportDataObject(ObservableCollection<ReportData> reportDatas, int patientId)
        {
            List<object> dataList = new List<object>();
            foreach (var item in reportDatas)
            {
                item.PatientId = patientId;
                object objct = new
                {
                    PatientId = item.PatientId,
                    PatientReportId = item.PatientReportId,
                    Mode = item.Mode,
                    Img = item.Img,
                    Eye = item.Eye,
                    Prediction = item.Prediction,
                    Size = item.Size
                };
                dataList.Add(objct);
            }
            return dataList;
        }
    }
}
