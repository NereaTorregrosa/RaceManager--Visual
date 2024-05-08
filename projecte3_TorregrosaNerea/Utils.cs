using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace projecte3_TorregrosaNerea
{
    public class Utils
    {
        public static bool EsNumeroEnter(string text)
        {
            if (int.TryParse(text, out _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool EsDecimal(string valor)
        {
            decimal resultat;
            return decimal.TryParse(valor, out resultat);
        }

        public static bool EsDouble(string valor)
        {
            double resultat;
            return double.TryParse(valor, out resultat);
        }

        public static bool TeFormatHora(string valor)
        {
            string patro = @"^\d{2}:\d{2}:\d{2}$";

            return Regex.IsMatch(valor, patro);
        }
    }
}
