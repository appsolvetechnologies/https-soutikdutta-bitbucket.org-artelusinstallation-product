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
using System.Windows.Media.Imaging;

namespace Artelus.ViewModel
{
    public class CameraViewModel : BaseViewModel
    {


        //OD-Right
        //OS-Left
        CameraAPI camera;
        private int patientReportId;
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
        private string camMode;
        public string CamMode
        {
            get { return camMode; }
            set { camMode = value; }
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
        public int PatientReportId
        {
            get { return patientReportId; }
            set
            {
                patientReportId = value;
                RaisePropertyChange("PatientReportId");
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
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CamStartCommand { get; set; }
        public DelegateCommand CamStopCommand { get; set; }
        public DelegateCommand CamPosteriorCommand { get; set; }
        public DelegateCommand ReportViewCommand { get; set; }
        public DelegateCommand CamAnteriorCommand { get; set; }
        public DelegateCommand CaptureCommand { get; set; }
        public DelegateCommand SetHansanetCommand { get; set; }
        public DelegateCommand DeleteReportDataCommand { get; set; }
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
        private ObservableCollection<ReportData> porteriorReportData = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> PorteriorReportData
        {
            get { return porteriorReportData; }
            set
            {
                if (value != porteriorReportData)
                {
                    porteriorReportData = value;
                    RaisePropertyChange("PorteriorReportData");
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


        private ObservableCollection<ReportData> _OSPorteriorReportDatas = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> OSPorteriorReportDatas
        {
            get { return _OSPorteriorReportDatas; }
            set
            {
                if (value != _OSPorteriorReportDatas)
                {
                    _OSPorteriorReportDatas = value;
                    RaisePropertyChange("OSPorteriorReportDatas");
                }
            }
        }
        private ObservableCollection<ReportData> _ODPorteriorReportDatas = new ObservableCollection<ReportData>();
        public ObservableCollection<ReportData> ODPorteriorReportDatas
        {
            get { return _ODPorteriorReportDatas; }
            set
            {
                if (value != _ODPorteriorReportDatas)
                {
                    _ODPorteriorReportDatas = value;
                    RaisePropertyChange("ODPorteriorReportDatas");
                }
            }
        }


        //public CameraViewModel()
        //{
        //    //Initialize();
        //}

        public CameraViewModel(PatientEntity patient)
        {
            PatientEntity = patient;
            Initialize();
        }

        private void Initialize()
        {
            CameraEntity = new CameraEntity();
            CameraEntity.Eye = "OS";
            SaveCommand = new DelegateCommand(OnSaveCommand);
            CamStartCommand = new DelegateCommand(OnCamStartCommand);
            CamStopCommand = new DelegateCommand(OnCamStopCommand);
            CamPosteriorCommand = new DelegateCommand(OnCamPosteriorCommand);
            CamAnteriorCommand = new DelegateCommand(OnCamAnteriorCommand);
            CaptureCommand = new DelegateCommand(OnCaptureCommand);
            SetHansanetCommand = new DelegateCommand(OnSetHansanetCommand);
            ReportViewCommand = new DelegateCommand(OnReportViewCommand);

            camera = CameraAPI.GetInstance();
            camera.Connect();
            camera.NewFrameEvent += new NewFrameHandler(camera_NewFrameEvent);
            camera.CapturedFrameEvent += new CapturedFrameHandler(camera_capturedFrameEvent);
            camera.setMode(CameraAPI.OperatingMode.POSTERIOR_MODE);
            CamMode = CameraAPI.OperatingMode.POSTERIOR_MODE.ToString();
        }

        private void OnReportViewCommand(object args)
        {
            var patientVM = new ReportViewModel(PatientEntity);
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

        private void OnSetHansanetCommand(object args)
        {
            if (this.hansanet == "Disable Hansanet")
                this.Hansanet = "Enable Hansanet";
            else
                this.Hansanet = "Disable Hansanet";
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

        void SaveReportData()
        {
            if (PatientEntity != null)
            {
                if (PatientReportId == 0)
                    PatientReportId = new Patient().AddReport(PatientEntity.Id, "HSR Layout");


                string rootPath = AppDomain.CurrentDomain.BaseDirectory;
                string path = Path.Combine(rootPath, "Uploads", PatientEntity.UniqueID.ToString(), PatientReportId.ToString());
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileName = CameraEntity.Eye + "_" + DateTime.Now.Ticks.ToString() + ".png";
                string filePath = Path.Combine(path, fileName);
                Save(CameraEntity.CaptureStream, filePath);

                int reportDataId = new Patient().AddReportData(PatientReportId, "POSTERIOR_MODE", CameraEntity.Eye, Path.Combine(PatientEntity.UniqueID.ToString(), PatientReportId.ToString(), fileName), null, null);

                string size = Program.FileSize(filePath);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    string prediction = "sushruta";
                    if (this.hansanet != "Disable Hansanet")
                        prediction = "hansasushruta";
                    string predictionResult = RestCalls.RestPredict(filePath, prediction);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    PredictionEntity obj = JsonConvert.DeserializeObject<PredictionEntity>(predictionResult);

                    //PredictionEntity obj = new PredictionEntity();
                    //  obj.result = "Bad Image";
                    if (obj.result.StartsWith("Bad"))
                    {
                        string result = obj.result.Replace("(0)", "").Trim();
                        PorteriorReportData.Add(new Model.ReportData() { Id = reportDataId, Eye = CameraEntity.Eye, ImageUrl = filePath, Mode = CameraAPI.OperatingMode.POSTERIOR_MODE.ToString(), Size = size, Status = false, Prediction = result });
                        new Patient().UpdateReportData(reportDataId, result, size);
                    }
                    else
                    {
                        string result = obj.result.Replace(" (1). ", "").Replace(" (0). ", "").Trim();
                        PorteriorReportData.Add(new Model.ReportData() { Id = reportDataId, Eye = CameraEntity.Eye, ImageUrl = filePath, Mode = CameraAPI.OperatingMode.POSTERIOR_MODE.ToString(), Size = size, Status = true, Prediction = result });
                        new Patient().UpdateReportData(reportDataId, result, size);
                    }
                });
                flag = false;
            }
        }

        private void OnSaveCommand(object args)
        {

            camera.StopLive();
            camera.Disconnect();
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var cameraView = (win) as Artelus.MainWindow;
                    cameraView.ContentSource = new Uri("Views/ReportView.xaml", UriKind.Relative);
                    cameraView.DataContext = new ReportViewModel(PatientEntity);
                }
            }
            //Clear();
            //this.CloseAction();
            //var patientVM = new ReportViewModel(PatientEntity);
            //var window = new ModernWindow
            //{
            //    Style = (Style)App.Current.Resources["BlankWindow"],
            //    Title = "Patient Report",
            //    IsTitleVisible = true,
            //    WindowState = WindowState.Maximized
            //};
            //window.Content = new ReportView(patientVM, window);
            //var closeResult = window.ShowDialog();


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
                    SaveReportData();
                }

            }
        });
        }

        private void OnCamStartCommand(object args)
        {
            flag = true;
            ManualCapture = false;
            camMode = string.Empty;
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
