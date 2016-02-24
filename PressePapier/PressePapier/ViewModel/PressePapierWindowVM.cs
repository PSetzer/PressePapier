using PressePapier.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PressePapier.ViewModel
{
    class PressePapierWindowVM : ObservableObject
    {
        #region bound properties
        private string _textTB1;
        public string TextTB1
        {
            get { return _textTB1; }
            set
            {
                _textTB1 = value;
                OnPropertyChanged("TextTB1");
            }
        }

        private string _textTB2;
        public string TextTB2
        {
            get { return _textTB2; }
            set
            {
                _textTB2 = value;
                OnPropertyChanged("TextTB2");
            }
        }

        private string _textTB3;
        public string TextTB3
        {
            get { return _textTB3; }
            set
            {
                _textTB3 = value;
                OnPropertyChanged("TextTB3");
            }
        }

        private string _textTB4;
        public string TextTB4
        {
            get { return _textTB4; }
            set
            {
                _textTB4 = value;
                OnPropertyChanged("TextTB4");
            }
        }

        private string _textTB5;
        public string TextTB5
        {
            get { return _textTB5; }
            set
            {
                _textTB5 = value;
                OnPropertyChanged("TextTB5");
            }
        }

        private string _textTB6;
        public string TextTB6
        {
            get { return _textTB6; }
            set
            {
                _textTB6 = value;
                OnPropertyChanged("TextTB6");
            }
        }

        private string _textTB7;
        public string TextTB7
        {
            get { return _textTB7; }
            set
            {
                _textTB7 = value;
                OnPropertyChanged("TextTB7");
            }
        }

        private string _textTB8;
        public string TextTB8
        {
            get { return _textTB8; }
            set
            {
                _textTB8 = value;
                OnPropertyChanged("TextTB8");
            }
        }

        private string _textTB9;
        public string TextTB9
        {
            get { return _textTB9; }
            set
            {
                _textTB9 = value;
                OnPropertyChanged("TextTB9");
            }
        }

        private string _textTB10;
        public string TextTB10
        {
            get { return _textTB10; }
            set
            {
                _textTB10 = value;
                OnPropertyChanged("TextTB10");
            }
        }

        private string _textFichierEnCours;
        public string TextFichierEnCours
        {
            get { return _textFichierEnCours; }
            set
            {
                _textFichierEnCours = value;
                OnPropertyChanged("TextFichierEnCours");
            }
        }

        private string _toolTipFichierEnCours;
        public string ToolTipFichierEnCours
        {
            get { return _toolTipFichierEnCours; }
            set
            {
                _toolTipFichierEnCours = value;
                OnPropertyChanged("ToolTipFichierEnCours");
            }
        }

        private Visibility _lblNotifEnregVisibility;
        public Visibility LblNotifEnregVisibility
        {
            get { return _lblNotifEnregVisibility; }
            set
            {
                _lblNotifEnregVisibility = value;
                OnPropertyChanged("LblNotifEnregVisibility");
            }
        }

        private Visibility _appVisibility;
        public Visibility AppVisibility
        {
            get { return _appVisibility; }
            set
            {
                _appVisibility = value;
                OnPropertyChanged("AppVisibility");
            }
        }

        private ImageSource _appIcon;
        public ImageSource AppIcon
        {
            get { return _appIcon; }
            set
            {
                _appIcon = value;
                OnPropertyChanged("AppIcon");
            }
        }

        public ICommand SauvegarderTextesCommand
        {
            get { return new RelayCommand(SauvegarderTextes); }
        }

        public ICommand ChargerTextesCommand
        {
            get { return new RelayCommand(ChargerTextes); }
        }

        public ICommand EffacerTextesCommand
        {
            get { return new RelayCommand(EffacerTextes); }
        }

        public ICommand ModifToucheRaccourciCommand
        {
            get { return new RelayCommand(ModifToucheRaccourci); }
        }
        #endregion bound properties

        #region initialisation
        internal Dictionary<Key, string> dicKeysTextes = new Dictionary<Key, string>();
        internal List<HotKey> lstHotKeys = new List<HotKey>();
        internal readonly NotifyIcon _nIcon;
        internal bool blnShowTooltip = true;
        internal FichierServices fichierServices = new FichierServices();
        internal ConfigServices configServices = new ConfigServices();
        internal KeyServices keyServices = new KeyServices();

        public PressePapierWindowVM(NotifyIcon nIcon)
        {
            _nIcon = nIcon;
            InitProperties();
            InitDicKeysTextes();
            ModifToucheRaccourci("rbCtrl");
            ChargerTextes("appStart");
            GestionDisplayTooltip();
        }

        private void InitProperties()
        {
            foreach (var p in this.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)))
                p.SetValue(this, "");

            LblNotifEnregVisibility = Visibility.Hidden;
            AppVisibility = Visibility.Hidden;
            AppIcon = BitmapFrame.Create(new Uri("ClipBoard.ico", UriKind.Relative));
            _nIcon.Icon = new System.Drawing.Icon("ClipBoard.ico");
        }

        private void InitDicKeysTextes()
        {
            dicKeysTextes.Add(Key.D1, "TextTB1");
            dicKeysTextes.Add(Key.D2, "TextTB2");
            dicKeysTextes.Add(Key.D3, "TextTB3");
            dicKeysTextes.Add(Key.D4, "TextTB4");
            dicKeysTextes.Add(Key.D5, "TextTB5");
            dicKeysTextes.Add(Key.D6, "TextTB6");
            dicKeysTextes.Add(Key.D7, "TextTB7");
            dicKeysTextes.Add(Key.D8, "TextTB8");
            dicKeysTextes.Add(Key.D9, "TextTB9");
            dicKeysTextes.Add(Key.D0, "TextTB10");
        }
        #endregion initialisation

        #region gestion keys
        private void OnHotKeyHandlerCopy(HotKey hotKey)
        {
            string pName = dicKeysTextes[hotKey.Key];
            string text = (string)this.GetType().GetProperties().Single(p => p.Name == pName).GetValue(this);
            keyServices.CopyTextToEditor(AppVisibility, text);
        }
        
        private void OnHotKeyHandlerStore(HotKey hotKey)
        {
            string contenu = keyServices.GetTextToStore(AppVisibility);
            if (contenu != "")
            {
                string pName = dicKeysTextes[hotKey.Key];
                this.GetType().GetProperties().Single(p => p.Name == pName).SetValue(this, contenu);
                NotifCopy();
            }
        }

        public void ModifToucheRaccourci(string buttonName)
        {
            foreach (HotKey hotKey in lstHotKeys)
                hotKey.Unregister();
            lstHotKeys.Clear();

            KeyModifier keyModifier;
            if (buttonName == "rbCtrl") keyModifier = KeyModifier.Ctrl;
            else keyModifier = KeyModifier.Alt;

            foreach (var key in dicKeysTextes.Keys)
            {
                lstHotKeys.Add(new HotKey(key, keyModifier, OnHotKeyHandlerCopy));
                lstHotKeys.Add(new HotKey(key, keyModifier | KeyModifier.Shift, OnHotKeyHandlerStore));
            }
        }
        #endregion gestion keys

        #region gestion textes
        internal void SauvegarderTextes(string buttonName)
        {
            string dernierFichierOuvert = configServices.GetDernierFichierOuvert();
            string pathFichier;

            if (buttonName == "btnEnregistrer" && dernierFichierOuvert != "")
                pathFichier = dernierFichierOuvert;
            else
                pathFichier = FichierUtils.ChoixFichierAEnregistrer();

            fichierServices.SauvegardeFichier(GetTextes(), pathFichier);
            SetFichierEnCours(pathFichier);
            NotifEnreg();
        }

        internal void ChargerTextes(string buttonName)
        {
            string dernierFichierOuvert = configServices.GetDernierFichierOuvert();
            string pathFichier;

            if ((buttonName == "btnRecharger" && dernierFichierOuvert != "") || buttonName == "appStart")
                pathFichier = dernierFichierOuvert;
            else
                pathFichier = FichierUtils.ChoixFichierACharger();

            SetTextes(fichierServices.ChargementFichier(pathFichier));
            SetFichierEnCours(pathFichier);
        }

        internal void SetFichierEnCours(string pathFichier)
        {
            if (pathFichier != "")
            {
                TextFichierEnCours = TextUtils.GetNomFichier(pathFichier);
                ToolTipFichierEnCours = TextFichierEnCours;
                configServices.SetDernierFichierOuvert(pathFichier);
            }
        }

        internal Dictionary<string, string> GetTextes()
        {
            var textes = new Dictionary<string, string>();

            foreach (string tb in dicKeysTextes.Values)
            {
                string text = (string)this.GetType().GetProperties().Single(p => p.Name == tb).GetValue(this);
                textes.Add(tb, text ?? "");
            }

            return textes;
        }

        internal void SetTextes(Dictionary<string, string> textes)
        {
            if (textes.Count > 0)
            {
                string texteAInserer;
                foreach (string tb in dicKeysTextes.Values)
                {
                    textes.TryGetValue(tb, out texteAInserer);
                    this.GetType().GetProperties().Single(p => p.Name == tb).SetValue(this, texteAInserer ?? "");
                }
            }
        }

        internal void EffacerTextes()
        {
            foreach (var p in this.GetType().GetProperties().Where(p => dicKeysTextes.Values.Contains(p.Name)))
                p.SetValue(this, "");
        }

        public void GestionDisplayTooltip()
        {
            if (blnShowTooltip)
            {
                string tooltipText = TextUtils.GetTooltipText(TextFichierEnCours, GetTextes());
                _nIcon.ShowBalloonTip(5000, "PressePapier", tooltipText, ToolTipIcon.None);
                DelayShowTooltip();
            }
        }
        #endregion gestion textes

        #region traitements éphémères
        private async Task NotifCopy()
        {
            _nIcon.Icon = new System.Drawing.Icon("ClipBoardActivity.ico");
            AppIcon = BitmapFrame.Create(new Uri("ClipBoardActivity.ico", UriKind.Relative));
            await Task.Delay(4000);
            _nIcon.Icon = new System.Drawing.Icon("ClipBoard.ico");
            AppIcon = BitmapFrame.Create(new Uri("ClipBoard.ico", UriKind.Relative));
        }

        private async Task NotifEnreg()
        {
            LblNotifEnregVisibility = Visibility.Visible;
            await Task.Delay(4000);
            LblNotifEnregVisibility = Visibility.Hidden;
        }

        private async Task DelayShowTooltip()
        {
            blnShowTooltip = false;
            await Task.Delay(4000);
            blnShowTooltip = true;
        }
        #endregion traitements éphémères
    }
}
