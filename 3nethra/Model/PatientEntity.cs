using Helpers;
using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Artelus.Model
{
    public class PatientEntity : BaseViewModel, IDataErrorInfo
    {
        private int id;
        private string nm;
        private string lNm;
        private string mNm;
        private string notResident;
        private string ifResidentOfM;
        private string icNumber;
        private string otherOption;
        private string othersID;
        private string docNm;
        private string hospitalNm;
        private string hospitalID;
        private string hospitalScreening;
        private string email;
        private string maritalStatus;
        private int age;
        private string sex;
        private string perAdr;
        private string area;
        private string residentPh;
        private string mob;
        private string occupation;
        private string workingAt;
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
        private int collectionID;
        private Guid installID;
        private DateTime cDt;
        private DateTime mDt;
        private Guid uniqueID;
        private int errorCount;
        private string errMsg;
        private string profile;
        private string medicalInsurance;
        private string previousState;
        private bool showRelation;
        public string PreviousState
        {
            get { return previousState; }
            set
            {
                previousState = value;
                RaisePropertyChange("PreviousState");
            }
        }
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
        public string Nm
        {
            get { return nm; }
            set
            {
                if (nm != value)
                {
                    nm = value;
                    RaisePropertyChange("Nm");
                }
            }
        }
        public string LNm
        {
            get { return lNm; }
            set
            {
                if (lNm != value)
                {
                    lNm = value;
                    RaisePropertyChange("LNm");
                }
            }
        }
        public string MNm
        {
            get { return mNm; }
            set
            {
                if (mNm != value)
                {
                    mNm = value;
                    RaisePropertyChange("MNm");
                }
            }
        }
        public string NotResident
        {
            get { return notResident; }
            set
            {
                if (notResident != value)
                {
                    notResident = value;
                    RaisePropertyChange("NotResident");
                }
            }
        }
        public string IfResidentOfM
        {
            get { return ifResidentOfM; }
            set
            {
                if (ifResidentOfM != value)
                {
                    ifResidentOfM = value;
                    RaisePropertyChange("IfResidentOfM");
                }
            }
        }
        public string IcNumber
        {
            get { return icNumber; }
            set
            {
                if (icNumber != value)
                {
                    icNumber = value;
                    RaisePropertyChange("IcNumber");
                }
            }
        }
        public string OtherOption
        {
            get { return otherOption; }
            set
            {
                if (otherOption != value)
                {
                    otherOption = value;
                    RaisePropertyChange("OtherOption");
                }
            }
        }
        public bool ShowRelation
        {
            get { return showRelation; }
            set
            {
                if (showRelation != value)
                {
                    showRelation = value;
                    RaisePropertyChange("ShowRelation");
                }
            }
        }
        public string OthersID
        {
            get { return othersID; }
            set
            {
                if (othersID != value)
                {
                    othersID = value;
                    RaisePropertyChange("OthersID");
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
        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    RaisePropertyChange("Email");
                }
            }
        }
        public string MaritalStatus
        {
            get { return maritalStatus; }
            set
            {
                if (maritalStatus != value)
                {
                    maritalStatus = value;
                    RaisePropertyChange("MaritalStatus");
                }
            }
        }
        public int Age
        {
            get { return age; }
            set
            {
                if (age != value)
                {
                    age = value;
                    RaisePropertyChange("Age");
                }
            }
        }
        public string Sex
        {
            get { return sex; }
            set
            {
                if (sex != value)
                {
                    sex = value;
                    RaisePropertyChange("Sex");
                }
            }
        }
        public string PerAdr
        {
            get { return perAdr; }
            set
            {
                if (perAdr != value)
                {
                    perAdr = value;
                    RaisePropertyChange("PerAdr");
                }
            }
        }
        public string Area
        {
            get { return area; }
            set
            {
                if (area != value)
                {
                    area = value;
                    RaisePropertyChange("Area");
                }
            }
        }
        public string ResidentPh
        {
            get { return residentPh; }
            set
            {
                if (residentPh != value)
                {
                    residentPh = value;
                    RaisePropertyChange("ResidentPh");
                }
            }
        }
        public string Mob
        {
            get { return mob; }
            set
            {
                if (mob != value)
                {
                    mob = value;
                    RaisePropertyChange("Mob");
                }
            }
        }
        public string Occupation
        {
            get { return occupation; }
            set
            {
                if (occupation != value)
                {
                    occupation = value;
                    RaisePropertyChange("Occupation");
                }
            }
        }
        public string WorkingAt
        {
            get { return workingAt; }
            set
            {
                if (workingAt != value)
                {
                    workingAt = value;
                    RaisePropertyChange("WorkingAt");
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
                    if (this.Nm != null && !this.Nm.Contains(value))
                        this.ShowRelation = true;
                    else
                        this.ShowRelation = false;

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
        public DateTime MDt
        {
            get { return mDt; }
            set
            {
                if (mDt != value)
                {
                    mDt = value;
                    RaisePropertyChange("MDt");
                }
            }
        }
        public DateTime CDt
        {
            get { return cDt; }
            set
            {
                if (cDt != value)
                {
                    cDt = value;
                    RaisePropertyChange("CDt");
                }
            }
        }
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
        public string ErrMsg
        {
            get { return errMsg; }
            set
            {
                if (errMsg != value)
                {
                    errMsg = value;
                    RaisePropertyChange("ErrMsg");
                }
            }
        }
        public int ErrorCount
        {
            get { return errorCount; }
            set
            {
                if (errorCount != value)
                {
                    errorCount = value;
                    RaisePropertyChange("ErrorCount");
                }
            }
        }
        public string Profile
        {
            get { return profile; }
            set
            {
                if (profile != value)
                {
                    profile = value;
                    RaisePropertyChange("Profile");
                }
            }
        }
        public string this[string columnName]
        {
            get
            {
                return Validate(columnName);
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
                case "Email":
                    if (String.IsNullOrEmpty(email))
                        validationMessgae = "Cannot be empty.";
                    else if (!IsValidEmail(email))
                    {
                        validationMessgae = "Please enter a valid email id.";
                    }
                    break;
                case "Nm":
                    if (String.IsNullOrEmpty(nm))
                        validationMessgae = "Cannot be empty.";
                    break;
                case "LNm":
                    if (String.IsNullOrEmpty(lNm))
                        validationMessgae = "Cannot be empty.";
                    break;
                case "MaritalStatus":
                    if (String.IsNullOrEmpty(maritalStatus))
                        validationMessgae = "Cannot be empty.";
                    break;
                case "Age":

                    if (age <= 0 || age > 120)
                    {
                        validationMessgae = "Please enter a valid age.";
                    }

                    break;

                case "HospitalScreening":
                    if (String.IsNullOrEmpty(hospitalScreening))
                        validationMessgae = "Cannot be empty.";
                    break;

                case "Area":
                    if (String.IsNullOrEmpty(area))
                        validationMessgae = "Cannot be empty.";
                    break;

                case "Mob":
                    if (String.IsNullOrEmpty(mob))
                        validationMessgae = "Cannot be empty.";
                    else if (mob.Length < 10 || mob.Length > 15)
                    {
                        validationMessgae = "Please enter a valid mobile number.";
                    }
                    break;

                case "Sex":
                    if (String.IsNullOrEmpty(sex))
                        validationMessgae = "Cannot be empty.";
                    break;
                case "TermsCondition":
                    if (String.IsNullOrEmpty(termsCondition))
                        validationMessgae = "Cannot be empty.";
                    break;

            }
            return validationMessgae;
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
