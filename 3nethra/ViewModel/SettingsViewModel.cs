using Helpers;
using System;
using Artelus.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;
using System.Net;
using System.Configuration;
using System.Web.Script.Serialization;
using System.IO;

namespace Artelus.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public Action CloseAction { get; set; }
        private string msg;
        public string Msg
        {
            get { return msg; }
            set
            {
                msg = value;
                RaisePropertyChange("Msg");
            }
        }
        private UserEntity user;
        public UserEntity User
        {
            get { return user; }
            set
            {
                user = value;
                RaisePropertyChange("User");
            }
        }
        public DelegateCommand SaveCommand { get; set; }
        public SettingsViewModel(UserEntity user)
        {
            User = user;
            User.InstallID = Guid.NewGuid();
            SaveCommand = new DelegateCommand(OnSaveCommand);
        }
        private void OnSaveCommand(object args)
        {
            if (string.IsNullOrEmpty(User.Location) || string.IsNullOrEmpty(User.PinCode))
                Msg = "Invalid Data";
            else
            {
                user.IsConfigured = true;
                string status = UpdateInstallation(user);
                if (status == "success")
                {
                    if (new User().Update(User))
                        this.CloseAction();
                    else
                        Msg = "Invalid Data";
                }
                else ModernDialog.ShowMessage("Internel Server Error. Please contact your administrator", "Alert", MessageBoxButton.OK);
            }
        }

        private string UpdateInstallation(UserEntity user)
        {
            string result = string.Empty;
            string url = ConfigurationManager.AppSettings["installationAPI"].ToString();
            HttpWebRequest httpWebRequest = null;
            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage("An error has occurred on the server", "Alert", MessageBoxButton.OK);
            }
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            object objct = new
            {
                username = user.UserNm,
                password = user.Pwd,
                location_name = user.Location,
                location_pincode = user.PinCode,
                install_id = user.InstallID.ToString()
            };
            var json = new JavaScriptSerializer().Serialize(objct);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            APIResult val = new JavaScriptSerializer().Deserialize<APIResult>(result);
            return val.status;
        }
    }

    internal class APIResult
    {
        public string status { get; set; }
        public object allData { get; set; }
    }
}
