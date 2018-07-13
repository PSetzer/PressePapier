using Microsoft.Win32;
using PressePapier.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Linq;

namespace PressePapier.View
{
    /// <summary>
    /// Logique d'interaction pour PressePapierWindow.xaml
    /// </summary>
    public partial class PressePapierWindow : Window
    {
        #region initialisation
        PressePapierWindowVM viewModel;
        NotifyIcon nIcon;

        public PressePapierWindow()
        {
            InitializeComponent();
            nIcon = new NotifyIcon();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {   
            nIcon.Click += new EventHandler(nIcon_Click);
            int topSvTextbox = int.Parse(windowGrid.RowDefinitions[0].Height.ToString()) + int.Parse(windowGrid.RowDefinitions[1].Height.ToString());
            svTextBox.MaxHeight = SystemParameters.PrimaryScreenHeight * 0.9 - topSvTextbox;
            viewModel = new PressePapierWindowVM(nIcon);
            this.DataContext = viewModel;
        }
        #endregion initialisation

        #region évènements Fenêtre
        private void btnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            nIcon.Visible = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Hide();
            nIcon.Visible = true;
        }
        #endregion évènements Fenêtre

        #region évènements NotifyIcon
        private void nIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
            this.WindowState = WindowState.Normal;
            nIcon.Visible = false;
        }
        #endregion évènements NotifyIcon
    }
}
