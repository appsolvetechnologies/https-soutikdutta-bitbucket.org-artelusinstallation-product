using Artelus.Common;
using Artelus.Model;
using Artelus.Views;
using FirstFloor.ModernUI.Windows.Controls;
using Helpers;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Input;

namespace Artelus.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string logo;
        private UserEntity user;
        public UserEntity User
        {
            get { return user; }
            set { user = value; }
        }
        public string Logo
        {
            get { return logo; }
            set
            {
                logo = value;
                RaisePropertyChange("Logo");
            }
        }
        public DelegateCommand LoginCommand { get; set; }
        public ICommand LogOffCommand { get; set; }

        private void OnLogOffCommand(object args)
        {
            Helper.LogOff();
        }
        public LoginViewModel()
        {
            LogOffCommand = new DelegateCommand(OnLogOffCommand);
            Logo = AppDomain.CurrentDomain.BaseDirectory + "Resources\\logo.png";
            User = new UserEntity();
            LoginCommand = new DelegateCommand(OnLoginCommand);
        }

        private void OnLoginCommand(object args)
        {
            var model = args as UserEntity;
            if (model != null)
            {
                var result = new User().Validate(model.UserNm, model.Pwd);
                if (result != null && result.Id > 0)
                {
                    string[] roles = { EnumRoles.Administrator.ToString() };
                    Program.SetIdentity(result.Id.ToString(), roles);
                    if (!result.IsConfigured)
                    {
                        var settingVM = new SettingsViewModel(result);
                        var window = new ModernWindow
                        {
                            Style = (Style)App.Current.Resources["BlankWindow"],
                            Title = "Settings",
                            IsTitleVisible = true
                        };
                        window.Content = new SettingsView(settingVM, window);
                        window.Width = SystemParameters.MaximizedPrimaryScreenWidth - (SystemParameters.MaximizedPrimaryScreenWidth - 500);
                        window.Height = SystemParameters.MaximizedPrimaryScreenHeight - (SystemParameters.MaximizedPrimaryScreenHeight - 420);
                        var closeResult = window.ShowDialog();
                    }
                    Clear();
                    Helper.Profile(result.IsConfigured);
                }
                else ModernDialog.ShowMessage("Incorrect UserName or Password!", "Login", MessageBoxButton.OK);
            }
        }

        private void Clear()
        {
            User.Id = 0;
            User.UserNm = string.Empty;
            User.Pwd = string.Empty;
        }

    }
}
