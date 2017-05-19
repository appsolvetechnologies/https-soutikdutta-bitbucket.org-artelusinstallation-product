using Helpers;
using System;
using Artelus.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;

namespace Artelus.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public Action CloseAction { get; set; }
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
            if (string.IsNullOrEmpty(User.Location) && string.IsNullOrEmpty(User.PinCode))
                ModernDialog.ShowMessage("Invalid Data", "Login", MessageBoxButton.OK);
            else
            {
                user.IsConfigured = true;
                if (new User().Update(User))
                    this.CloseAction();
                else
                    ModernDialog.ShowMessage("Invalid Data", "Login", MessageBoxButton.OK);
            }
        }
    }
}
