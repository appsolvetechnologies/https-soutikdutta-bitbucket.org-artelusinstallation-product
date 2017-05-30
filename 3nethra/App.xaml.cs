using Artelus.Common;
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
            string relative = @"C:\Users\sushruta\Desktop\Artelus\product\3nethra\ArtelusLocal.sdf";
            string absolute = Path.GetDirectoryName(relative);
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);

            //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Artelus";
            //AppDomain.CurrentDomain.SetData("DataDirectory", path);

            MainWindow window = new MainWindow();
            window.ContentSource = new Uri("Views/LoginView.xaml", UriKind.Relative);
            Application.Current.MainWindow = window;

            window.InitializeComponent();
            window.Show();
        }
    }
}
