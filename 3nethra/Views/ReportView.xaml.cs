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
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl
    {
        public ReportView()
        {
            InitializeComponent();
        }

        public ReportView(ReportViewModel viewModel, ModernWindow window)
        {
            this.DataContext = viewModel;
            InitializeComponent();
            viewModel.CloseAction = () =>
            {
                window.DialogResult = true;
                window.Close();
            };
        }
    }
}
