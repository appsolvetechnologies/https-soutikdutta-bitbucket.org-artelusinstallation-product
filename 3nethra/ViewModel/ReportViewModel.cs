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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Artelus.ViewModel
{

    public class ReportViewModel : BaseViewModel
    {
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

        public Action CloseAction { get; set; }

        public ReportViewModel(PatientEntity model, PatientReport obj)
        {
            BackCommand = new DelegateCommand(OnBackCommand);

            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(rootPath, "Uploads");
            ReportDatas = new ObservableCollection<ReportData>();
            PatientEntity = model;
            if (PatientEntity.Sex == "m")
                PatientEntity.Sex = "Male";
            else
                PatientEntity.Sex = "Female";

            if (PatientEntity.MaritalStatus == "no")
                PatientEntity.MaritalStatus = "Single";
            else
                PatientEntity.MaritalStatus = "Married";

            if (PatientEntity.IfResidentOfM == "yes")
            {
                PatientEntity.OtherOption = "IC Number:";
                PatientEntity.OthersID = PatientEntity.IcNumber;
            }

            if (obj != null)
                PatientReport = obj;
            else
                PatientReport = new Patient().GetLastestReport(model.Id);

            if (PatientReport != null)
            {
                var osResult = new Patient().GetPosteriorOSReportData(PatientReport.Id, false);
                foreach (var data in osResult)
                {
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    OSReportDatas.Add(data);
                }
                var odResult = new Patient().GetPosteriorODReportData(PatientReport.Id, false);
                foreach (var data in odResult)
                {
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    ODReportDatas.Add(data);
                }
            }

            //var report = new Patient().GetAllReport(model.Id);
            //foreach (var item in report)
            //{
            //    var result = new Patient().GetAllReportData(item.Id);
            //    item.ReportDatas = new ObservableCollection<ReportData>();
            //    foreach (var data in result)
            //    {
            //        data.ImageUrl = Path.Combine(path, PatientEntity.UniqueID.ToString(), data.Img);
            //        item.ReportDatas.Add(data);
            //    }
            //    PatientReports.Add(item);
            //}

            TakeReportCommand = new DelegateCommand(OnTakeReportCommand);
            PreviousReportCommand = new DelegateCommand(OnPreviousReportCommand);
            FtpTransferCommand = new DelegateCommand(OnFtpTransferCommand);
            ViewReportDataCommand = new DelegateCommand(OnViewReportDataCommand);
            SaveNextCommand = new DelegateCommand(OnSaveNextCommand);
            SaveExitCommand = new DelegateCommand(OnSaveExitCommand);
            SendMailCommand = new DelegateCommand(OnSendMailCommand);
            ViewPDFCommand = new DelegateCommand(OnViewPDFCommand);
        }

        private void OnViewPDFCommand(object args)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(rootPath, "Uploads");
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
            string predictionResult = string.Empty;
            int posteriorCount = 0, anteriorCount = 0;
            List<ReportData> posteriorDatas = new Patient().GetPosteriorReportData(PatientReport.Id);
            List<ReportData> anteriorDatas = new Patient().GetAnteriorReportData(PatientReport.Id);

            if (posteriorDatas != null)
            {
                bool res = posteriorDatas.Select(x => x.Prediction == "Doctor review recommended" && x.Prediction != "Bad Image").Any();
                if (res)
                    predictionResult = "EXAMINATION RESULT: Diabetic Retinopathy Suspected - Doctor Review Recommended                    * KINDLY CORRELATE CLINICALLY *";
                else
                    predictionResult = "EXAMINATION RESULT: No Abnormlities detected                    * KINDLY CORRELATE CLINICALLY *";

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
            text = string.Format(text, DateTime.Now.ToShortDateString(), PatientEntity.Nm, PatientEntity.DocNm, PatientEntity.HospitalNm, PatientEntity.Id.ToString(), PatientEntity.HospitalID, PatientEntity.HospitalScreening, PatientEntity.Mob, PatientEntity.Age, PatientEntity.Sex, PatientEntity.Hypertension, PatientEntity.Cataract, PatientEntity.LaserTreatment, PatientEntity.AllergyDrugs, PatientEntity.CurrentMedications, PatientEntity.Info, posteriorHTML, PatientEntity.OtherOption, PatientEntity.OthersID, anteriorHTML, predictionResult);
            System.IO.File.WriteAllText(fileHTML, text);


            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertUrl(fileHTML);
            doc.Save(filePDF);
            doc.Close();
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

            //var reporthVM = new ReportHistoryViewModel(PatientEntity);
            //var window = new ModernWindow
            //{
            //    Style = (Style)App.Current.Resources["BlankWindow"],
            //    Title = "Patient Report History",
            //    IsTitleVisible = true,
            //    WindowState = WindowState.Maximized
            //};
            //window.Content = new ReportHistoryView(reporthVM, window);
            //var closeResult = window.ShowDialog();
            //PatientReports = new ObservableCollection<PatientReport>();
            //var report = new Patient().GetAllReport(PatientEntity.Id);
            //foreach (var item in report)
            //    PatientReports.Add(item);
        }

        private void OnFtpTransferCommand(object args)
        {
            try
            {
                string rootPath = AppDomain.CurrentDomain.BaseDirectory;
                string localPath = Path.Combine(rootPath, "Uploads", PatientEntity.Id.ToString());
                string json = new Patient().Get(PatientEntity.Id);
                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);
                System.IO.File.WriteAllText(localPath + "/Patient.json", json);
                string reportJson = new Patient().GetReportJson(PatientEntity.Id);
                System.IO.File.WriteAllText(localPath + "/PatientReport.json", reportJson);
                string reportDataJson = new Patient().GetReportDataJson();
                System.IO.File.WriteAllText(localPath + "/ReportData.json", reportDataJson);

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
                    string rootDir = client.WorkingDirectory + "/" + PatientEntity.Id.ToString();

                    if (!client.Exists(rootDir))
                        client.CreateDirectory(rootDir);
                    foreach (var item in PatientReports)
                    {
                        string rootDataDir = rootDir + "/" + item.Id.ToString();
                        if (!client.Exists(rootDataDir))
                        {
                            client.CreateDirectory(rootDataDir);
                            client.ChangeDirectory(rootDataDir);
                            UploadDirectory(client, localPath, rootDir);
                        }
                    }
                    client.Disconnect();
                }
                //ModernDialog.ShowMessage("File transfer completed successfully", "Messagebox", MessageBoxButton.OK);

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    {
                        client.CreateDirectory(subPath);
                    }
                    UploadDirectory(client, info.FullName, remotePath + "/" + info.Name);
                }
                else
                {
                    using (Stream fileStream = new FileStream(info.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        Console.WriteLine(
                            "Uploading {0} ({1:N0} bytes)", info.FullName, ((FileInfo)info).Length);
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
                string rootPath = AppDomain.CurrentDomain.BaseDirectory;
                string path = Path.Combine(rootPath, "Uploads");
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

            string body = "Hi " + PatientEntity.Nm + ",<br><br>" + "Please find your report attachment.";

            List<string> attachment = new List<string>();
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(rootPath, "Uploads", PatientEntity.UniqueID.ToString(), PatientReport.UniqueID.ToString());
            List<string> files = Directory.EnumerateFiles(path).ToList();
            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);
                if (fileName != "report.html")
                    attachment.Add(file);
            }

            Mail.Send(PatientEntity.Email, "Artelus Report", body, true, attachment, Helper.ContactEmails());
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
                    artelus.ContentSource = new Uri("Views/PatientView.xaml", UriKind.Relative);
                    //var dataContext = win.DataContext as MainWindowViewModel;
                    artelus.DataContext = new PatientViewModel();
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
    }
}
