using Helpers;
using Artelus.Model;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;
using Artelus.Views;

namespace Artelus.ViewModel
{
    public class PatientViewModel : BaseViewModel
    {
        //Properties 
        private bool showOtherOption;
        public bool ShowOtherOption
        {
            get { return showOtherOption; }
            set
            {
                showOtherOption = value;
                RaisePropertyChange("ShowOtherOption");
            }
        }

        private bool initialLoad = true;
        public bool InitialLoad
        {
            get { return initialLoad; }
            set
            {
                initialLoad = value;
                RaisePropertyChange("InitialLoad");
            }
        }

        private bool showAllergyOption;
        public bool ShowAllergyOption
        {
            get { return showAllergyOption; }
            set
            {
                showAllergyOption = value;
                RaisePropertyChange("ShowAllergyOption");
            }
        }

        private bool hideOthers;
        public bool HideOthers
        {
            get { return hideOthers; }
            set
            {
                hideOthers = value;
                RaisePropertyChange("HideOthers");
            }
        }
        private PatientEntity patient;
        public PatientEntity PatientEntity
        {
            get { return patient; }
            set
            {
                patient = value;
                RaisePropertyChange("PatientEntity");

            }
        }
        private IdName selectedOption;
        public IdName SelectedOption
        {
            get { return selectedOption; }
            set
            {
                if (!InitialLoad)
                    this.PatientEntity.OthersID = string.Empty;
                if (value != selectedOption)
                {
                    if (PatientEntity != null && value != null && value.Name != "Others")
                    {
                        PatientEntity.OtherOption = value.Name;
                        this.ShowOtherOption = false;
                    }
                    else if (PatientEntity != null && value != null && value.Name == "Others")
                    {
                        PatientEntity.OtherOption = string.Empty;
                        this.ShowOtherOption = true;
                    }
                    else
                        this.ShowOtherOption = false;

                    selectedOption = value;
                    InitialLoad = false;
                    RaisePropertyChange("SelectedOption");
                }
            }
        }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand UpdateCommand { get; set; }
        public DelegateCommand ResidentChangeCommand { get; set; }
        public DelegateCommand AlergyChangeCommand { get; set; }

        public IdNameCollection OtherIDCollection { get; set; } = new IdNameCollection();
        public PatientViewModel()
        {
            Initialize();
        }

        public PatientViewModel(PatientEntity model)
        {
            Initialize();
            PatientEntity = model;
            if (PatientEntity.IfResidentOfM == "no")
            {
                this.HideOthers = true;
                if (PatientEntity.OtherOption != "Passport" && PatientEntity.OtherOption != "Driving License")
                    ShowOtherOption = true;
            }
            if (PatientEntity.AllergyDrugs == "yes")
                ShowAllergyOption = true;
        }

        private void Initialize()
        {
            PatientEntity = new PatientEntity();
            PatientEntity.IfResidentOfM = "yes";
            PatientEntity.LaserTreatment = "no";
            PatientEntity.Cataract = "no";
            PatientEntity.Diabetic = "no";
            PatientEntity.Hypertension = "no";
            PatientEntity.AllergyDrugs = "no";
            PatientEntity.CurrentMedications = "no";
            OtherIDCollection.Add(new IdName() { Id = 1, Name = "Passport" });
            OtherIDCollection.Add(new IdName() { Id = 2, Name = "Driving License" });
            OtherIDCollection.Add(new IdName() { Id = 3, Name = "Others" });
            PatientEntity.OtherOption = "Passport";
            SaveCommand = new DelegateCommand(OnSaveCommand);
            UpdateCommand = new DelegateCommand(OnUpdateCommand);
            ResidentChangeCommand = new DelegateCommand(OnResidentChangeCommand);
            AlergyChangeCommand = new DelegateCommand(OnAlergyChangeCommand);
        }

        private void OnAlergyChangeCommand(object args)
        {
            if (PatientEntity.AllergyDrugs == "yes")
            {
                PatientEntity.AllergyDrugsDtl = string.Empty;
                this.ShowAllergyOption = true;
            }
            else this.ShowAllergyOption = false;

        }

