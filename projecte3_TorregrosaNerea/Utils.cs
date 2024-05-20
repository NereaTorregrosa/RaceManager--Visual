using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (int.TryParse(text, out int numero) && numero >=0)
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
            if (decimal.TryParse(valor, out decimal resultat) && resultat >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool EsDouble(string valor)
        {
            if (double.TryParse(valor, out double resultat) && resultat >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TeFormatHora(string valor)
        {
            string patro = @"^\d{2}:\d{2}:\d{2}$";

            return Regex.IsMatch(valor, patro);
        }

        public static bool EsDataFutura(DateTime data1, DateTime data12)
        {

            return data12.Date.CompareTo(data1.Date) > 0;
        }

        public static string convertirDateTimeAString(DateTime d) 
        {
           return d.ToString("HH:mm:ss");
        }

        public static string convertirDateTimeAStringData(DateTime d)
        {
            return d.ToString("dd/MM/yyyy");
        }

        public static  DateTime aconseguirTempsEstimat(string text)
        {
            TimeSpan intervalTemps = TimeSpan.ParseExact(text, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
            DateTime dataAvui = DateTime.Now.Date;
            return dataAvui.Add(intervalTemps);

        }

    }
}
