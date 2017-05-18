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
            return validationMessgae;
        }
    }
}
