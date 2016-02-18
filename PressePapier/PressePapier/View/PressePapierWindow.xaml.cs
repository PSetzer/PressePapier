﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Threading;
using WindowsInput;
using Microsoft.Win32;
using PressePapier.Model;

namespace PressePapier.View
{
    /// <summary>
    /// Logique d'interaction pour PressePapierWindow.xaml
    /// </summary>
    public partial class PressePapierWindow : Window
    {
        #region initialisation
        Dictionary<Key, TextBox> dicKeysTB = new Dictionary<Key, TextBox>();
        List<HotKey> lstHotKeys = new List<HotKey>();
        private const int initialLineHeight = 20;
        private const int addLineHeight = 17;
        private const int maxTbHeight = 208;
        private double maxSvHeight = 0;
        private bool isAppActive = true;
        FichierServices fichierServices = new FichierServices();
        ConfigServices configServices = new ConfigServices();

        System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();

        public delegate void MouseEventHandler(object sender, System.Windows.Forms.MouseEventArgs e);
        System.Windows.Threading.DispatcherTimer messageTimer = new System.Windows.Threading.DispatcherTimer();
        private bool blnShowTooltip = true;

        System.Windows.Threading.DispatcherTimer timerNotifEnreg = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer timerNotifCopy = new System.Windows.Threading.DispatcherTimer();

        public PressePapierWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            //InitTextes();
            InitDicKeysTB();
            InitControles();
            //GestionChargementFichier(configServices.GetDernierFichierOuvert());
            GestionDisplayTooltip();            
        }

        private void InitTextes()
        {
            lblNotifEnreg.Visibility = Visibility.Hidden;
            txtbFichierEnCours.Text = "";
        }

        private void InitDicKeysTB()
        {
            dicKeysTB.Add(Key.D1, textBox1);
            dicKeysTB.Add(Key.D2, textBox2);
            dicKeysTB.Add(Key.D3, textBox3);
            dicKeysTB.Add(Key.D4, textBox4);
            dicKeysTB.Add(Key.D5, textBox5);
            dicKeysTB.Add(Key.D6, textBox6);
            dicKeysTB.Add(Key.D7, textBox7);
            dicKeysTB.Add(Key.D8, textBox8);
            dicKeysTB.Add(Key.D9, textBox9);
            dicKeysTB.Add(Key.D0, textBox10);
        }

        private void InitControles()
        {
            this.Height = svTextBox.Margin.Top + svTextBox.Height + 30;
            this.Width = stackPanel1.Margin.Left + stackPanel1.Width + 40;
            this.Icon = BitmapFrame.Create(new Uri("ClipBoard.ico", UriKind.Relative));
            maxSvHeight = System.Windows.SystemParameters.PrimaryScreenHeight - svTextBox.Margin.Top - 120;

            //rbCtrl.IsChecked = true;
            txtbFichierEnCours.MaxWidth = this.Width - txtbFichierEnCours.Margin.Left - (btnMinimiser.Margin.Right + btnMinimiser.Width);

            nIcon.Icon = new System.Drawing.Icon("ClipBoard.ico");
            nIcon.Click += new EventHandler(nIcon_Click);

            nIcon.BalloonTipClicked += new EventHandler(nIcon_Click);
            nIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(nIcon_MouseMove);

            messageTimer.Tick += new EventHandler(messageTimer_Tick);
            messageTimer.Interval = new TimeSpan(0, 0, 0, 4, 0);

            timerNotifEnreg.Tick += new EventHandler(timerNotifEnreg_Tick);
            timerNotifEnreg.Interval = new TimeSpan(0, 0, 0, 4, 0);
            timerNotifCopy.Tick += new EventHandler(timerNotifCopy_Tick);
            timerNotifCopy.Interval = new TimeSpan(0, 0, 0, 1, 0);
        }

        #endregion

        #region actions touches de raccourci
        void OnHotKeyHandler(HotKey hotKey)
        {
            if (hotKey.KeyModifiers.Equals(KeyModifier.Ctrl | KeyModifier.Shift) && rbCtrl.IsChecked == true ||
                hotKey.KeyModifiers.Equals(KeyModifier.Alt | KeyModifier.Shift) && rbAlt.IsChecked == true)
            {
                string contenu = FichierUtils.GetClipBoardText();

                if (contenu != "")
                {
                    dicKeysTB[hotKey.Key].Text = contenu;

                    nIcon.Icon = new System.Drawing.Icon("ClipBoardActivity.ico");
                    this.Icon = BitmapFrame.Create(new Uri("ClipBoardActivity.ico", UriKind.Relative));
                    timerNotifCopy.Stop();
                    timerNotifCopy.Start();
                }
            }
            else if (hotKey.KeyModifiers.Equals(KeyModifier.Ctrl) && rbCtrl.IsChecked == true ||
                        hotKey.KeyModifiers.Equals(KeyModifier.Alt) && rbAlt.IsChecked == true)
            {
                if (!isAppActive)
                {
                    //Ctrl, Alt et Shift doivent être relachées pour que les ModifiedKeyStroke fonctionnent
                    //sinon la touche enfoncée lors de l'appel de cette fontion sera ajoutée au KeyModifier
                    while (InputSimulator.IsKeyDown(VirtualKeyCode.CONTROL) || InputSimulator.IsKeyDown(VirtualKeyCode.MENU) || InputSimulator.IsKeyDown(VirtualKeyCode.SHIFT))
                    {
                        Thread.Sleep(100);
                    }
                }

                Clipboard.SetDataObject(dicKeysTB[hotKey.Key].Text);

                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
            }
        }
        #endregion

