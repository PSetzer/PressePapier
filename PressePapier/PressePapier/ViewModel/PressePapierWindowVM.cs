using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                OnPropertyChanged(TextTB1);
            }
        }

        private string _textTB2;
        public string TextTB2
        {
            get { return _textTB2; }
            set
            {
                _textTB2 = value;
                OnPropertyChanged(TextTB2);
            }
        }

        private string _textTB3;
        public string TextTB3
        {
            get { return _textTB3; }
            set
            {
                _textTB3 = value;
                OnPropertyChanged(TextTB3);
            }
        }

        private string _textTB4;
        public string TextTB4
        {
            get { return _textTB4; }
            set
            {
                _textTB4 = value;
                OnPropertyChanged(TextTB4);
            }
        }

        private string _textTB5;
        public string TextTB5
        {
            get { return _textTB5; }
            set
            {
                _textTB5 = value;
                OnPropertyChanged(TextTB5);
            }
        }

        private string _textTB6;
        public string TextTB6
        {
            get { return _textTB6; }
            set
            {
                _textTB6 = value;
                OnPropertyChanged(TextTB6);
            }
        }

        private string _textTB7;
        public string TextTB7
        {
            get { return _textTB7; }
            set
            {
                _textTB7 = value;
                OnPropertyChanged(TextTB7);
            }
        }

        private string _textTB8;
        public string TextTB8
        {
            get { return _textTB8; }
            set
            {
                _textTB8 = value;
                OnPropertyChanged(TextTB8);
            }
        }

        private string _textTB9;
        public string TextTB9
        {
            get { return _textTB9; }
            set
            {
                _textTB9 = value;
                OnPropertyChanged(TextTB9);
            }
        }

        private string _textTB10;
        public string TextTB10
        {
            get { return _textTB10; }
            set
            {
                _textTB10 = value;
                OnPropertyChanged(TextTB10);
            }
        }

        private string _textFichierEnCours;
        public string TextFichierEnCours
        {
            get { return _textFichierEnCours; }
            set
            {
                _textFichierEnCours = value;
                OnPropertyChanged(TextFichierEnCours);
            }
        }

        private string _textEnregEffectue;
        public string TextEnregEffectue
        {
            get { return _textEnregEffectue; }
            set
            {
                _textEnregEffectue = value;
                OnPropertyChanged(TextEnregEffectue);
            }
        }

        private Dictionary<Key, string> dicKeysTextes = new Dictionary<Key, string>();

        public PressePapierWindowVM()
        {
            InitDicKeysTextes();
        }

        private void InitDicKeysTextes()
        {
            dicKeysTextes.Add(Key.D1, TextTB1);
            /*dicKeysTextes.Add(Key.D2, _textTB2);
            dicKeysTextes.Add(Key.D3, _textTB3);
            dicKeysTextes.Add(Key.D4, _textTB4);
            dicKeysTextes.Add(Key.D5, _textTB5);
            dicKeysTextes.Add(Key.D6, _textTB6);
            dicKeysTextes.Add(Key.D7, _textTB7);
            dicKeysTextes.Add(Key.D8, _textTB8);
            dicKeysTextes.Add(Key.D9, _textTB9);
            dicKeysTextes.Add(Key.D0, _textTB10);*/
        }

        public ICommand EffacerTextesCommand
        {
            get { return new RelayCommand(EffacerTextes); }
        }

        private void EffacerTextes()
        {
            foreach (var p in this.GetType().GetProperties())
            {
                if (p.Name.StartsWith("TextTB")) p.SetValue(this, "");
            }
        }
    }
}
