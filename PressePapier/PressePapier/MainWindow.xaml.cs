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

namespace PressePapier
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region initialisation
        Dictionary<Key, string> dicKeysTB = new Dictionary<Key, string>();
        List<HotKey> lstHotKeys = new List<HotKey>();
        private const int initialLineHeight = 20;
        private const int addLineHeight = 17;
        private const int maxTbHeight = 208;
        private double maxSvHeight = 0;
        private bool isAppActive = true;
        GestionFichier gestionFichier = new GestionFichier();

        System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();
        /* gestion TooTip*/
        public delegate void MouseEventHandler(object sender, System.Windows.Forms.MouseEventArgs e);
        System.Windows.Threading.DispatcherTimer messageTimer = new System.Windows.Threading.DispatcherTimer();
        private bool blnShowTooltip = true;

        System.Windows.Threading.DispatcherTimer timerNotifEnreg = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer timerNotifCopy = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
                InitTextes();
                InitDicKeysTB();
                InitControles();
                GestionChargementFichier(gestionFichier.GetDernierFichierOuvert);
                GestionDisplayTooltip();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void InitTextes()
        {
            lblNotifEnreg.Visibility = Visibility.Hidden;
            txtbFichierEnCours.Text = "";
        }

        private void InitDicKeysTB()
        {
            dicKeysTB.Add(Key.D1, textBox1.Name);
            dicKeysTB.Add(Key.D2, textBox2.Name);
            dicKeysTB.Add(Key.D3, textBox3.Name);
            dicKeysTB.Add(Key.D4, textBox4.Name);
            dicKeysTB.Add(Key.D5, textBox5.Name);
            dicKeysTB.Add(Key.D6, textBox6.Name);
            dicKeysTB.Add(Key.D7, textBox7.Name);
            dicKeysTB.Add(Key.D8, textBox8.Name);
            dicKeysTB.Add(Key.D9, textBox9.Name);
            dicKeysTB.Add(Key.D0, textBox10.Name);
        }

        private void InitControles()
        {
            this.Height = svTextBox.Margin.Top + svTextBox.Height + 30;
            this.Width = stackPanel1.Margin.Left + stackPanel1.Width + 40;
            this.Icon = BitmapFrame.Create(new Uri("ClipBoard.ico", UriKind.Relative));
            maxSvHeight = System.Windows.SystemParameters.PrimaryScreenHeight - svTextBox.Margin.Top - 120;

            rbCtrl.IsChecked = true;
            txtbFichierEnCours.MaxWidth = this.Width - txtbFichierEnCours.Margin.Left - (btnMinimiser.Margin.Right + btnMinimiser.Width);

            nIcon.Icon = new System.Drawing.Icon("ClipBoard.ico");
            nIcon.Click += new EventHandler(nIcon_Click);

            /* gestion TooTip*/
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
            try
            {
                if (hotKey.KeyModifiers.Equals(KeyModifier.Ctrl | KeyModifier.Shift) && rbCtrl.IsChecked == true ||
                    hotKey.KeyModifiers.Equals(KeyModifier.Alt | KeyModifier.Shift) && rbAlt.IsChecked == true)
                {
                    string contenu = GetClipBoardText();

                    if (contenu != "")
                    {
                        foreach (StackPanel sp in grdTextBox.Children.OfType<StackPanel>())
                        {
                            TextBox tb = sp.Children[0] as TextBox;
                            if (dicKeysTB[hotKey.Key] == tb.Name)
                            {
                                tb.Text = contenu;
                                break;
                            }
                        }

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

                    foreach (StackPanel sp in grdTextBox.Children.OfType<StackPanel>())
                    {
                        TextBox tb = sp.Children[0] as TextBox;
                        if (dicKeysTB[hotKey.Key] == tb.Name)
                        {
                            Clipboard.SetDataObject(tb.Text);
                            break;
                        }
                    }

                    InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GetClipBoardText()
        {
            string contenu = "";
            bool blnDone = false;
            int count = 0;

            if (Clipboard.GetDataObject().GetDataPresent(System.Windows.DataFormats.Text))
            {
                //boucle afin de gérer le bug WPF CLIPBRD_E_CANT_OPEN
                while (!blnDone)
                {
                    try
                    {
                        count += 1;
                        contenu = (string)Clipboard.GetData(System.Windows.DataFormats.Text);
                        blnDone = true;
                    }
                    catch (Exception)
                    {
                        if (count > 50)
                        {
                            blnDone = true;
                            MessageBox.Show("Le contenu du Presse Papier n'a pas pu être récupéré", "Erreur");
                        }
                        Thread.Sleep(100);
                    }
                }
            }

            return contenu;
        }
        #endregion

        #region gestion boutons

        #region évènements RadioButtons
        //possibilité de transférer les traitements dans un objet à part
        private void RegisterKeys(object sender, RoutedEventArgs e)
        {
            try
            {
                // touches de collage vers un éditeur de texte
                KeyModifier keyModifier = new KeyModifier();
                if ((RadioButton)sender == rbCtrl) keyModifier = KeyModifier.Ctrl;
                else keyModifier = KeyModifier.Alt;

                foreach (var keyTB in dicKeysTB)
                {
                    lstHotKeys.Add(new HotKey(keyTB.Key, keyModifier, OnHotKeyHandler));
                    lstHotKeys.Add(new HotKey(keyTB.Key, keyModifier | KeyModifier.Shift, OnHotKeyHandler));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void UnregisterKeys(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (HotKey hotKey in lstHotKeys)
                {
                    hotKey.Unregister();
                }
                lstHotKeys.Clear();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region évènements boutons menu
        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button enregBtnPressed = sender as Button;

                if (enregBtnPressed.Equals(btnEnregistrer))
                {
                    GestionEnregFichier(gestionFichier.GetDernierFichierOuvert);
                }
                else if (enregBtnPressed.Equals(btnEnregSous))
                {
                    GestionEnregFichier(gestionFichier.ChoixFichierAEnregistrer);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnCharger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button chargerBtnPressed = sender as Button;

                if (chargerBtnPressed.Equals(btnCharger))
                {
                    GestionChargementFichier(gestionFichier.ChoixFichierACharger);
                }
                else if (chargerBtnPressed.Equals(btnRecharger))
                {
                    GestionChargementFichier(gestionFichier.GetDernierFichierOuvert);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnEffacer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (StackPanel sp in grdTextBox.Children.OfType<StackPanel>())
                {
                    TextBox tb = sp.Children[0] as TextBox;
                    tb.Clear();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region méthodes
        private void GestionChargementFichier(Func<string> GetPathFichier)
        {
            try
            {
                string pathFichier = GetPathFichier();

                if(pathFichier != "")
                {
                    Dictionary<string, string> textes = gestionFichier.ChargementFichier(pathFichier);
                    SetTextes(textes);
                    txtbFichierEnCours.Text = Utils.GetNomFichier(pathFichier);
                    gestionFichier.SetDernierFichierOuvert(pathFichier);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void GestionEnregFichier(Func<string> GetPathFichier)
        {
            try
            {
                string pathFichier = GetPathFichier();

                if (pathFichier != "")
                {
                    Dictionary<string, string> textes = GetTextes();
                    gestionFichier.SauvegardeFichier(textes, pathFichier);
                    txtbFichierEnCours.Text = Utils.GetNomFichier(pathFichier);
                    gestionFichier.SetDernierFichierOuvert(pathFichier);
                    lblNotifEnreg.Visibility = Visibility.Visible;
                    timerNotifEnreg.Stop();
                    timerNotifEnreg.Start();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private Dictionary<string, string> GetTextes()
        {
            Dictionary<string, string> textes = new Dictionary<string, string>();

            try
            {
                foreach (StackPanel sp in grdTextBox.Children.OfType<StackPanel>())
                {
                    TextBox tb = sp.Children[0] as TextBox;
                    textes.Add(tb.Name, tb.Text);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return textes;
        }

        private void SetTextes(Dictionary<string, string> textes)
        {
            try
            {
                if (textes.Count > 0)
                {
                    string texteAInserer;
                    foreach (StackPanel sp in grdTextBox.Children.OfType<StackPanel>())
                    {
                        TextBox tb = sp.Children[0] as TextBox;
                        textes.TryGetValue(tb.Name, out texteAInserer);
                        tb.Text = texteAInserer;
                    }
                }
            }
            catch (Exception)
            {

                throw;
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

        #endregion

        #region gestion taille TextBox
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //hauteur de la textbox et de son stackpanel
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
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region gestion Fenêtre
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
        #endregion

        #region gestion NotifyIcon

        private void nIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
            this.WindowState = WindowState.Normal;
            nIcon.Visible = false;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Hide();
                nIcon.Visible = true;
            }
        }

        #region gestion TooTip

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
        #endregion

        #region gestion Timers
        /* gestion TooTip*/
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
