using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace PressePapier
{
    public static class FichierUtils
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

        public static string ChoixFichierAEnregistrer()
        {
            string pathFichier = "";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Fichiers XML|*.xml|Tous les fichiers|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.DefaultExt = ".xml";

            if (saveFileDialog1.ShowDialog() == true)
            {
                pathFichier = saveFileDialog1.FileName;
            }

            return pathFichier;
        }

        public static string ChoixFichierACharger()
        {
            string pathFichier = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // On interdit la sélection de plusieurs fichiers.
            openFileDialog1.Multiselect = false;

            // On supprime le nom de fichier, qui ici vaut "openFileDialog1" (avant sélection d'un fichier).
            openFileDialog1.FileName = string.Empty;

            // On met des filtres pour les types de fichiers : "Nom|*.extension|autreNom|*.autreExtension" (autant de filtres qu'on veut).
            openFileDialog1.Filter = "Fichiers XML|*.xml|Tous les fichiers|*.*";

            // Le filtre sélectionné : le 1er (là on ne commence pas à compter à 0).
            openFileDialog1.FilterIndex = 1;

            // On affiche le dernier dossier ouvert.
            openFileDialog1.RestoreDirectory = true;

            // Si l'utilisateur clique sur "Ouvrir"...
            if (openFileDialog1.ShowDialog() == true)
            {
                pathFichier = openFileDialog1.FileName;
            }

            return pathFichier;
        }
    }
}
