using Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Artelus.Model
{
    public class PatientReport : BaseViewModel, IDataErrorInfo
    {

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChange("Id");
                }
            }
        }

        private int patientId;
        public int PatientId
        {
            get { return patientId; }
            set
            {
                if (patientId != value)
                {
                    patientId = value;
                    RaisePropertyChange("PatientId");
                }
            }
        }

        private DateTime dt;
        public DateTime Dt
        {
            get { return dt; }
            set
            {
                if (dt != value)
                {
                    dt = value;
                    RaisePropertyChange("Dt");
                }
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    RaisePropertyChange("Location");
                }
            }
        }

        public string this[string columnName] => Validate(columnName);

        public string Error => throw new NotImplementedException();
        private string Validate(string properyName)
        {
            string validationMessgae = string.Empty;
            return validationMessgae;
        }
        public ObservableCollection<ReportData> ReportDatas { get; set; }
        public ObservableCollection<ReportData> PosteriorReportDatas { get; set; }
        public ObservableCollection<ReportData> AnteriorReportDatas { get; set; }

        private ObservableCollection<ReportData> _OSPosteriorReportDatas { get; set; }

        public ObservableCollection<ReportData> OSPosteriorReportDatas {
            get { return _OSPosteriorReportDatas; }
            set {
                if (_OSPosteriorReportDatas != value)
                {
                    _OSPosteriorReportDatas = value;
                    RaisePropertyChange("OSPosteriorReportDatas");
                }
            }
        }
        private ObservableCollection<ReportData> _ODPosteriorReportDatas { get; set; }

        public ObservableCollection<ReportData> ODPosteriorReportDatas
        {
            get { return _ODPosteriorReportDatas; }
            set
            {
                if (_ODPosteriorReportDatas != value)
                {
                    _ODPosteriorReportDatas = value;
                    RaisePropertyChange("ODPosteriorReportDatas");
                }
            }
        }
    }

    public class ReportData : BaseViewModel, IDataErrorInfo
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChange("Id");
                }
            }
        }
        private int patientReportId { get; set; }
        public int PatientReportId
        {
            get { return patientReportId; }
            set
            {
                if (patientReportId != value)
                {
                    patientReportId = value;
                    RaisePropertyChange("PatientReportId");
                }
            }
        }
        private string mode { get; set; }
        public string Mode
        {
            get { return mode; }
            set
            {
                if (mode != value)
                {
                    mode = value;
                    RaisePropertyChange("Mode");
                }
            }
        }
        private string img { get; set; }
        public string Img
        {
            get { return img; }
            set
            {
                if (img != value)
                {
                    img = value;
                    RaisePropertyChange("Img");
                }
            }
        }
        private string imageUrl { get; set; }
        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                if (imageUrl != value)
                {
                    imageUrl = value;
                    RaisePropertyChange("ImageUrl");
                }
            }
        }
        private string eye { get; set; }
        public string Eye
        {
            get { return eye; }
            set
            {
                if (eye != value)
                {
                    eye = value;
                    RaisePropertyChange("Eye");
                }
            }
        }
        private string prediction { get; set; }
        public string Prediction
        {
            get { return prediction; }
            set
            {
                if (prediction != value)
                {
                    prediction = value;
                    RaisePropertyChange("Prediction");
                }
            }
        }
        private string size { get; set; }
        public string Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;
                    RaisePropertyChange("Size");
                }
            }
        }
        private string fileName { get; set; }
        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    RaisePropertyChange("FileName");
                }
            }
        }
        private bool isChecked{get;set;}
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    RaisePropertyChange("IsChecked");
                }
            }
        }

        private BitmapImage bitMapImg;

        public BitmapImage BitMapImg
        {
            get { return bitMapImg; }
            set
            {
                if (bitMapImg != value)
                {
                    bitMapImg = value;
                    RaisePropertyChange("BitMapImg");
                }
            }
        }


        private bool status { get; set; }
        public bool Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    RaisePropertyChange("Status");
                }
            }
        }

        public string this[string columnName] => Validate(columnName);

        public string Error => throw new NotImplementedException();
        private string Validate(string properyName)
        {
            string validationMessgae = string.Empty;
            return validationMessgae;
        }
    }

}
