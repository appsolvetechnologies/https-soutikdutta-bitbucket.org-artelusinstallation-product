using Helpers;
using System;
using System.ComponentModel;

namespace Artelus.Model
{
    public class UserEntity : BaseViewModel, IDataErrorInfo
    {
        private int id;
        private string userNm;
        private string pwd;
        private bool isConfigured;
        private Guid installID;
        private string location;
        private string pinCode;
        private string token;
        private string email;
        private string address;
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
        public string UserNm
        {
            get { return userNm; }
            set
            {
                if (userNm != value)
                {
                    userNm = value;
                    RaisePropertyChange("UserNm");
                }
            }
        }
        public string Pwd
        {
            get { return pwd; }
            set
            {
                if (pwd != value)
                {
                    pwd = value;
                    RaisePropertyChange("Pwd");
                }
            }
        }
        public bool IsConfigured
        {
            get { return isConfigured; }
            set
            {
                if (isConfigured != value)
                {
                    isConfigured = value;
                    RaisePropertyChange("IsConfigured");
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
        public string PinCode
        {
            get { return pinCode; }
            set
            {
                if (pinCode != value)
                {
                    pinCode = value;
                    RaisePropertyChange("PinCode");
                }
            }
        }
        public string Token
        {
            get { return token; }
            set
            {
                if (token != value)
                {
                    token = value;
                    RaisePropertyChange("Token");
                }
            }
        }
        public string Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    address = value;
                    RaisePropertyChange("Address");
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
                case "UserNm":
                    if (String.IsNullOrEmpty(userNm))
                        validationMessgae = "Cannot be empty.";
                    else if (userNm?.Length < 4)
                        validationMessgae = "Minimum 4 characters required!";
                    break;

                case "Pwd":
                    if (String.IsNullOrEmpty(pwd))
                        validationMessgae = "Cannot be empty.";
                    else if (pwd.Length >= 50)
                        validationMessgae = "Maximum 50 characters only.";
                    break;

                case "Location":
                    if (String.IsNullOrEmpty(location))
                        validationMessgae = "";
                    break;

                case "PinCode":
                    if (String.IsNullOrEmpty(pinCode))
                        validationMessgae = "";
                    break;
            }
            return validationMessgae;
        }
    }

    public class APIResult
    {
        public string status { get; set; }
        public string token { get; set; }
        public string msg { get; set; }
        public int user { get; set; }
    }

}
