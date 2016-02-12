using System;
using System.Windows;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;

namespace PressePapier
{
	class GestionFichier
	{
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        string pathDossierConfig;
        string pathFichierConfig;

        public GestionFichier()
        {
            pathDossierConfig = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PressePapier";
            pathFichierConfig = pathDossierConfig + "\\config.xml";
        }

    #region enregistrement
        public string ChoixFichierAEnregistrer()
        {
            string pathFichier = "";

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

        public void SauvegardeFichier(Dictionary<string, string> textes, string pathFichier)
        {
            try
            {
                if (pathFichier != "")
                {
                    XDocument document = new XDocument(
                                                new XElement("Racine",
                                                    new XElement("Textes")));

                    foreach (var text in textes)
                    {
                        document.Element("Racine").Element("Textes").Add(new XElement(text.Key, text.Value));
                    }

                    document.Save(pathFichier);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Le fichier n'a pas pu être sauvegardé :\n", ex);
            }
        }
    #endregion

    #region chargement
        public string ChoixFichierACharger()
        {
            string pathFichier = "";

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

        public Dictionary<string, string> ChargementFichier(string pathFichier)
        {
            Dictionary<string, string> textes = new Dictionary<string, string>();

            try
            {
                if (pathFichier != "")
                {
                    XDocument sauvegardeXML = XDocument.Load(pathFichier);

                    var requete = from d in sauvegardeXML.Root.Descendants("Textes")
                                  select d.Descendants();

                    foreach (var node in requete)
                    {
                        foreach (XElement element in node)
                        {
                            textes.Add(element.Name.ToString(), element.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Le fichier n'a pas pu être chargé :\n", ex);
            }

            return textes;
        }
    #endregion

        private void CreateFichierConfigIfNotExists()
        {
            try
            {                
                if(!Directory.Exists(pathDossierConfig))
                {
                    Directory.CreateDirectory(pathDossierConfig);
                }

                if (!File.Exists(pathFichierConfig))
                {
                    XDocument fichierConfig = new XDocument(
                                                new XElement("Racine",
                                                    new XElement("DernierFichier")));

                    fichierConfig.Save(pathFichierConfig);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Le fichier de configuration n'a pas pu être créé :\n", ex);
            }
        }

        public string GetDernierFichierOuvert()
        {
            string pathFichier = "";

            try
            {
                if (File.Exists(pathFichierConfig))
                {
                    XDocument fichierConfig = XDocument.Load(pathFichierConfig);
                    pathFichier = fichierConfig.Element("Racine").Element("DernierFichier").Value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Le dernier fichier ouvert n'a pas pu être récupéré :\n", ex);
            }

            return pathFichier;
        }

        public void SetDernierFichierOuvert(string pathFichier)
        {
            CreateFichierConfigIfNotExists();
            XDocument fichierConfig = XDocument.Load(pathFichierConfig);
            fichierConfig.Element("Racine").Element("DernierFichier").SetValue(pathFichier);
            fichierConfig.Save(pathFichierConfig);
        }

    }
}
