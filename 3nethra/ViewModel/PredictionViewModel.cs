using Artelus.Common;
using Artelus.Model;
using FirstFloor.ModernUI.Windows.Controls;
using Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Input;

namespace Artelus.ViewModel
{
    public class PredictionViewModel : BaseViewModel
    {
        private int progressValue;
        public int ProgressValue
        {
            get { return progressValue; }
            set
            {
                progressValue = value;
                RaisePropertyChange("ProgressValue");
            }
        }
        private bool isEnabled = true;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                RaisePropertyChange("IsEnabled");
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
        private bool selectAll;
        public bool SelectAll
        {
            get { return selectAll; }
            set
            {
                selectAll = value;
                RaisePropertyChange("SelectAll");
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
        private string hansanet = "Enable Hansanet";
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
        public DelegateCommand StartPredictionCommand { get; set; }
        public DelegateCommand BackCommand { get; set; }
        public ICommand LogOffCommand { get; set; }
        public DelegateCommand ShowImageCommand { get; set; }

        private void OnLogOffCommand(object args)
        {
            Helper.LogOff();
        }
        public PredictionViewModel(PatientEntity model)
        {
            ShowImageCommand = new DelegateCommand(OnShowImageCommand);
            LogOffCommand = new DelegateCommand(OnLogOffCommand);
            SetHansanetCommand = new DelegateCommand(OnSetHansanetCommand);
            SelectAllCommand = new DelegateCommand(OnSelectAllComand);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            StartPredictionCommand = new DelegateCommand(OnStartPredictionCommand);
            BackCommand = new DelegateCommand(OnBackCommand);
            PatientEntity = model;
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(rootPath, "Uploads");
            PatientReport = new PatientReport();
            PatientReport = new Patient().GetLastestReport(model.Id);
            if (PatientReport != null)
            {
                PatientReport.OSPosteriorReportDatas = new ObservableCollection<ReportData>();
                PatientReport.ODPosteriorReportDatas = new ObservableCollection<ReportData>();

                var osResult = new Patient().GetPosteriorOSReportData(PatientReport.Id, true);
                foreach (var osData in osResult)
                {
                    osData.ImageUrl = Path.Combine(path, osData.Img);
                    osData.BitMapImg = new System.Windows.Media.Imaging.BitmapImage(new Uri(osData.ImageUrl));
                    osData.FileName = Path.GetFileName(osData.ImageUrl);
                    PatientReport.OSPosteriorReportDatas.Add(osData);
                }
                var odResult = new Patient().GetPosteriorODReportData(PatientReport.Id, true);
                foreach (var odData in odResult)
                {
                    odData.ImageUrl = Path.Combine(path, odData.Img);
                    odData.BitMapImg = new System.Windows.Media.Imaging.BitmapImage(new Uri(odData.ImageUrl));
                    odData.FileName = Path.GetFileName(odData.ImageUrl);
                    PatientReport.ODPosteriorReportDatas.Add(odData);
                }

                PatientReport.OSAnteriorReportDatas = new ObservableCollection<ReportData>();
                PatientReport.ODAnteriorReportDatas = new ObservableCollection<ReportData>();

                var osAnResult = new Patient().GetOSAnteriorReportData(PatientReport.Id, true);
                foreach (var osData in osAnResult)
                {
                    osData.ImageUrl = Path.Combine(path, osData.Img);
                    osData.BitMapImg = new System.Windows.Media.Imaging.BitmapImage(new Uri(osData.ImageUrl));
                    osData.FileName = Path.GetFileName(osData.ImageUrl);
                    PatientReport.OSAnteriorReportDatas.Add(osData);
                }
                var odAnResult = new Patient().GetODAnteriorReportData(PatientReport.Id, true);
                foreach (var odData in odAnResult)
                {
                    odData.ImageUrl = Path.Combine(path, odData.Img);
                    odData.BitMapImg = new System.Windows.Media.Imaging.BitmapImage(new Uri(odData.ImageUrl));
                    odData.FileName = Path.GetFileName(odData.ImageUrl);
                    PatientReport.ODAnteriorReportDatas.Add(odData);
                }
            }
        }

        private void OnShowImageCommand(object args)
        {
            string imagePath = args as string;
            Process.Start(imagePath);
        }

        private void OnBackCommand(object args)
        {
            PatientEntity.PreviousState = "PredictionView";
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var cameraView = (win) as Artelus.MainWindow;
                    cameraView.ContentSource = new Uri("Views/CameraView.xaml", UriKind.Relative);
                    cameraView.DataContext = new CameraViewModel(PatientEntity, PatientReport);
                }
            }
        }


