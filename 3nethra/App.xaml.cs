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

           // AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

            string relative = @"D:\Project\3nethra\3nethra\3nethra\3nethra.sdf";


            string localDB = ConfigurationManager.AppSettings["localDB"].ToString();
            string basedir = AppDomain.CurrentDomain.BaseDirectory;

            string path = Path.Combine(basedir, localDB);
            string absolute = Path.GetDirectoryName(relative);
            //AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);

            //string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Forus\3nethra";
            //AppDomain.CurrentDomain.SetData("DataDirectory", path);
            
            MainWindow window = new MainWindow();
            window.ContentSource = new Uri("Views/LoginView.xaml", UriKind.Relative);
            Application.Current.MainWindow = window;

            window.InitializeComponent();
            window.Show();
        }
    }
}
