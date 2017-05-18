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
        public DelegateCommand ViewCommand { get; set; }

        public SearchViewModel()
        {
            Patients = new ObservableCollection<PatientEntity>();
            ViewCommand = new DelegateCommand(OnViewCommand);
            var result = new Patient().GetAll();
            foreach (var item in result)
                Patients.Add(item);
        }

        private void OnViewCommand(object args)
        {
            var model = args as PatientEntity;
            var reportVM = new ReportViewModel(model);
            var window = new ModernWindow
            {
                Style = (Style)App.Current.Resources["BlankWindow"],
                Title = "Camera",
                IsTitleVisible = true,
                WindowState = WindowState.Maximized
            };
            window.Content = new ReportView(reportVM, window);
            var closeResult = window.ShowDialog();
        }
    }
}
