using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PressePapier
{
    class ConfigServices
    {
        string pathDossierConfig;
        string pathFichierConfig;

        public ConfigServices()
        {
            pathDossierConfig = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PressePapier";
            pathFichierConfig = pathDossierConfig + "\\config.xml";
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

        private void CreateFichierConfigIfNotExists()
        {
            try
            {
                if (!Directory.Exists(pathDossierConfig))
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
    }
}
