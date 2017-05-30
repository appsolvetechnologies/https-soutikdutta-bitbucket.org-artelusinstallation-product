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
using Artelus.Common;

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
            if (string.IsNullOrEmpty(User.Location) || string.IsNullOrEmpty(User.PinCode) || string.IsNullOrEmpty(User.Email) || string.IsNullOrEmpty(User.Address))
                Msg = "Invalid Data";
            else
            {
                APIResult result = UpdateInstallation(user);
                if (result.status == "ok")
                {
                    User.IsConfigured = true;
                    User.Token = result.token;
                    if (new User().Update(User))
                        this.CloseAction();
                    else
                        Msg = "Invalid Data";
                }
                else ModernDialog.ShowMessage("Internel Server Error. Please contact your administrator", "Alert", MessageBoxButton.OK);
            }
        }

        private APIResult UpdateInstallation(UserEntity user)
        {
            APIResult result = new APIResult();
            string url = ConfigurationManager.AppSettings["installationAPI"].ToString();
            object objct = new
            {
                password = "admin@123!@#",
                email = "monilal@artelus.com",
                address=user.Address,
                location_name = user.Location,
                location_pincode = user.PinCode,
                install_id = user.InstallID.ToString()
            };
            var json = new JavaScriptSerializer().Serialize(objct);
            try
            {
                result = RestCalls.SyncReport(url, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                ModernDialog.ShowMessage("An error has occurred on the server", "Alert", MessageBoxButton.OK);
            }
            return result;
        }
    }
}
