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

        private string _textEnregEffectue;
        public string TextEnregEffectue
        {
            get { return _textEnregEffectue; }
            set
            {
                _textEnregEffectue = value;
                OnPropertyChanged("TextEnregEffectue");
            }
        }

        private bool? _isRbCtrlChecked;
        public bool? IsRbCtrlChecked
        {
            get { return _isRbCtrlChecked; }
            set
            {
                _isRbCtrlChecked = value;
                OnPropertyChanged("IsRbCtrlChecked");
            }
        }

        private bool? _isRbAltChecked;
        public bool? IsRbAltChecked
        {
            get { return _isRbAltChecked; }
            set
            {
                _isRbAltChecked = value;
                OnPropertyChanged("IsRbAltChecked");
            }
        }

        private bool _isAppActive;
        public bool IsAppActive
        {
            get { return _isAppActive; }
            set
            {
                _isAppActive = value;
                OnPropertyChanged("IsAppActive");
            }
        }

        private Dictionary<Key, string> dicKeysTextes = new Dictionary<Key, string>();
        List<HotKey> lstHotKeys = new List<HotKey>();

        public PressePapierWindowVM()
        {
            InitDicKeysTextes();
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

        void OnHotKeyHandler(HotKey hotKey)
        {
            if (hotKey.KeyModifiers.Equals(KeyModifier.Ctrl | KeyModifier.Shift) && IsRbCtrlChecked == true ||
                hotKey.KeyModifiers.Equals(KeyModifier.Alt | KeyModifier.Shift) && IsRbAltChecked == true)
            {
                string contenu = FichierUtils.GetClipBoardText();

                if (contenu != "")
                {
                    foreach (var p in this.GetType().GetProperties())
                    {
                        if (dicKeysTextes[hotKey.Key] == p.Name)
                            p.SetValue(this, contenu);
                    }

                    /*nIcon.Icon = new System.Drawing.Icon("ClipBoardActivity.ico");
                    this.Icon = BitmapFrame.Create(new Uri("ClipBoardActivity.ico", UriKind.Relative));
                    timerNotifCopy.Stop();
                    timerNotifCopy.Start();*/
                }
            }
            else if (hotKey.KeyModifiers.Equals(KeyModifier.Ctrl) && IsRbCtrlChecked == true ||
                        hotKey.KeyModifiers.Equals(KeyModifier.Alt) && IsRbAltChecked == true)
            {
                if (!IsAppActive)
                {
                    //Ctrl, Alt et Shift doivent être relachées pour que les ModifiedKeyStroke fonctionnent
                    //sinon la touche enfoncée lors de l'appel de cette fontion sera ajoutée au KeyModifier
                    while (InputSimulator.IsKeyDown(VirtualKeyCode.CONTROL) || InputSimulator.IsKeyDown(VirtualKeyCode.MENU) || InputSimulator.IsKeyDown(VirtualKeyCode.SHIFT))
                    {
                        Thread.Sleep(100);
                    }
                }

                foreach (var p in this.GetType().GetProperties())
                {
                    if (dicKeysTextes[hotKey.Key] == p.Name)
                        Clipboard.SetDataObject(p.GetValue(this));
                }

                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
            }
        }

        public ICommand EffacerTextesCommand
        {
            get { return new RelayCommand(EffacerTextes); }
        }

        private void EffacerTextes()
        {
            foreach (var p in this.GetType().GetProperties())
            {
                if (dicKeysTextes.Values.Contains(p.Name)) p.SetValue(this, "");
            }
        }

        public ICommand ModifToucheRaccourciCommand
        {
            get { return new RelayCommand(ModifToucheRaccourci); }
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
                lstHotKeys.Add(new HotKey(key, keyModifier, OnHotKeyHandler));
                lstHotKeys.Add(new HotKey(key, keyModifier | KeyModifier.Shift, OnHotKeyHandler));
            }
        }
    }
}
