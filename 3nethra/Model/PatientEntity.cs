using Helpers;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace Artelus.Model
{
    public class PatientEntity : BaseViewModel, IDataErrorInfo
    {
        private int id;
        private int patientId;
        private string nm;
        private string lNm;
        private string mNm;
        private string notResident;
        private string ifResidentOfM;
        private string icNumber;
        private string otherOption;
        private string othersID;
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
        private Guid installID;
        private DateTime cDt;
        private DateTime mDt;
        private Guid uniqueID;
        private int errorCount;
        private string errMsg;
        private string profile;
        private string previousState;
        private PatientReport patientReport;
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
        public PatientReport PatientReport
        {
            get { return patientReport; }
            set
            {
                if (patientReport != value)
                {
                    patientReport = value;
                    RaisePropertyChange("PatientReport");
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
                        validationMessgae = "Please enter a valid age.";
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

                case "IcNumber":
                    if (ifResidentOfM == "Yes" && string.IsNullOrEmpty(icNumber))
                        validationMessgae = "Cannot be empty.";
                    else if (ifResidentOfM == "Yes" && !string.IsNullOrEmpty(icNumber) && icNumber.Length != 12)
                        validationMessgae = "IC Number should be 12 digit.";
                    else if (ifResidentOfM == "Yes" && !string.IsNullOrEmpty(icNumber) && icNumber.Length == 12 && !IsValidNum(icNumber))
                        validationMessgae = "Invalid IC Number";
                    break;
                case "OtherOption":
                    if (ifResidentOfM == "No" && string.IsNullOrEmpty(otherOption))
                        validationMessgae = "Cannot be empty.";
                    break;
                case "OthersID":
                    if (ifResidentOfM == "No" && string.IsNullOrEmpty(othersID))
                        validationMessgae = "Cannot be empty.";
                    break;
            }
            return validationMessgae;
        }

        bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;

        }

        bool IsValidNum(string num)
        {
            Regex regex = new Regex(@"[\d]");
            if (regex.IsMatch(num))
                return true;
            return false;
        }
    }
}
