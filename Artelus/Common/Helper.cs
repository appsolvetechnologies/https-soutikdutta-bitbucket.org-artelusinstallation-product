using Artelus.ViewModel;
using System;
using System.IO;
using System.Windows;

namespace Artelus.Common
{
    public class Helper
    {
        public static string BaseDirectory(string file)
        {
            //var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Artelus";
            return Path.Combine(directory, file);
        }

        public static void LogOff()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var artelus = (win) as Artelus.MainWindow;
                    artelus.ContentSource = new Uri("Views/LoginView.xaml", UriKind.Relative);
                    var dataContext = artelus.DataContext as MainWindowViewModel;
                    if (dataContext != null)
                        dataContext.IsAuthorize = false;
                    else
                    {
                        var vm = new MainWindowViewModel();
                        vm.IsAuthorize = false;
                        artelus.DataContext = vm;
                    }
                }
            }
        }

        public static string ContactEmails() => "pwalia@artelus.com,rkodhandapani@artelus.com";

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
        public static string ReadAllTextReportFile()
        {
            return System.IO.File.ReadAllText(Program.BaseDir() + "\\Resources\\report.html");
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
