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
        private string docNm;
        private string hospitalNm;
        private string hospitalID;
        private string hospitalScreening;
        private string currentMedications;
        private string laserTreatment;
        private string cataract;
        private string hypertension;
        private string allergyDrugs;
        private string allergyDrugsDtl;
        private string diabetic;
        private string info;
        private string emergContactNm;
        private string emergPh;
        private string statedConsentPerson;
        private string relation;
        private string termsCondition;
        private string medicalInsurance;
        private int collectionID;
        private Guid installID;
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
        public string DocNm
        {
            get { return docNm; }
            set
            {
                if (docNm != value)
                {
                    docNm = value;
                    RaisePropertyChange("DocNm");
                }
            }
        }
        public string HospitalNm
        {
            get { return hospitalNm; }
            set
            {
                if (hospitalNm != value)
                {
                    hospitalNm = value;
                    RaisePropertyChange("HospitalNm");
                }
            }
        }
        public string HospitalID
        {
            get { return hospitalID; }
            set
            {
                if (hospitalID != value)
                {
                    hospitalID = value;
                    RaisePropertyChange("HospitalID");
                }
            }
        }
        public string HospitalScreening
        {
            get { return hospitalScreening; }
            set
            {
                if (hospitalScreening != value)
                {
                    hospitalScreening = value;
                    RaisePropertyChange("HospitalScreening");
                }
            }
        }
        public string CurrentMedications
        {
            get { return currentMedications; }
            set
            {
                if (currentMedications != value)
                {
                    currentMedications = value;
                    RaisePropertyChange("CurrentMedications");
                }
            }
        }
        public string LaserTreatment
        {
            get { return laserTreatment; }
            set
            {
                if (laserTreatment != value)
                {
                    laserTreatment = value;
                    RaisePropertyChange("LaserTreatment");
                }
            }
        }
        public string Cataract
        {
            get { return cataract; }
            set
            {
                if (cataract != value)
                {
                    cataract = value;
                    RaisePropertyChange("Cataract");
                }
            }
        }
        public string Hypertension
        {
            get { return hypertension; }
            set
            {
                if (hypertension != value)
                {
                    hypertension = value;
                    RaisePropertyChange("Hypertension");
                }
            }
        }
        public string AllergyDrugs
        {
            get { return allergyDrugs; }
            set
            {
                if (allergyDrugs != value)
                {
                    allergyDrugs = value;
                    RaisePropertyChange("AllergyDrugs");
                }
            }
        }
        public string AllergyDrugsDtl
        {
            get { return allergyDrugsDtl; }
            set
            {
                if (allergyDrugsDtl != value)
                {
                    allergyDrugsDtl = value;
                    RaisePropertyChange("AllergyDrugsDtl");
                }
            }
        }
        public string Diabetic
        {
            get { return diabetic; }
            set
            {
                if (diabetic != value)
                {
                    diabetic = value;
                    RaisePropertyChange("Diabetic");
                }
            }
        }
        public string Info
        {
            get { return info; }
            set
            {
                if (info != value)
                {
                    info = value;
                    RaisePropertyChange("Info");
                }
            }
        }
        public string EmergContactNm
        {
            get { return emergContactNm; }
            set
            {
                if (emergContactNm != value)
                {
                    emergContactNm = value;
                    RaisePropertyChange("EmergContactNm");
                }
            }
        }
        public string EmergPh
        {
            get { return emergPh; }
            set
            {
                if (emergPh != value)
                {
                    emergPh = value;
                    RaisePropertyChange("EmergPh");
                }
            }
        }
        public string StatedConsentPerson
        {
            get { return statedConsentPerson; }
            set
            {
                if (statedConsentPerson != value)
                {
                    statedConsentPerson = value;
                    RaisePropertyChange("StatedConsentPerson");
                }
            }
        }
        public string Relation
        {
            get { return relation; }
            set
            {
                if (relation != value)
                {

                    relation = value;
                    RaisePropertyChange("Relation");
                }
            }
        }
        public string TermsCondition
        {
            get { return termsCondition; }
            set
            {
                if (termsCondition != value)
                {
                    termsCondition = value;
                    RaisePropertyChange("TermsCondition");
                }
            }
        }
        public int CollectionID
        {
            get { return collectionID; }
            set
            {
                if (collectionID != value)
                {
                    collectionID = value;
                    RaisePropertyChange("CollectionID");
                }
            }
        }
        public string MedicalInsurance
        {
            get { return medicalInsurance; }
            set
            {
                if (medicalInsurance != value)
                {
                    medicalInsurance = value;
                    RaisePropertyChange("MedicalInsurance");
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
        public Guid InstallID
        {
            get { return installID; }
            set
            {
                if (installID != value)
                {
                    installID = value;
                    RaisePropertyChange("InstallID");
                }
            }
        }
        private Guid uniqueID;
        public Guid UniqueID
        {
            get { return uniqueID; }
            set
            {
                if (uniqueID != value)
                {
                    uniqueID = value;
                    RaisePropertyChange("UniqueID");
                }
            }
        }

        public string this[string columnName] => Validate(columnName);


        public ObservableCollection<ReportData> ReportDatas { get; set; }
        public ObservableCollection<ReportData> PosteriorReportDatas { get; set; }
        public ObservableCollection<ReportData> AnteriorReportDatas { get; set; }

        private ObservableCollection<ReportData> _OSPosteriorReportDatas { get; set; }
        public ObservableCollection<ReportData> OSPosteriorReportDatas
        {
            get { return _OSPosteriorReportDatas; }
            set
            {
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
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        private string Validate(string properyName)
        {
            string validationMessgae = string.Empty;
            switch (properyName)
            {
                case "HospitalScreening":
                    if (String.IsNullOrEmpty(hospitalScreening))
                        validationMessgae = "Cannot be empty.";
                    break;
                case "TermsCondition":
                    if (String.IsNullOrEmpty(termsCondition))
                        validationMessgae = "Cannot be empty.";
                    break;
            }
            return validationMessgae;
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
        private bool isChecked { get; set; }
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
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public string this[string columnName] => Validate(columnName);
        //  public string Error => throw new NotImplementedException();
        private string Validate(string properyName)
        {
            string validationMessgae = string.Empty;
            return validationMessgae;
        }
    }

}
