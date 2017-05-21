using Artelus.Model;
using Artelus.Views;
using FirstFloor.ModernUI.Windows.Controls;
using Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Artelus.ViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        public string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    RaisePropertyChange("SearchText");
                }
            }
        }
        private IdName selectedOption;
        public IdName SelectedOption
        {
            get { return selectedOption; }
            set
            {
                selectedOption = value;
                RaisePropertyChange("SelectedOption");
            }
        }
        private ObservableCollection<PatientEntity> patients = new ObservableCollection<PatientEntity>();
        public ObservableCollection<PatientEntity> Patients
        {
            get { return patients; }
            set
            {
                if (value != patients)
                {
                    patients = value;
                    RaisePropertyChange("Patients");
                }
            }
        }
        public DelegateCommand ViewProfileCommand { get; set; }
        public DelegateCommand ViewReportCommand { get; set; }
        public IdNameCollection FilterCollection { get; set; } = new IdNameCollection();
        public DelegateCommand SearchCommand { get; set; }
        public SearchViewModel()
        {
            Patients = new ObservableCollection<PatientEntity>();
            ViewProfileCommand = new DelegateCommand(OnViewProfileCommand);
            ViewReportCommand = new DelegateCommand(OnViewReportCommand);
            SearchCommand = new DelegateCommand(OnSearchCommand);
            Patients = new Patient().GetAll();
            //foreach (var item in result)
            //{
            //    item.MaritalStatus = item.MaritalStatus == "yes" ? "Y" : "N";
            //    Patients.Add(item);
            //}
            FilterCollection.Add(new IdName() { Id = "p_id", Name = "Patient ID" });
            FilterCollection.Add(new IdName() { Id = "name", Name = "Patient Name" });
            FilterCollection.Add(new IdName() { Id = "p_email", Name = "Email" });
            FilterCollection.Add(new IdName() { Id = "mobile", Name = "Mobile" });
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

        private void OnSearchCommand(object args)
        {
            if (SelectedOption.Id.ToString() == "p_id")
            {
                int n;
                if (!int.TryParse(SearchText, out n))
                {
                    ModernDialog.ShowMessage("Patient ID should be a number", "Error", MessageBoxButton.OK);
                    return;
                }
            }
            Patients = new Patient().GetAll(SelectedOption.Id.ToString(), SearchText);
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
                    cameraView.DataContext = new ReportViewModel(model, null);
                }
            }
        }
    }
}
