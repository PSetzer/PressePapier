using PressePapier.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WindowsInput;

namespace PressePapier.ViewModel
{
    class PressePapierWindowVM : ObservableObject
    {
        #region properties
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
        #endregion properties

        private Dictionary<Key, string> dicKeysTextes = new Dictionary<Key, string>();
        List<HotKey> lstHotKeys = new List<HotKey>();
        FichierServices fichierServices = new FichierServices();
        ConfigServices configServices = new ConfigServices();

        public PressePapierWindowVM()
        {
            InitProperties();
            InitDicKeysTextes();
            ModifToucheRaccourci("rbCtrl");
            ChargerTextes("btnRecharger");
        }

        private void InitProperties()
        {
            foreach (var p in this.GetType().GetProperties().Where(p => p.GetType() == typeof(string)))
                p.SetValue(this, "");

            LblNotifEnregVisibility = Visibility.Hidden;
            AppVisibility = Visibility.Hidden;
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

        private void OnHotKeyHandlerCopy(HotKey hotKey)
        {
            if (AppVisibility != Visibility.Visible)
            {
                //Ctrl, Alt et Shift doivent être relachées pour que les ModifiedKeyStroke fonctionnent, sinon la touche enfoncée lors de l'appel de cette fontion sera ajoutée au KeyModifier
                while (InputSimulator.IsKeyDown(VirtualKeyCode.CONTROL) || InputSimulator.IsKeyDown(VirtualKeyCode.MENU) || InputSimulator.IsKeyDown(VirtualKeyCode.SHIFT))
                {
                    Thread.Sleep(100);
                }
            }

            string pName = dicKeysTextes[hotKey.Key];
            string text = (string)this.GetType().GetProperties().Single(p => p.Name == pName).GetValue(this);
            Clipboard.SetDataObject(text);
            
            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
        }
        
        private void OnHotKeyHandlerStore(HotKey hotKey)
        {
            if (AppVisibility != Visibility.Visible)
            {
                //Ctrl, Alt et Shift doivent être relachées pour que les ModifiedKeyStroke fonctionnent, sinon la touche enfoncée lors de l'appel de cette fontion sera ajoutée au KeyModifier
                while (InputSimulator.IsKeyDown(VirtualKeyCode.CONTROL) || InputSimulator.IsKeyDown(VirtualKeyCode.MENU) || InputSimulator.IsKeyDown(VirtualKeyCode.SHIFT))
                {
                    Thread.Sleep(100);
                }
            }

            //les touches de raccourci de stockage doivent être maintenues un moment pour que ce mécanisme marche
            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
            
            string contenu = FichierUtils.GetClipBoardText();

            if (contenu != "")
            {
                string pName = dicKeysTextes[hotKey.Key];
                this.GetType().GetProperties().Single(p => p.Name == pName).SetValue(this, contenu);

                /*nIcon.Icon = new System.Drawing.Icon("ClipBoardActivity.ico");
                this.Icon = BitmapFrame.Create(new Uri("ClipBoardActivity.ico", UriKind.Relative));
                timerNotifCopy.Stop();
                timerNotifCopy.Start();*/
            }
        }

        private void SauvegarderTextes(string buttonName)
        {
            string dernierFichierOuvert = configServices.GetDernierFichierOuvert();
            string pathFichier = "";

            if (buttonName == "btnEnregistrer" && dernierFichierOuvert != "")
                pathFichier = dernierFichierOuvert;
            else
                pathFichier = FichierUtils.ChoixFichierAEnregistrer();

            if (pathFichier != "")
            {
                fichierServices.SauvegardeFichier(GetTextes(), pathFichier);

                TextFichierEnCours = FichierUtils.GetNomFichier(pathFichier);
                configServices.SetDernierFichierOuvert(pathFichier);

                /*LblNotifEnregVisibility = Visibility.Visible;
                timerNotifEnreg.Stop();
                timerNotifEnreg.Start();*/
            }
        }

        private void ChargerTextes(string buttonName)
        {
            string dernierFichierOuvert = configServices.GetDernierFichierOuvert();
            string pathFichier = "";

            if (buttonName == "btnRecharger" && dernierFichierOuvert != "")
                pathFichier = dernierFichierOuvert;
            else
                pathFichier = FichierUtils.ChoixFichierACharger();

            if (pathFichier != "")
            {
                SetTextes(fichierServices.ChargementFichier(pathFichier));

                TextFichierEnCours = FichierUtils.GetNomFichier(pathFichier);
                configServices.SetDernierFichierOuvert(pathFichier);
            }
        }

        private Dictionary<string, string> GetTextes()
        {
            var textes = new Dictionary<string, string>();

            foreach (string tb in dicKeysTextes.Values)
            {
                string text = (string)this.GetType().GetProperties().Single(p => p.Name == tb).GetValue(this);
                textes.Add(tb, text);
            }

            return textes;
        }

        private void SetTextes(Dictionary<string, string> textes)
        {
            if (textes.Count > 0)
            {
                string texteAInserer;
                foreach (string tb in dicKeysTextes.Values)
                {
                    textes.TryGetValue(tb, out texteAInserer);
                    this.GetType().GetProperties().Single(p => p.Name == tb).SetValue(this, texteAInserer);
                }
            }
        }

        private void EffacerTextes()
        {
            foreach (var p in this.GetType().GetProperties().Where(p => dicKeysTextes.Values.Contains(p.Name)))
                p.SetValue(this, "");
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
    }
}