        private void OnSetHansanetCommand(object args)
        {
            if (this.hansanet == "Enable Hansanet")
                this.Hansanet = "Disable Hansanet";
            else
                this.Hansanet = "Enable Hansanet";
        }
        private void OnSelectAllComand(object args)
        {
            foreach (var item in PatientReport.OSPosteriorReportDatas)
                item.IsChecked = this.SelectAll;

            foreach (var item in PatientReport.ODPosteriorReportDatas)
                item.IsChecked = this.SelectAll;
        }
        private void OnSaveCommand(object args)
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var cameraView = (win) as Artelus.MainWindow;
                    cameraView.ContentSource = new Uri("Views/ReportView.xaml", UriKind.Relative);
                    cameraView.DataContext = new ReportViewModel(PatientEntity, null);
                }
            }

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ProgressValue++;
        }
        private void OnStartPredictionCommand(object args)
        {
            string prediction = "sushruta";
            if (this.hansanet != "Enable Hansanet")
                prediction = "hansasushruta";

            //System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //dispatcherTimer.Tick += dispatcherTimer_Tick;
            //dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000 );
            //ProgressValue = 0;
            //dispatcherTimer.Start();

            //System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            //{

            this.IsEnabled = false;
            this.IsProgressActive = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (sender, e) =>
            {
                foreach (var item in PatientReport.OSPosteriorReportDatas)
                {
                    if (item.IsChecked)
                    {
                        //try
                        //{
                        string predictionResult = RestCalls.RestPredict(item.ImageUrl, prediction);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        PredictionEntity obj = JsonConvert.DeserializeObject<PredictionEntity>(predictionResult);

                        //PredictionEntity obj = new PredictionEntity();
                        //  obj.result = "Bad Image";
                        if (obj.result.StartsWith("Bad"))
                        {
                            item.Prediction = obj.result.Replace("(0)", "").Trim();
                            new Patient().UpdateReportData(item.Id, item.Prediction);
                        }
                        else
                        {
                            item.Prediction = obj.result.Replace(" (1). ", "").Replace(" (0). ", "").Trim();
                            new Patient().UpdateReportData(item.Id, item.Prediction);
                        }
                        //}
                        //catch
                        //{
                        //    ModernDialog.ShowMessage("Prediction Internal Server Error.", "Prediction", MessageBoxButton.OK);
                        //}

                    }
                }
                foreach (var item in PatientReport.ODPosteriorReportDatas)
                {
                    if (item.IsChecked)
                    {
                        //try
                        //{
                        string predictionResult = RestCalls.RestPredict(item.ImageUrl, prediction);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        PredictionEntity obj = JsonConvert.DeserializeObject<PredictionEntity>(predictionResult);

                        //PredictionEntity obj = new PredictionEntity();
                        //  obj.result = "Bad Image";
                        if (obj.result.StartsWith("Bad"))
                        {
                            item.Prediction = obj.result.Replace("(0)", "").Trim();
                            new Patient().UpdateReportData(item.Id, item.Prediction);
                        }
                        else
                        {
                            item.Prediction = obj.result.Replace(" (1). ", "").Replace(" (0). ", "").Trim();
                            new Patient().UpdateReportData(item.Id, item.Prediction);
                        }
                        //}
                        //catch
                        //{
                        //    ModernDialog.ShowMessage("Prediction Internal Server Error.", "Prediction", MessageBoxButton.OK);
                        //}

                    }
                }

            };

            bw.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    ModernDialog.ShowMessage("Prediction Internal Server Error.", "Prediction", MessageBoxButton.OK);
                }
                this.IsEnabled = true;
                this.IsProgressActive = false;
            };

            bw.RunWorkerAsync();



            //this.IsEnabled = true;
            //dispatcherTimer.Stop();
            //ProgressValue = 0;
            //});
        }
    }
}
