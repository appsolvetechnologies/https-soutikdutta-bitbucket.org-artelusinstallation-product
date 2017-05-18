﻿using Artelus.ViewModel;
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
    /// Interaction logic for PatientProfileView.xaml
    /// </summary>
    public partial class PatientProfileView : UserControl
    {
        public PatientProfileView()
        {
            InitializeComponent();
        }
        public PatientProfileView(ProfileViewModel viewModel, ModernWindow window)
        {
            this.DataContext = viewModel;
            InitializeComponent();
            //viewModel.CloseAction = () =>
            //{
            //    window.DialogResult = true;
            //    window.Close();
            //};
        }
    }
}
