using Artelus.Model;
using Artelus.Views;
using FirstFloor.ModernUI.Windows.Controls;
using Helpers;
using Renci.SshNet;
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

        public DelegateCommand CompleteCommand { get; set; }
        public Action CloseAction { get; set; }
        public ReportViewModel(PatientEntity model)
        {
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
            PatientReport = new Patient().GetLastestReport(model.Id);
            if (PatientReport != null)
            {
                var osResult = new Patient().GetOSReportData(PatientReport.Id,false);
                foreach (var data in osResult)
                {
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    OSReportDatas.Add(data);
                }
                var odResult = new Patient().GetODReportData(PatientReport.Id,false);
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
                var username = "ftpuser2";
                var pwd = "ftp";

                //var methods = new List<AuthenticationMethod>();
                //methods.Add(new PrivateKeyAuthenticationMethod("password", pwd));

                //var con = new ConnectionInfo("192.168.0.133", username,methods);

                //new SftpClient()
                using (var client = new SftpClient("192.168.0.133", username, pwd))
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
    }
}
