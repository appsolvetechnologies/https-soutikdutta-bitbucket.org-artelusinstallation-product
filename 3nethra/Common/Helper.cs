using Artelus.ViewModel;
using System;
using System.Windows;

namespace Artelus.Common
{
    public class Helper
    {
        public static void LogOff()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var dataContext = win.DataContext as MainWindowViewModel;
                    dataContext.IsAuthorize = false;
                    dataContext.CurrentViewModel = new LoginViewModel();
                }
            }
        }

        public static void Profile(bool isConfigured)
        {
            foreach (Window win in Application.Current.Windows)
            {

                if (win.GetType().Name == "MainWindow")
                {
                    var Artelus = (win) as Artelus.MainWindow;
                    //new Uri()
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Views\\PatientView.xaml";
                    //Artelus.ContentSource = new Uri(path,UriKind.Relative);
                    Artelus.ContentSource = new Uri("Views/PatientView.xaml", UriKind.Relative);
                    //if (isConfigured)
                    //    Artelus.ContentSource = new Uri("Views/PatientView.xaml", UriKind.Relative);
                    //else
                    //    Artelus.ContentSource = new Uri("Views/SettingsView.xaml", UriKind.Relative);

                }
            }
        }

        public static void Alert(string msg, bool isAuthorize = false)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                foreach (Window win in Application.Current.Windows)
                {
                    if (win.GetType().Name == "MainWindow")
                    {
                        var dataContext = win.DataContext as MainWindowViewModel;
                        dataContext.Alert = msg;

                        if (isAuthorize)
                            dataContext.IsAuthorize = true;
                        break;
                    }
                }
            }));
        }
    }
}
