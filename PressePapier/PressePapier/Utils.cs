using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressePapier
{
    public static class Utils
    {
        public static string GetNomFichier(string pathFichier)
        {
            string nomFichier = "";

            try
            {
                if (pathFichier != "")
                {
                    string nomFichierAvecExt = pathFichier.Split('\\').Last<string>();
                    string extension = nomFichierAvecExt.Split('.').Last<string>();
                    var elemsNomFichier = nomFichierAvecExt.Split('.').TakeWhile<string>(item => item != extension);
                    nomFichier = elemsNomFichier.Aggregate<string>((item, next) => item + "." + next);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return nomFichier;
        }
    }
}
