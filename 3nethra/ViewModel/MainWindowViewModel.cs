using Artelus.Common;
using Artelus.ViewModel;
using Helpers;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Artelus
{
    public class MainWindowViewModel : BaseViewModel
    {
        //private PatientViewModel patientViewModel = new PatientViewModel();
        private LoginViewModel loginViewModel = new LoginViewModel();
        private BaseViewModel _CurrentViewModel;
        private string alert;
        private bool isAuthorize;
        private bool isVisible;
        public BaseViewModel CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }
        public DelegateCommand NavCommand { get; set; }
        public ICommand LogOffCommand { get; set; }
        public bool IsAuthorize
        {
            get { return isAuthorize; }
            set
            {
                if (isAuthorize != value)
                {
                    isAuthorize = value;
                    RaisePropertyChange("IsAuthorize");
                }
            }
        }
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    RaisePropertyChange("IsVisible");
                }
            }
        }
        public string Alert
        {
            get { return alert; }
            set
            {
                alert = value;
                RaisePropertyChange("Alert");
            }
        }
        public MainWindowViewModel() {
            NavCommand = new DelegateCommand(OnNavCommand);
            CurrentViewModel = loginViewModel;
            LogOffCommand = new DelegateCommand(OnLogOffCommand);
            if (Program.Principal != null)
                IsAuthorize = true;
            else
                IsAuthorize = false;
        }
        private void OnNavCommand(object args)
        {
            string destination = args as string;
            switch (destination)
            {
                case "Patient":
                    //CurrentViewModel = patientViewModel;
                    break;
                case "Login":
                default:
                    CurrentViewModel = loginViewModel;
                    break;
            }
        }

        private void OnLogOffCommand(object args)
        {
            Helper.LogOff();
        }
    }
}
