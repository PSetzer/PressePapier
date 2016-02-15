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
	class FichierServices
	{
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
    }
}
