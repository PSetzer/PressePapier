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
        NotifyIcon nIcon = new NotifyIcon();

        public PressePapierWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitIcon();
            svTextBox.MaxHeight = SystemParameters.PrimaryScreenHeight * 0.9 - svTextBox.Margin.Top;
            viewModel = new PressePapierWindowVM(nIcon);
            this.DataContext = viewModel;
        }

        private void InitIcon()
        {
            nIcon.Click += new EventHandler(nIcon_Click);
            nIcon.BalloonTipClicked += new EventHandler(nIcon_Click);
            nIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(nIcon_MouseMove);
        }
        #endregion initialisation

        #region évènements Fenêtre
        private void btnMinimiser_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

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

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Hide();
                nIcon.Visible = true;
            }
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

        private void nIcon_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            viewModel.GestionDisplayTooltip();
        }
        #endregion évènements NotifyIcon

        private void svTextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //this.SizeToContent = System.Windows.SizeToContent.Height;
        }

        private void windowGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Height = windowGrid.Height;
        }
    }
}
