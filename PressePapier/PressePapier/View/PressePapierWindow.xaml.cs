using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using PressePapier.ViewModel;
using System.Windows.Forms;

namespace PressePapier.View
{
    /// <summary>
    /// Logique d'interaction pour PressePapierWindow.xaml
    /// </summary>
    public partial class PressePapierWindow : Window
    {
        #region initialisation
        private const int initialLineHeight = 20;
        private const int addLineHeight = 17;
        private const int maxTbHeight = 208;
        private double maxSvHeight = 0;

        PressePapierWindowVM viewModel;
        NotifyIcon nIcon = new NotifyIcon();

        public PressePapierWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitTaillePosition();
            InitIcon();
            viewModel = new PressePapierWindowVM(nIcon);
            this.DataContext = viewModel;
        }
        
        private void InitTaillePosition()
        {
            this.WindowState = WindowState.Minimized;
            this.Height = svTextBox.Margin.Top + svTextBox.Height + 30;
            this.Width = stackPanel1.Margin.Left + stackPanel1.Width + 40;
            maxSvHeight = System.Windows.SystemParameters.PrimaryScreenHeight - svTextBox.Margin.Top - 120;
            txtbFichierEnCours.MaxWidth = this.Width - txtbFichierEnCours.Margin.Left - (btnMinimiser.Margin.Right + btnMinimiser.Width);
        }

        private void InitIcon()
        {
            nIcon.Click += new EventHandler(nIcon_Click);
            nIcon.BalloonTipClicked += new EventHandler(nIcon_Click);
            nIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(nIcon_MouseMove);
        }
        #endregion initialisation

        #region gestion taille et position éléments
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*//hauteur de la textbox et de son stackpanel
            TextBox tbChanged = sender as TextBox;
            StackPanel spChanged = tbChanged.Parent as StackPanel;
            int lineHeight = initialLineHeight + ((tbChanged.LineCount - 1) * addLineHeight);
            double initialTbHeight = tbChanged.Height;

            tbChanged.Height = lineHeight;
            if (tbChanged.Height > maxTbHeight)
            {
                tbChanged.Height = maxTbHeight;
            }
            double diffTbHeight = tbChanged.Height - initialTbHeight;

            spChanged.Height = lineHeight;
            if (spChanged.Height > maxTbHeight)
            {
                spChanged.Height = maxTbHeight;
            }

            //hauteur de la grid, du scrollviewer et du form
            double newGrdHeight = 0;
            foreach (StackPanel sp in grdTextBox.Children.OfType<StackPanel>())
            {
                newGrdHeight += sp.Height + 15; //+15 pour l'espace après chaque textbox
            }

            grdTextBox.Height = newGrdHeight - 15; //-15 car pas d'espace après la dernière textbox
            svTextBox.Height = grdTextBox.Height;
            if (svTextBox.Height > maxSvHeight)
            {
                svTextBox.Height = maxSvHeight;
            }

            this.Height = svTextBox.Margin.Top + svTextBox.Height + 30;

            //déplacement des éléments vers le bas
            foreach (StackPanel spMove in grdTextBox.Children.OfType<StackPanel>())
            {
                if (spMove.Margin.Top > spChanged.Margin.Top)
                {
                    spMove.Margin = new Thickness(spMove.Margin.Left, spMove.Margin.Top + diffTbHeight, spMove.Margin.Right, spMove.Margin.Bottom);
                }
            }

            foreach (Label lblMove in grdTextBox.Children.OfType<Label>())
            {
                if (lblMove.Margin.Top > spChanged.Margin.Top)
                {
                    lblMove.Margin = new Thickness(lblMove.Margin.Left, lblMove.Margin.Top + diffTbHeight, lblMove.Margin.Right, lblMove.Margin.Bottom);
                }
            }*/
        }
        #endregion gestion taille et position éléments

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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            border1.Margin = new Thickness(0);
            border1.Width = e.NewSize.Width;
            border1.Height = e.NewSize.Height;
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
    }
}