        #region évènements RadioButtons
        private void RegisterKeys(object sender, RoutedEventArgs e)
        {
            // touches de collage vers un éditeur de texte
            KeyModifier keyModifier;
            if ((RadioButton)sender == rbCtrl) keyModifier = KeyModifier.Ctrl;
            else keyModifier = KeyModifier.Alt;

            foreach (var keyTB in dicKeysTB)
            {
                lstHotKeys.Add(new HotKey(keyTB.Key, keyModifier, OnHotKeyHandler));
                lstHotKeys.Add(new HotKey(keyTB.Key, keyModifier | KeyModifier.Shift, OnHotKeyHandler));
            }
        }

        private void UnregisterKeys(object sender, RoutedEventArgs e)
        {
            foreach (HotKey hotKey in lstHotKeys)
            {
                hotKey.Unregister();
            }
            lstHotKeys.Clear();
        }
        #endregion

        #region évènements et méthodes boutons menu
        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            string dernierFichierOuvert = configServices.GetDernierFichierOuvert();

            if ((Button)sender == btnEnregistrer && dernierFichierOuvert != "")
                GestionEnregFichier(dernierFichierOuvert);
            else
                GestionEnregFichier(FichierUtils.ChoixFichierAEnregistrer());
        }

        private void btnCharger_Click(object sender, RoutedEventArgs e)
        {
            string dernierFichierOuvert = configServices.GetDernierFichierOuvert();

            if ((Button)sender == btnRecharger && dernierFichierOuvert != "")
                GestionChargementFichier(dernierFichierOuvert);
            else
                GestionChargementFichier(FichierUtils.ChoixFichierACharger());
        }

        private void btnEffacer_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox tb in dicKeysTB.Values) tb.Clear();
        }

        private void GestionEnregFichier(string pathFichier)
        {
            if (pathFichier != "")
            {
                fichierServices.SauvegardeFichier(GetTextes(), pathFichier);

                txtbFichierEnCours.Text = FichierUtils.GetNomFichier(pathFichier);
                configServices.SetDernierFichierOuvert(pathFichier);

                lblNotifEnreg.Visibility = Visibility.Visible;
                timerNotifEnreg.Stop();
                timerNotifEnreg.Start();
            }
        }

        private void GestionChargementFichier(string pathFichier)
        {
            if (pathFichier != "")
            {
                SetTextes(fichierServices.ChargementFichier(pathFichier));

                txtbFichierEnCours.Text = FichierUtils.GetNomFichier(pathFichier);
                configServices.SetDernierFichierOuvert(pathFichier);
            }
        }

        private Dictionary<string, string> GetTextes()
        {
            var textes = new Dictionary<string, string>();

            foreach (TextBox tb in dicKeysTB.Values)
            {
                textes.Add(tb.Name, tb.Text);
            }

            return textes;
        }

        private void SetTextes(Dictionary<string, string> textes)
        {
            if (textes.Count > 0)
            {
                string texteAInserer;
                foreach (TextBox tb in dicKeysTB.Values)
                {
                    textes.TryGetValue(tb.Name, out texteAInserer);
                    tb.Text = texteAInserer;
                }
            }
        }

        private void txtbFichierEnCours_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            txtbFichierEnCours.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            if (txtbFichierEnCours.DesiredSize.Width > txtbFichierEnCours.MaxWidth)
            {
                txtbFichierEnCours.ToolTip = txtbFichierEnCours.Text;
            }
            else e.Handled = true;
        }
        #endregion

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
        #endregion

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

        private void Window_Activated(object sender, EventArgs e)
        {
            isAppActive = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            isAppActive = false;
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
        #endregion

        #region gestion NotifyIcon et ToolTip
        private void nIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
            this.WindowState = WindowState.Normal;
            nIcon.Visible = false;
        }

        private void nIcon_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (blnShowTooltip)
            {
                GestionDisplayTooltip();
            }
        }

        private void GestionDisplayTooltip()
        {
            Dictionary<string, string> textes = GetTextes();
            string tooltipText = "";
            bool aucunTexte = true;

            if (txtbFichierEnCours.Text != "")
            {
                tooltipText = txtbFichierEnCours.Text + "\n";
                if (tooltipText.Length > 28)
                {
                    tooltipText = tooltipText.Substring(0, 25) + "...\n";
                }
            }

            foreach (var text in textes)
            {
                string numTextbox = text.Key.Substring(7);
                string premiereLigne = text.Value.Split('\n')[0];
                if (premiereLigne != "")
                {
                    aucunTexte = false;
                    if (premiereLigne.Length > 24)
                    {
                        premiereLigne = premiereLigne.Substring(0, 21) + "...";
                    }
                    tooltipText += numTextbox + " : " + premiereLigne + "\n";
                }
            }

            if (aucunTexte)
            {
                tooltipText += "Aucun texte";
            }

            nIcon.ShowBalloonTip(5000, this.Title, tooltipText, System.Windows.Forms.ToolTipIcon.None);
            blnShowTooltip = false;
            messageTimer.Start();
        }
        #endregion

        #region évènements Timers
        private void messageTimer_Tick(object sender, EventArgs e)
        {
            blnShowTooltip = true;
            messageTimer.Stop();
        }

        private void timerNotifEnreg_Tick(object sender, EventArgs e)
        {
            lblNotifEnreg.Visibility = Visibility.Hidden;
            timerNotifEnreg.Stop();
        }

        private void timerNotifCopy_Tick(object sender, EventArgs e)
        {
            nIcon.Icon = new System.Drawing.Icon("ClipBoard.ico");
            this.Icon = BitmapFrame.Create(new Uri("ClipBoard.ico", UriKind.Relative));
            timerNotifCopy.Stop();
        }
        #endregion
    }
}