        private void OnResidentChangeCommand(object args)
        {

            if (PatientEntity.IfResidentOfM == "no")
            {
                PatientEntity.OthersID = string.Empty;
                if (SelectedOption != null && SelectedOption.Name == "Others")
                    this.ShowOtherOption = true;
                this.HideOthers = true;
            }
            else
            {
                PatientEntity.IcNumber = string.Empty;
                this.HideOthers = false;
                this.ShowOtherOption = false;
            }
        }

        private void OnSaveCommand(object args)
        {
            var model = args as PatientEntity;
            model.CDt = DateTime.UtcNow;
            model.MDt = DateTime.UtcNow;
            model.CollectionID = 123;
            model.UniqueID = Guid.NewGuid();
            model.InstallID = new Guid("a06e10b6-63a1-48b0-9da9-411f82de47fa");

            if (model.IfResidentOfM == "yes")
            {
                model.OtherOption = string.Empty;
                model.OthersID = string.Empty;
            }

            if (model.Id == 0)
            {
                model.Id = new Patient().Add(model);
                if (model.Id > 0)
                {
                    foreach (Window win in Application.Current.Windows)
                    {
                        if (win.GetType().Name == "MainWindow")
                        {
                            var cameraView = (win) as Artelus.MainWindow;
                            cameraView.ContentSource = new Uri("Views/CameraView.xaml", UriKind.Relative);
                            cameraView.DataContext = new CameraViewModel(model);
                        }
                    }
                }
            }
            else
            {
                model.MDt= DateTime.UtcNow;
                new Patient().Update(model);
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

            //var camVM = new CameraViewModel(model);
            //var window = new ModernWindow
            //{
            //    Style = (Style)App.Current.Resources["BlankWindow"],
            //    Title = "Camera",
            //    IsTitleVisible = true,
            //    WindowState = WindowState.Maximized
            //};
            //window.Content = new CameraView(camVM, window);
            //var closeResult = window.ShowDialog();
            //this.Clear();
        }

        private void OnUpdateCommand(object args)
        {
            var model = args as PatientEntity;
            PatientEntity.Id = model.Id;
            PatientEntity.Nm = model.Nm;
            PatientEntity.Email = model.Email;
            PatientEntity.PerAdr = model.PerAdr;
            PatientEntity.Mob = model.Mob;
        }

        private void Clear()
        {
            //PatientEntity = null;
            PatientEntity.Id = 0;
            PatientEntity.Nm = string.Empty;
            PatientEntity.LNm = string.Empty;
            PatientEntity.MNm = string.Empty;
            PatientEntity.Email = string.Empty;
            PatientEntity.PerAdr = string.Empty;
            PatientEntity.Mob = string.Empty;
            PatientEntity.IfResidentOfM = "yes";
            PatientEntity.MaritalStatus = string.Empty;
            PatientEntity.Age = 0;
            PatientEntity.Sex = string.Empty;
            PatientEntity.PerAdr = string.Empty;
            PatientEntity.Area = string.Empty;
            PatientEntity.ResidentPh = string.Empty;
            PatientEntity.Occupation = string.Empty;
            PatientEntity.WorkingAt = string.Empty;
            PatientEntity.DocNm = string.Empty;
            PatientEntity.HospitalNm = string.Empty;
            PatientEntity.HospitalID = string.Empty;
            PatientEntity.LaserTreatment = "no";
            PatientEntity.Cataract = "no";
            PatientEntity.Diabetic = "no";
            PatientEntity.Hypertension = "no";
            PatientEntity.AllergyDrugs = "no";
            PatientEntity.CurrentMedications = "no";
            PatientEntity.Info = string.Empty;
            PatientEntity.EmergContactNm = string.Empty;
            PatientEntity.AllergyDrugsDtl = string.Empty;
            PatientEntity.StatedConsentPerson = string.Empty;
            PatientEntity.Relation = string.Empty;
            PatientEntity.TermsCondition = string.Empty;
        }
    }
}
