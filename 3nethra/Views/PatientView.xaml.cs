﻿using Artelus.Model;
using Artelus.ViewModel;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Artelus.Views
{
    /// <summary>
    /// Interaction logic for PatientView.xaml
    /// </summary>
    public partial class PatientView : UserControl
    {
        private int _errors = 0;
        public PatientView()
        {
            InitializeComponent();
            this.DataContext = new PatientViewModel();
        }

        public PatientView(PatientViewModel viewModel, ModernWindow window)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _errors++;

            else
                _errors--;

            var dataContext = this.DataContext as PatientViewModel;
            dataContext.ErrorCount = _errors;
        }

    }
}
