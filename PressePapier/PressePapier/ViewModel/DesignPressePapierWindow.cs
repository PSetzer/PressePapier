using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PressePapier.ViewModel
{
    class DesignPressePapierWindow
    {
        public DesignPressePapierWindow()
        {
            _textTB1 = "texte 1";
            _textTB2 = "texte 2";
            _textTB3 = "texte 3";
            _textTB4 = "texte 4";
            _textTB5 = "texte 5";
            _textTB6 = "texte 6";
            _textTB7 = "texte 7";
            _textTB8 = "texte 8";
            _textTB9 = "texte 9";
            _textTB10 = "texte 10";
            _textFichierEnCours = "fichier en cours";
            _toolTipFichierEnCours = _textFichierEnCours;
            _lblNotifEnregVisibility = Visibility.Visible;
            _appVisibility = Visibility.Visible;
        }
        
        private string _textTB1;
        public string TextTB1
        {
            get { return _textTB1; }
            set{}
        }

        private string _textTB2;
        public string TextTB2
        {
            get { return _textTB2; }
            set{}
        }

        private string _textTB3;
        public string TextTB3
        {
            get { return _textTB3; }
            set{}
        }

        private string _textTB4;
        public string TextTB4
        {
            get { return _textTB4; }
            set{}
        }

        private string _textTB5;
        public string TextTB5
        {
            get { return _textTB5; }
            set{}
        }

        private string _textTB6;
        public string TextTB6
        {
            get { return _textTB6; }
            set{}
        }

        private string _textTB7;
        public string TextTB7
        {
            get { return _textTB7; }
            set{}
        }

        private string _textTB8;
        public string TextTB8
        {
            get { return _textTB8; }
            set{}
        }

        private string _textTB9;
        public string TextTB9
        {
            get { return _textTB9; }
            set{}
        }

        private string _textTB10;
        public string TextTB10
        {
            get { return _textTB10; }
            set{}
        }

        private string _textFichierEnCours;
        public string TextFichierEnCours
        {
            get { return _textFichierEnCours; }
            set{}
        }

        private string _toolTipFichierEnCours;
        public string ToolTipFichierEnCours
        {
            get { return _toolTipFichierEnCours; }
            set{}
        }

        private Visibility _lblNotifEnregVisibility;
        public Visibility LblNotifEnregVisibility
        {
            get { return _lblNotifEnregVisibility; }
            set {}
        }

        private Visibility _appVisibility;
        public Visibility AppVisibility
        {
            get { return _appVisibility; }
            set{}
        }

        private ImageSource _appIcon;
        public ImageSource AppIcon
        {
            get { return _appIcon; }
            set{}
        }
    }
}
