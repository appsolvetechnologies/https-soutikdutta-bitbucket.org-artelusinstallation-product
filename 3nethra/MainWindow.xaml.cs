using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace Artelus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private TextBlock textBlock;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

        //private void OnTextUpdated(object sender, DataTransferEventArgs e)
        //{
        //    textBlock = (sender as TextBlock);
        //    if (textBlock != null)
        //        if (!string.IsNullOrEmpty(textBlock.Text))
        //        {
        //            StackPanel parent = (StackPanel)textBlock.Parent;
        //            parent.Visibility = Visibility.Visible;
        //            Thread t = new Thread(new ThreadStart(ThreadWorker));
        //            t.Start();
        //        }
        //        else
        //        {
        //            StackPanel parent = (StackPanel)textBlock.Parent;
        //            parent.Visibility = Visibility.Hidden;
        //        }
        //}
        //private void ThreadWorker()
        //{
        //    System.Threading.Thread.Sleep(4000);
        //    textBlock.Dispatcher.BeginInvoke(new Action(() =>
        //    {
        //        //textBlock.Text = string.Empty;
        //        StackPanel parent = (StackPanel)textBlock.Parent;
        //        parent.Visibility = Visibility.Hidden;
        //    }));

        //}
    }
}
