using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressePapier.Model
{
    public static class TextUtils
    {
        public static string GetNomFichier(string pathFichier)
        {
            string nomFichier = "";

            try
            {
                if (pathFichier != "")
                {
                    string nomFichierAvecExt = pathFichier.Split('\\').Last();
                    if (nomFichierAvecExt.Split('.').Length > 1)
                    {
                        string extension = nomFichierAvecExt.Split('.').Last();
                        var elemsNomFichier = nomFichierAvecExt.Split('.').TakeWhile(item => item != extension);
                        nomFichier = elemsNomFichier.Aggregate((item, next) => item + "." + next);
                    }
                    else
                    {
                        nomFichier = nomFichierAvecExt;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Le nom du fichier n'a pas pu être récupéré :\n", ex);
            }

            return nomFichier;
        }

        public static string GetTooltipText(string textFichierEnCours, Dictionary<string, string> textes)
        {
            string tooltipText = "";
            bool aucunTexte = true;

            if (textFichierEnCours != "")
            {
                tooltipText = textFichierEnCours + "\n";
                if (tooltipText.Length > 28)
                {
                    tooltipText = tooltipText.Substring(0, 25) + "...\n";
                }
            }

            foreach (var text in textes)
            {
                string numTextbox = text.Key.Substring(6);
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

            return tooltipText;
        }
    }
}
