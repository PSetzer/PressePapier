using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;

namespace PressePapier.Model
{
    class KeyServices
    {
        public void TextCopy(Visibility appVisibility, string text)
        {
            WaitForKeyRelease(appVisibility);

            System.Windows.Clipboard.SetDataObject(text);
            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
        }

        public string TextStore(Visibility appVisibility)
        {
            WaitForKeyRelease(appVisibility);

            //les touches de raccourci de stockage doivent être maintenues un moment pour que ce mécanisme marche
            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
            return GetClipBoardText();
        }

        private void WaitForKeyRelease(Visibility appVisibility)
        {
            if (appVisibility != Visibility.Visible)
            {
                //Ctrl, Alt et Shift doivent être relachées pour que les ModifiedKeyStroke fonctionnent, sinon la touche enfoncée lors de l'appel de cette fontion sera ajoutée au KeyModifier
                while (InputSimulator.IsKeyDown(VirtualKeyCode.CONTROL) || InputSimulator.IsKeyDown(VirtualKeyCode.MENU) || InputSimulator.IsKeyDown(VirtualKeyCode.SHIFT))
                {
                    Thread.Sleep(100);
                }
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
                    catch (Exception ex)
                    {
                        if (count > 50)
                        {
                            blnDone = true;
                            throw new Exception("Le contenu du Presse Papier n'a pas pu être récupéré :\n", ex);
                        }
                        Thread.Sleep(100);
                    }
                }
            }

            return contenu;
        }
    }
}
