using Artelus.Common;
using Artelus.Model;
using FirstFloor.ModernUI.Windows.Controls;
using Helpers;
using System;
using System.Windows;

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

        public LoginViewModel()
        {
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
                    //if (result.IsConfigured)
                    //    Helper.Alert("Please Update Application Setting", false);
                    //else
                    //    Helper.Alert(string.Empty, true);



                    Helper.Profile(result.IsConfigured);
                    Clear();
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
