using Artelus.Common;
using Artelus.Model;
using Artelus.Views;
using FirstFloor.ModernUI.Windows.Controls;
using Helpers;
using Renci.SshNet;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Artelus.ViewModel
{

    public class ReportViewModel : BaseViewModel
    {
        private string predictionResult;
        public string PredictionResult
        {
            get { return predictionResult; }
            set
            {
                predictionResult = value;
                RaisePropertyChange("PredictionResult");
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
        private PatientEntity patient;
        public PatientEntity PatientEntity
        {
            get { return patient; }
            set { patient = value; }
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

        private bool showReport;
        public bool ShowReport
        {
            get { return showReport; }
            set
            {
                if (value != showReport)
                {
                    showReport = value;
                    RaisePropertyChange("ShowReport");
                }
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

        private ObservableCollection<ReportData> reportDatas = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> ReportDatas
        {
            get { return reportDatas; }
            set
            {
                if (value != reportDatas)
                {
                    reportDatas = value;
                    RaisePropertyChange("ReportDatas");
                }
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
        public DelegateCommand TakeReportCommand { get; set; }
        public DelegateCommand FtpTransferCommand { get; set; }
        public DelegateCommand ViewReportDataCommand { get; set; }
        public DelegateCommand PreviousReportCommand { get; set; }
        public DelegateCommand SaveNextCommand { get; set; }
        public DelegateCommand SaveExitCommand { get; set; }
        public DelegateCommand SendMailCommand { get; set; }
        public DelegateCommand BackCommand { get; set; }
        public DelegateCommand ViewPDFCommand { get; set; }
        public DelegateCommand PrintPDFCommand { get; set; }
        public DelegateCommand ShowImageCommand { get; set; }

        public Action CloseAction { get; set; }
        public ICommand LogOffCommand { get; set; }

        private void OnLogOffCommand(object args)
        {
            Helper.LogOff();
        }

        public ReportViewModel(PatientEntity model, PatientReport obj)
        {
            ShowImageCommand = new DelegateCommand(OnShowImageCommand);
            BackCommand = new DelegateCommand(OnBackCommand);
            LogOffCommand = new DelegateCommand(OnLogOffCommand);
            PrintPDFCommand = new DelegateCommand(OnPrintPDFCommand);
            string path = Path.Combine(Program.BaseDir(), "Uploads");
            ReportDatas = new ObservableCollection<ReportData>();
            PatientEntity = model;
            if (PatientEntity.Sex == "m")
                PatientEntity.Sex = "Male";
            else
                PatientEntity.Sex = "Female";

            if (PatientEntity.MaritalStatus == "No")
                PatientEntity.MaritalStatus = "Single";
            else
                PatientEntity.MaritalStatus = "Married";

            if (PatientEntity.IfResidentOfM == "Yes")
            {
                PatientEntity.OtherOption = "IC Number:";
                PatientEntity.OthersID = PatientEntity.IcNumber;
            }
            else
                PatientEntity.OtherOption = PatientEntity.OtherOption + ":";

            if (obj != null)
                PatientReport = obj;
            else
                PatientReport = new Patient().GetLastestReport(model.Id);

            if (PatientReport != null)
            {
                var osResult = new Patient().GetPosteriorOSReportData(PatientReport.Id, false);
                var odResult = new Patient().GetPosteriorODReportData(PatientReport.Id, false);
                bool docReview = osResult.Any(x => x.Prediction == "Doctor review recommended");
                if (!docReview)
                    docReview = odResult.Any(x => x.Prediction == "Doctor review recommended");

                bool noDR = osResult.Any(x => x.Prediction == "No DR detected");
                if (!noDR)
                    noDR = odResult.Any(x => x.Prediction == "No DR detected");

                bool badImg = osResult.Any(x => x.Prediction == "Bad Image");
                if (!badImg)
                    badImg = odResult.Any(x => x.Prediction == "Bad Image");

                if (docReview)
                    PredictionResult = "EXAMINATION RESULT: Diabetic Retinopathy Suspected - Doctor Review Recommended                    * KINDLY CORRELATE CLINICALLY *";
                else if (noDR)
                    PredictionResult = "EXAMINATION RESULT: No Abnormlities detected                    * KINDLY CORRELATE CLINICALLY *";
                else if (badImg)
                    PredictionResult = "EXAMINATION RESULT: Bad Image                    * KINDLY CORRELATE CLINICALLY *";
                else
                    PredictionResult = "EXAMINATION RESULT:                     * KINDLY CORRELATE CLINICALLY *";

                foreach (var data in osResult)
                {
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    OSReportDatas.Add(data);
                }
                foreach (var data in odResult)
                {
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    ODReportDatas.Add(data);
                }
            }

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

            TakeReportCommand = new DelegateCommand(OnTakeReportCommand);
            PreviousReportCommand = new DelegateCommand(OnPreviousReportCommand);
            FtpTransferCommand = new DelegateCommand(OnFtpTransferCommand);
            ViewReportDataCommand = new DelegateCommand(OnViewReportDataCommand);
            SaveNextCommand = new DelegateCommand(OnSaveNextCommand);
            SaveExitCommand = new DelegateCommand(OnSaveExitCommand);
            SendMailCommand = new DelegateCommand(OnSendMailCommand);
            ViewPDFCommand = new DelegateCommand(OnViewPDFCommand);
            CreateReport();
        }

        private void OnShowImageCommand(object args)
        {
            string imagePath = args as string;
            Process.Start(imagePath);
        }

        private void OnViewPDFCommand(object args)
        {
            string dir = Path.Combine(Program.BaseDir(), "Uploads", PatientEntity.UniqueID.ToString(), PatientReport.UniqueID.ToString());
            string filePDF = Path.Combine(dir, "report.pdf");
            var pdfVM = new PDFViewModel(filePDF);
            var window = new ModernWindow
            {
                Style = (Style)App.Current.Resources["BlankWindow"],
                Title = "Prediction Result",
                IsTitleVisible = true
            };
            window.Content = new PDFView(pdfVM, window);
            var closeResult = window.ShowDialog();
        }

        void CreateReport()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (sender, e) =>
            {
                string path = Path.Combine(Program.BaseDir(), "Uploads");
                string text = Common.Helper.ReadAllTextReportFile();
                string dir = Path.Combine(Program.BaseDir(), "Uploads", PatientEntity.UniqueID.ToString(), PatientReport.UniqueID.ToString());
                string filePDF = Path.Combine(dir, "report.pdf");
                string fileHTML = Path.Combine(dir, "report.html");

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                else if (File.Exists(filePDF))
                    File.Delete(filePDF);

                string posteriorHTML = "<tr>";
                string anteriorHTML = "<tr>";
                string prediction = string.Empty;
                int posteriorCount = 0, anteriorCount = 0;
                List<ReportData> posteriorDatas = new Patient().GetPosteriorReportData(PatientReport.Id);
                List<ReportData> anteriorDatas = new Patient().GetAnteriorReportData(PatientReport.Id);

                if (posteriorDatas != null)
                {
                    bool docReview = posteriorDatas.Any(x => x.Prediction == "Doctor review recommended");
                    bool noDR = posteriorDatas.Any(x => x.Prediction == "No DR detected");

                    if (docReview)
                        prediction = "EXAMINATION RESULT: Diabetic Retinopathy Suspected - Doctor Review Recommended                    * KINDLY CORRELATE CLINICALLY *";
                    else if (noDR)
                        prediction = "EXAMINATION RESULT: No Abnormlities detected                    * KINDLY CORRELATE CLINICALLY *";
                    else
                        prediction = "EXAMINATION RESULT: Bad Image                    * KINDLY CORRELATE CLINICALLY *";

                    foreach (var item in posteriorDatas)
                    {
                        item.ImageUrl = Path.Combine(path, item.Img);
                        item.FileName = Path.GetFileName(item.ImageUrl);
                        string result = item.Prediction + " | " + (item.Eye == "OS" ? "Right Eye" : "Left Eye");
                        if (posteriorCount != 0 && (posteriorCount % 2 == 0))
                            posteriorHTML += "<tr><td style='width:33.33%; vertical-align:top;padding-bottom:15px;'><table style='width:100%;height:200px;margin-bottom:10px;'><tr><td><img style='max-width:200px;max-height:200px;' src='" + item.FileName + "' /></td></tr></table><h3 style='font-size: 14px;color:#333;font-weight:400;margin:0;'>" + result + "</h3></td>";
                        else
                            posteriorHTML += "<td style='width:33.33%; vertical-align:top;padding-bottom:15px;'><table style='width:100%;height:200px;margin-bottom:10px;'><tr><td><img style='max-width:200px;max-height:200px;' src='" + item.FileName + "' /></td></tr></table><h3 style='font-size: 14px;color:#333;font-weight:400;margin:0;'>" + result + "</h3></td>" + (posteriorCount != 0 ? "</tr>" : "");
                        posteriorCount++;
                    }
                }

                foreach (var item in anteriorDatas)
                {
                    item.ImageUrl = Path.Combine(path, item.Img);
                    item.FileName = Path.GetFileName(item.ImageUrl);
                    string result = item.Prediction + " | " + item.Eye == "OS" ? "Right Eye" : "Left Eye";
                    if (posteriorCount != 0 && (posteriorCount % 2 == 0))
                        anteriorHTML += "<tr><td style='width:33.33%; vertical-align:top;padding-bottom:15px;'><table style='width:100%;height:200px;margin-bottom:10px;'><tr><td><img style='max-width:200px;max-height:200px;' src=" + item.FileName + " /></td></tr></table><h3 style='font-size: 14px;color:#333;font-weight:400;margin:0;'>" + result + "</h3></td>";
                    else
                        anteriorHTML += "<td style='width:33.33%; vertical-align:top;padding-bottom:15px;'><table style='width:100%;height:200px;margin-bottom:10px;'><tr><td><img style='max-width:200px;max-height:200px;' src=" + item.FileName + " /></td></tr></table><h3 style='font-size: 14px;color:#333;font-weight:400;margin:0;'>" + result + "</h3></td>" + (posteriorCount != 0 ? "</tr>" : "");
                    anteriorCount++;
                }
                text = string.Format(text, DateTime.Now.ToShortDateString(), PatientEntity.Nm, PatientEntity.DocNm, PatientEntity.HospitalNm, PatientEntity.Id.ToString(), PatientEntity.HospitalID, PatientEntity.HospitalScreening, PatientEntity.Mob, PatientEntity.Age, PatientEntity.Sex, PatientEntity.Hypertension, PatientEntity.Cataract, PatientEntity.LaserTreatment, PatientEntity.AllergyDrugs, PatientEntity.CurrentMedications, PatientEntity.Info, posteriorHTML, PatientEntity.OtherOption, PatientEntity.OthersID, anteriorHTML, predictionResult, PatientEntity.PatientId);
                System.IO.File.WriteAllText(fileHTML, text);


                HtmlToPdf converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertUrl(fileHTML);
                //doc.Margins = new PdfMargins(5);

                doc.Save(filePDF);
                doc.Close();
            };

            bw.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    ModernDialog.ShowMessage("Internal Server Error.", "Error Alert", MessageBoxButton.OK);
                }
            };
            bw.RunWorkerAsync();
        }

        private void OnPrintPDFCommand(object args)
        {
            //PrintDialog print = new PrintDialog();

            //PrintDocument doc = new PrintDocument();
            //doc.DocumentName = @"D:\Project\product\3nethra\bin\x86\Debug\Uploads\c36dd8cc-e531-4ffc-b201-effa06279e8a\19162fc1-fbc3-4452-b29c-216ef2ec8f65\report.pdf";
            //if (print.ShowDialog() == true)
            //{
            //    doc.Print();
            //}
        }

        private void OnBackCommand(object args)
        {
            PatientEntity.PreviousState = "ReportView";
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var predictionView = (win) as Artelus.MainWindow;
                    predictionView.ContentSource = new Uri("Views/ImagePredictionView.xaml", UriKind.Relative);
                    predictionView.DataContext = new PredictionViewModel(PatientEntity);
                }
            }
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

        private void OnFtpTransferCommand(object args)
        {
            this.IsProgressActive = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (sender, e) =>
            {
                APIResult result = new APIResult();
                if (PatientEntity.PatientId == 0)
                    result = SyncPatientDetails(PatientEntity, false);
                else
                    result = SyncPatientDetails(PatientEntity, true);

                if (result.status == "ok")
                {
                    foreach (var item in PatientReports)
                    {
                        item.Sync = true;
                        new Patient().UpdateSyncStatus(item.Id);
                    }
                }

                string localPath = Path.Combine(Program.BaseDir(), "Uploads", PatientEntity.UniqueID.ToString());
                //string json = new Patient().Get(PatientEntity.Id);
                //if (!Directory.Exists(localPath))
                //    Directory.CreateDirectory(localPath);
                //System.IO.File.WriteAllText(localPath + "/Patient.json", json);
                //string reportJson = new Patient().GetReportJson(PatientEntity.Id);
                //System.IO.File.WriteAllText(localPath + "/PatientReport.json", reportJson);
                //string reportDataJson = new Patient().GetReportDataJson(PatientEntity.Id);
                //System.IO.File.WriteAllText(localPath + "/ReportData.json", reportDataJson);

                // var keyFile = new PrivateKeyFile(@"D:\Project\3nethra\3nethra\machine.pem");
                //var keyFiles = new[] { keyFile };
                string ftpHost = ConfigurationManager.AppSettings["ftpHost"].ToString();
                string ftpUserName = ConfigurationManager.AppSettings["ftpUserName"].ToString();
                string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"].ToString();

                //var methods = new List<AuthenticationMethod>();
                //methods.Add(new PrivateKeyAuthenticationMethod("password", pwd));

                //var con = new ConnectionInfo("192.168.0.133", username,methods);

                //new SftpClient()
                using (var client = new SftpClient(ftpHost, ftpUserName, ftpPassword))
                {
                    client.Connect();
                    client.ChangeDirectory("/home/ftpuser2/FTPCollection");
                    string rootDir = client.WorkingDirectory + "/" + PatientEntity.UniqueID.ToString();

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

        private void OnTakeReportCommand(object args)
        {
            //this.CloseAction();
            //var camVM = new CameraViewModel(PatientEntity);
            //var window = new ModernWindow
            //{
            //    Style = (Style)App.Current.Resources["BlankWindow"],
            //    Title = "Camera",
            //    IsTitleVisible = true,
            //    WindowState = WindowState.Maximized
            //};
            //window.Content = new CameraView(camVM, window);
            //var closeResult = window.ShowDialog();
            //PatientReports = new ObservableCollection<PatientReport>();
            //var report = new Patient().GetAllReport(PatientEntity.Id);
            //foreach (var item in report)
            //    PatientReports.Add(item);


            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var cameraView = (win) as Artelus.MainWindow;
                    cameraView.ContentSource = new Uri("Views/CameraView.xaml", UriKind.Relative);
                    cameraView.DataContext = new CameraViewModel(PatientEntity);
                }
            }
        }

        private void OnViewReportDataCommand(object args)
        {
            PatientReport = args as PatientReport;
            if (PatientReport != null)
            {
                ReportDatas = new ObservableCollection<ReportData>();
                string path = Path.Combine(Program.BaseDir(), "Uploads");
                if (PatientReport.ReportDatas != null)
                {
                    foreach (var item in PatientReport.ReportDatas)
                    {
                        item.ImageUrl = Path.Combine(path, item.Img);
                        ReportDatas.Add(item);
                    }
                    ShowReport = true;
                }
            }
        }

        private void OnSendMailCommand(object args)
        {

            this.IsProgressActive = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (sender, e) =>
            {
                string body = "Hi " + PatientEntity.Nm + ",<br><br>" + "Please find your report attachment.";

                List<string> attachment = new List<string>();
                string path = Path.Combine(Program.BaseDir(), "Uploads", PatientEntity.UniqueID.ToString(), PatientReport.UniqueID.ToString());
                List<string> files = Directory.EnumerateFiles(path).ToList();
                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);
                    if (fileName != "report.html")
                        attachment.Add(file);
                }

                Mail.Send(PatientEntity.Email, "Artelus Report", body, true, attachment, Helper.ContactEmails());
            };

            bw.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                    ModernDialog.ShowMessage("Internal Server Error. Please contact your administrator", "Alert", MessageBoxButton.OK);
                else
                    ModernDialog.ShowMessage("E-Mail sent successfully!", "Report", MessageBoxButton.OK);
                this.IsProgressActive = false;
            };
            bw.RunWorkerAsync();
        }

        private void OnSaveNextCommand(object args)
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    //var patientView = (win) as Artelus.MainWindow;
                    //patientView.ContentSource = new Uri("Views/PatientView.xaml", UriKind.Relative);
                    //patientView.DataContext = new PatientViewModel();

                    var artelus = (win) as Artelus.MainWindow;
                    artelus.DataContext = null;
                    artelus.ContentSource = new Uri("Views/PatientView.xaml", UriKind.Relative);

                    //var dataContext = win.DataContext as MainWindowViewModel;
                    //artelus.DataContext = new PatientViewModel();
                    //dataContext.CurrentViewModel = new PatientViewModel();
                }
            }
        }

        private void OnSaveExitCommand(object args)
        {
            var result = ModernDialog.ShowMessage("Do you want to close the application?", "Are you sure?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private APIResult SyncPatientDetails(PatientEntity patient, bool isUpdate)
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
            try
            {
                result = RestCalls.SyncReport(url, json, isUpdate);
                if (result.status == "ok")
                {
                    if (isUpdate)
                    {
                        bool newReport = PatientReports.Any(x => x.Sync == false);
                        if (newReport)
                            result = SyncPatientReport(PatientReports, PatientEntity.PatientId);
                    }
                    else
                    {
                        PatientEntity.PatientId = result.user;
                        new Patient().UpdatePatientId(patient.Id, result.user);
                        SyncPatientReport(PatientReports, result.user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                ModernDialog.ShowMessage("An error has occurred on the server", "Alert", MessageBoxButton.OK);
            }
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
            try
            {
                result = RestCalls.SyncReport(url, json, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                ModernDialog.ShowMessage("An error has occurred on the server", "Alert", MessageBoxButton.OK);
            }
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
