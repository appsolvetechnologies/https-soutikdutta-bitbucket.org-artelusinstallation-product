﻿using Artelus.Model;
using Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artelus.ViewModel
{
    public class ReportHistoryViewModel : BaseViewModel
    {

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
        private PatientReport patientReport;
        public PatientReport PatientReport
        {
            get { return patientReport; }
            set
            {
                patientReport = value;
                RaisePropertyChange("PatientReport");
            }
        }
        private ObservableCollection<PatientReport> patientReports = new ObservableCollection<PatientReport>();
        public ObservableCollection<PatientReport> PatientReports
        {
            get { return patientReports; }
            set
            {
                if (value != patientReports)
                {
                    patientReports = value;
                    RaisePropertyChange("PatientReports");
                }
            }
        }

        public DelegateCommand ViewReportDataCommand { get; set; }

        public ReportHistoryViewModel(PatientEntity model)
        {
            PatientEntity = model;

            if (PatientEntity.MaritalStatus == "no")
                PatientEntity.MaritalStatus = "Single";
            else
                PatientEntity.MaritalStatus = "Married";

            if (PatientEntity.Sex == "m")
                PatientEntity.Sex = "Male";
            else
                PatientEntity.Sex = "Female";

            if (PatientEntity.AllergyDrugs == "yes")
                ShowAllergyOption = true;

            if (PatientEntity.IfResidentOfM == "yes")
            {
                PatientEntity.OtherOption = "IC Number:";
                PatientEntity.OthersID = PatientEntity.IcNumber;
            }
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(rootPath, "Uploads");
            var report = new Patient().GetAllReport(model.Id);
            foreach (var item in report)
            {
                var result = new Patient().GetAllReportData(item.Id);
                item.ReportDatas = new ObservableCollection<ReportData>();
                foreach (var data in result)
                {
                    data.ImageUrl = Path.Combine(path, data.Img);
                    item.ReportDatas.Add(data);
                }
                PatientReports.Add(item);
            }
            ViewReportDataCommand = new DelegateCommand(OnViewReportDataCommand);
        }

        private void OnViewReportDataCommand(object args)
        {

        }
        public Action CloseAction { get; set; }
    }
}
