﻿using Artelus.ViewModel;
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
                    var artelus = (win) as Artelus.MainWindow;
                    artelus.ContentSource = new Uri("Views/LoginView.xaml", UriKind.Relative);
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
                    var artelus = (win) as Artelus.MainWindow;
                    artelus.ContentSource = new Uri("Views/PatientView.xaml", UriKind.Relative);
                    var dataContext = artelus.DataContext as MainWindowViewModel;
                    dataContext.IsAuthorize = true;
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
