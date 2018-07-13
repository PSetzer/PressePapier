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
    }
}
