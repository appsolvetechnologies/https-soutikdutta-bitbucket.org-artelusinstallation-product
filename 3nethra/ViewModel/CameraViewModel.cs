using Artelus.Common;
using Artelus.Model;
using Artelus.Views;
using FirstFloor.ModernUI.Windows.Controls;
using ForusImaging;
using Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Artelus.ViewModel
{
    public class CameraViewModel : BaseViewModel
    {


        //OD-Right
        //OS-Left
        CameraAPI camera;
        private bool flag = true;
        public bool Flag
        {
            get { return flag; }
            set
            {
                flag = value;
                RaisePropertyChange("Flag");
            }
        }
        private bool manualCapture = false;
        private string camMode;
        public string CamMode
        {
            get { return camMode; }
            set
            {
                ShowPosterior = value == CameraAPI.OperatingMode.ANTERIOR_MODE.ToString();
                camMode = value;
                RaisePropertyChange("CamMode");
            }
        }
        public bool ManualCapture
        {
            get { return manualCapture; }
            set
            {
                manualCapture = value;
                RaisePropertyChange("ManualCapture");
            }
        }
        private bool showPosterior = true;
        public bool ShowPosterior
        {
            get { return showPosterior; }
            set
            {
                showPosterior = value;
                RaisePropertyChange("ShowPosterior");
            }
        }
        private CameraEntity cameraEntity;
        public CameraEntity CameraEntity
        {
            get { return cameraEntity; }
            set { cameraEntity = value; }
        }
        private PatientEntity patient;
        public PatientEntity PatientEntity
        {
            get { return patient; }
            set { patient = value; }
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
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CamStartCommand { get; set; }
        public DelegateCommand CamStopCommand { get; set; }
        public DelegateCommand CamPosteriorCommand { get; set; }
        public DelegateCommand ReportViewCommand { get; set; }
        public DelegateCommand CamAnteriorCommand { get; set; }
        public DelegateCommand CaptureCommand { get; set; }
        public DelegateCommand DeleteReportDataCommand { get; set; }
        public DelegateCommand BackCommand { get; set; }
        public Action CloseAction { get; set; }
        private ObservableCollection<ReportData> anteriorReportData = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> AnteriorReportData
        {
            get { return anteriorReportData; }
            set
            {
                if (value != anteriorReportData)
                {
                    anteriorReportData = value;
                    RaisePropertyChange("AnteriorReportData");
                }
            }
        }
        private ObservableCollection<ReportData> posteriorReportData = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> PosteriorReportData
        {
            get { return posteriorReportData; }
            set
            {
                if (value != posteriorReportData)
                {
                    posteriorReportData = value;
                    RaisePropertyChange("PosteriorReportData");
                }
            }
        }

        private ObservableCollection<ReportData> _OSAnteriorReportDatas = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> OSAnteriorReportDatas
        {
            get { return _OSAnteriorReportDatas; }
            set
            {
                if (value != _OSAnteriorReportDatas)
                {
                    _OSAnteriorReportDatas = value;
                    RaisePropertyChange("OSAnteriorReportDatas");
                }
            }
        }
        private ObservableCollection<ReportData> _ODAnteriorReportDatas = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> ODAnteriorReportDatas
        {
            get { return _ODAnteriorReportDatas; }
            set
            {
                if (value != _ODAnteriorReportDatas)
                {
                    _ODAnteriorReportDatas = value;
                    RaisePropertyChange("ODAnteriorReportDatas");
                }
            }
        }

        private ObservableCollection<ReportData> _OSPosteriorReportDatas = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> OSPosteriorReportDatas
        {
            get { return _OSPosteriorReportDatas; }
            set
            {
                if (value != _OSPosteriorReportDatas)
                {
                    _OSPosteriorReportDatas = value;
                    RaisePropertyChange("OSPosteriorReportDatas");
                }
            }
        }
        private ObservableCollection<ReportData> _ODPosteriorReportDatas = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> ODPosteriorReportDatas
        {
            get { return _ODPosteriorReportDatas; }
            set
            {
                if (value != _ODPosteriorReportDatas)
                {
                    _ODPosteriorReportDatas = value;
                    RaisePropertyChange("ODPosteriorReportDatas");
                }
            }
        }

        public CameraViewModel(PatientEntity patient, PatientReport report = null)
        {
            PatientEntity = patient;
            Initialize();
            if (report != null)
                PatientReport = report;
        }
        public ICommand LogOffCommand { get; set; }

        private void OnLogOffCommand(object args)
        {
            Helper.LogOff();
        }
        private void Initialize()
        {
            LogOffCommand = new DelegateCommand(OnLogOffCommand);
            CameraEntity = new CameraEntity();
            CameraEntity.Eye = "OD";
            SaveCommand = new DelegateCommand(OnSaveCommand);
            CamStartCommand = new DelegateCommand(OnCamStartCommand);
            CamStopCommand = new DelegateCommand(OnCamStopCommand);
            CamPosteriorCommand = new DelegateCommand(OnCamPosteriorCommand);
            CamAnteriorCommand = new DelegateCommand(OnCamAnteriorCommand);
            CaptureCommand = new DelegateCommand(OnCaptureCommand);
            ReportViewCommand = new DelegateCommand(OnReportViewCommand);
            BackCommand = new DelegateCommand(OnBackCommand);
            PatientReport = new PatientReport();
            camera = CameraAPI.GetInstance();
            camera.Connect();
            camera.NewFrameEvent += new NewFrameHandler(camera_NewFrameEvent);
            camera.CapturedFrameEvent += new CapturedFrameHandler(camera_capturedFrameEvent);
            camera.setMode(CameraAPI.OperatingMode.POSTERIOR_MODE);
            CamMode = CameraAPI.OperatingMode.POSTERIOR_MODE.ToString();
        }

        private void OnReportViewCommand(object args)
        {
            var patientVM = new ReportViewModel(PatientEntity, null);
            var window = new ModernWindow
            {
                Style = (Style)App.Current.Resources["BlankWindow"],
                Title = "Camera",
                IsTitleVisible = true,
                WindowState = WindowState.Maximized
            };
            window.Content = new ReportView(patientVM, window);
            var closeResult = window.ShowDialog();
        }

        private void OnBackCommand(object args)
        {
            var result = MessageBoxResult.Yes;
            if (OSPosteriorReportDatas.Count > 0 || ODPosteriorReportDatas.Count > 0 || OSAnteriorReportDatas.Count > 0 || ODAnteriorReportDatas.Count > 0)
                result = ModernDialog.ShowMessage("You will loose the capture image.", "Are you sure?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                camera.StopLive();
                camera.Disconnect();
                PatientEntity.PreviousState = "CameraView";
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
        }

        private void OnCaptureCommand(object args)
        {

            //CamMode = CameraAPI.OperatingMode.ANTERIOR_MODE.ToString();
            //camera.setMode(CameraAPI.OperatingMode.ANTERIOR_MODE);
            manualCapture = true;
            camera.StopLive();
            camera.Capture();
            flag = false;
            //ManualCapture = true;
            //SaveReportData();
        }

        private void OnSaveCommand(object args)
        {
            camera.StopLive();
            camera.Disconnect();


            if (PatientEntity != null)
            {
                if (PatientReport.Id == 0)
                {
                    UserEntity user = new User().Get(Program.UserId());
                    PatientReport.PatientId = PatientEntity.Id;
                    PatientReport.Dt = DateTime.UtcNow;
                    PatientReport.Location = user.Location;
                    PatientReport.InstallID = user.InstallID;
                    PatientReport.UniqueID = Guid.NewGuid();
                    PatientReport.Id = new Patient().AddReport(PatientReport);
                }

                string rootPath = AppDomain.CurrentDomain.BaseDirectory;
                string path = Path.Combine(rootPath, "Uploads", PatientEntity.UniqueID.ToString(), PatientReport.UniqueID.ToString());
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (PatientReport.OSPosteriorReportDatas != null)
                    foreach (var item in PatientReport.OSPosteriorReportDatas)
                    {
                        if (item.Id == 0)
                        {
                            string fileName = item.Eye + "_" + DateTime.UtcNow.Ticks.ToString() + ".png";
                            string filePath = Path.Combine(path, fileName);
                            Save(item.BitMapImg, filePath);
                            new Patient().AddReportData(PatientReport.Id, "POSTERIOR_MODE", item.Eye, Path.Combine(PatientEntity.UniqueID.ToString(), PatientReport.UniqueID.ToString(), fileName), null, Program.FileSize(filePath));
                        }
                    }
                if (PatientReport.ODPosteriorReportDatas != null)
                    foreach (var item in PatientReport.ODPosteriorReportDatas)
                    {
                        if (item.Id == 0)
                        {
                            string fileName = item.Eye + "_" + DateTime.UtcNow.Ticks.ToString() + ".png";
                            string filePath = Path.Combine(path, fileName);
                            Save(item.BitMapImg, filePath);
                            new Patient().AddReportData(PatientReport.Id, "POSTERIOR_MODE", item.Eye, Path.Combine(PatientEntity.UniqueID.ToString(), PatientReport.UniqueID.ToString(), fileName), null, Program.FileSize(filePath));
                        }

                    }
                if (PatientReport.OSAnteriorReportDatas != null)
                    foreach (var item in PatientReport.OSAnteriorReportDatas)
                    {
                        if (item.Id == 0)
                        {
                            string fileName = item.Eye + "_" + DateTime.UtcNow.Ticks.ToString() + ".png";
                            string filePath = Path.Combine(path, fileName);
                            Save(item.BitMapImg, filePath);
                            new Patient().AddReportData(PatientReport.Id, "ANTERIOR_MODE", item.Eye, Path.Combine(PatientEntity.UniqueID.ToString(), PatientReport.UniqueID.ToString(), fileName), null, Program.FileSize(filePath));
                        }
                    }
                if (PatientReport.ODAnteriorReportDatas != null)
                    foreach (var item in PatientReport.ODAnteriorReportDatas)
                    {
                        if (item.Id == 0)
                        {
                            string fileName = item.Eye + "_" + DateTime.UtcNow.Ticks.ToString() + ".png";
                            string filePath = Path.Combine(path, fileName);
                            Save(item.BitMapImg, filePath);
                            new Patient().AddReportData(PatientReport.Id, "ANTERIOR_MODE", item.Eye, Path.Combine(PatientEntity.UniqueID.ToString(), PatientReport.UniqueID.ToString(), fileName), null, Program.FileSize(filePath));
                        }
                    }

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
        }

        void Save(BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        void camera_NewFrameEvent(Bitmap frame)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (flag)
                    using (MemoryStream memory = new MemoryStream())
                    {
                        frame.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                        memory.Position = 0;
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.StreamSource = memory;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.EndInit();
                        CameraEntity.LiveStream = img;
                    }
            });
        }

        void camera_capturedFrameEvent(Bitmap frame)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (flag)
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        frame.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                        memory.Position = 0;
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.StreamSource = memory;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.EndInit();
                        CameraEntity.LiveStream = img;
                        cameraEntity.CaptureStream = img;

                        if (CamMode == CameraAPI.OperatingMode.POSTERIOR_MODE.ToString())
                        {
                            if (CameraEntity.Eye == "OS")
                            {
                                if (PatientReport.OSPosteriorReportDatas == null)
                                    PatientReport.OSPosteriorReportDatas = new ObservableCollection<ReportData>();
                                PatientReport.OSPosteriorReportDatas.Add(new ReportData() { Eye = "OS", BitMapImg = img });
                            }
                            else
                            {
                                if (PatientReport.ODPosteriorReportDatas == null)
                                    PatientReport.ODPosteriorReportDatas = new ObservableCollection<ReportData>();
                                PatientReport.ODPosteriorReportDatas.Add(new ReportData() { Eye = "OD", BitMapImg = img });
                            }
                        }
                        else if (CamMode == CameraAPI.OperatingMode.ANTERIOR_MODE.ToString())
                        {
                            if (CameraEntity.Eye == "OS")
                            {
                                if (PatientReport.OSAnteriorReportDatas == null)
                                    PatientReport.OSAnteriorReportDatas = new ObservableCollection<ReportData>();
                                PatientReport.OSAnteriorReportDatas.Add(new ReportData() { Eye = "OS", BitMapImg = img });
                            }
                            else
                            {
                                if (PatientReport.ODAnteriorReportDatas == null)
                                    PatientReport.ODAnteriorReportDatas = new ObservableCollection<ReportData>();
                                PatientReport.ODAnteriorReportDatas.Add(new ReportData() { Eye = "OD", BitMapImg = img });
                            }
                        }
                    }
                    //SaveReportData();
                }
            });
        }

        private void OnCamStartCommand(object args)
        {
            flag = true;
            ManualCapture = false;
            //camMode = string.Empty;
            camera.StartLive();
        }

        private void OnCamStopCommand(object args)
        {
            camera.StopLive();
        }

        private void OnCamPosteriorCommand(object args)
        {
            CamMode = CameraAPI.OperatingMode.POSTERIOR_MODE.ToString();
            camera.StopLive();
            camera.Capture();
            flag = false;
        }

        private void OnCamAnteriorCommand(object args)
        {
            CamMode = CameraAPI.OperatingMode.ANTERIOR_MODE.ToString();
            camera.setMode(CameraAPI.OperatingMode.ANTERIOR_MODE);
            camera.StopLive();
            camera.Capture();
            flag = false;
        }

        private void Clear()
        {
            camera.StopLive();
            camera.Disconnect();
        }
    }
}
