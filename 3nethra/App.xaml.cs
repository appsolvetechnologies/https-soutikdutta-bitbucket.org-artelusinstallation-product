using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Artelus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //string relative = @"D:\Project\3nethra\3nethra\3nethra\3nethra.sdf";
            string localDB = ConfigurationManager.AppSettings["localDB"].ToString();
            string basedir = AppDomain.CurrentDomain.BaseDirectory;

            string path = Path.Combine(basedir, localDB);
            string absolute = Path.GetDirectoryName(path);
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);


            MainWindow window = new MainWindow();
            window.ContentSource = new Uri("Views/LoginView.xaml", UriKind.Relative);
            Application.Current.MainWindow = window;

            window.InitializeComponent();
            window.Show();
        }
    }
}
