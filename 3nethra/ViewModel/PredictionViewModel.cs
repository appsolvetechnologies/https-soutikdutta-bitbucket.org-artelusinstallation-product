using Artelus.Model;
using Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Artelus.ViewModel
{
    public class PredictionViewModel : BaseViewModel
    {
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
        public DelegateCommand StartPredictionCommand { get; set; }

        public PredictionViewModel(PatientEntity model)
        {
            SetHansanetCommand = new DelegateCommand(OnSetHansanetCommand);
            SelectAllCommand = new DelegateCommand(OnSelectAllComand);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            StartPredictionCommand = new DelegateCommand(OnStartPredictionCommand);
            PatientEntity = model;
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(rootPath, "Uploads");
            PatientReport = new PatientReport();
            PatientReport = new Patient().GetLastestReport(model.Id);
            if (PatientReport != null)
            {
                PatientReport.OSPosteriorReportDatas = new ObservableCollection<ReportData>();
                PatientReport.ODPosteriorReportDatas = new ObservableCollection<ReportData>();

                var osResult = new Patient().GetOSReportData(PatientReport.Id, true);
                foreach (var data in osResult)
                {
                    data.ImageUrl = Path.Combine(path, data.Img);
                    data.FileName = Path.GetFileName(data.ImageUrl);
                    PatientReport.OSPosteriorReportDatas.Add(data);
                }
                var odResult = new Patient().GetODReportData(PatientReport.Id, true);
                foreach (var data in odResult)
                {
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
                item.IsChecked = this.SelectAll;

            foreach (var item in PatientReport.ODPosteriorReportDatas)
                item.IsChecked = this.SelectAll;
        }
        private void OnSaveCommand(object args)
        {

        }

        private void OnStartPredictionCommand(object args)
        {
            string prediction = "sushruta";
            if (this.hansanet != "Disable Hansanet")
                prediction = "hansasushruta";


            //System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            //{

            foreach (var item in PatientReport.OSPosteriorReportDatas)
            {
                if (item.IsChecked)
                {
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
                }
            }
            foreach (var item in PatientReport.ODPosteriorReportDatas)
            {
                if (item.IsChecked)
                {
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
                }
            }

            //});
        }
    }
}
