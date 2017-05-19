using Artelus.Model;
using Artelus.Views;
using FirstFloor.ModernUI.Windows.Controls;
using Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Artelus.ViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        public ObservableCollection<PatientEntity> Patients { get; set; }
        public DelegateCommand ViewProfileCommand { get; set; }
        public DelegateCommand ViewReportCommand { get; set; }

        public SearchViewModel()
        {
            Patients = new ObservableCollection<PatientEntity>();
            ViewProfileCommand = new DelegateCommand(OnViewProfileCommand);
            ViewReportCommand = new DelegateCommand(OnViewReportCommand);

            var result = new Patient().GetAll();
            foreach (var item in result)
            {
                item.MaritalStatus = item.MaritalStatus == "yes" ? "Y" : "N";
                Patients.Add(item);
            }
               
        }

        private void OnViewProfileCommand(object args)
        {
            var model = args as PatientEntity;
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var cameraView = (win) as Artelus.MainWindow;
                    cameraView.ContentSource = new Uri("Views/PatientProfileView.xaml", UriKind.Relative);
                    cameraView.DataContext = new ProfileViewModel(model);
                }
            }
        }

        private void OnViewReportCommand(object args)
        {
            var model = args as PatientEntity;
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    var cameraView = (win) as Artelus.MainWindow;
                    cameraView.ContentSource = new Uri("Views/ReportView.xaml", UriKind.Relative);
                    cameraView.DataContext = new ReportViewModel(model);
                }
            }

            //var reportVM = new ReportViewModel(model);
            //var window = new ModernWindow
            //{
            //    Style = (Style)App.Current.Resources["BlankWindow"],
            //    Title = "Camera",
            //    IsTitleVisible = true,
            //    WindowState = WindowState.Maximized
            //};
            //window.Content = new ReportView(reportVM, window);
            //var closeResult = window.ShowDialog();
        }
    }
}
