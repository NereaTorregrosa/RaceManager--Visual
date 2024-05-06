using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDCursa
    {
        private int id;
        private string nom;
        private DateTime dataInici;
        private DateTime dataFi;
        private string lloc;
        private int esportId;
        private int estatId;
        private string descripcio;
        private int limitInscripcions;
        private string urlFoto;
        private string urlWeb;

        public BDCursa(int id, string nom, DateTime dataInici, DateTime dataFi, string lloc, int esportId, int estatId, string descripcio, int limitInscripcions, string urlFoto, string urlWeb)
        {
            Id = id;
            Nom = nom;
            DataInici = dataInici;
            DataFi = dataFi;
            Lloc = lloc;
            EsportId = esportId;
            EstatId = estatId;
            Descripcio = descripcio;
            LimitInscripcions = limitInscripcions;
            UrlFoto = urlFoto;
            UrlWeb = urlWeb;
        }

        public BDCursa(string nom, DateTime dataInici, DateTime dataFi, string lloc, int esportId, int estatId, string descripcio, int limitInscripcions, string urlFoto, string urlWeb)
        {
            Nom = nom;
            DataInici = dataInici;
            DataFi = dataFi;
            Lloc = lloc;
            EsportId = esportId;
            EstatId = estatId;
            Descripcio = descripcio;
            LimitInscripcions = limitInscripcions;
            UrlFoto = urlFoto;
            UrlWeb = urlWeb;
        }

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public DateTime DataInici { get => dataInici; set => dataInici = value; }
        public DateTime DataFi { get => dataFi; set => dataFi = value; }
        public string Lloc { get => lloc; set => lloc = value; }
        public int EsportId { get => esportId; set => esportId = value; }
        public int EstatId { get => estatId; set => estatId = value; }
        public string Descripcio { get => descripcio; set => descripcio = value; }
        public int LimitInscripcions { get => limitInscripcions; set => limitInscripcions = value; }
        public string UrlFoto { get => urlFoto; set => urlFoto = value; }
        public string UrlWeb { get => urlWeb; set => urlWeb = value; }


        public static List<BDCursa> getCurses(String mNom = "", DateTime? dt = null, int? idEstat = null)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "nom", "%" + mNom + "%", System.Data.DbType.String);
                        DBUtils.createParam(consulta,
                                                "data_inici",
                                                dt == null ? DateTime.MinValue : dt,
                                                System.Data.DbType.DateTime);
                        DBUtils.createParam(consulta,"id_estat",idEstat,System.Data.DbType.Int32);
                        consulta.CommandText = @"select * from curses  where 
                                    ((cur_nom like @nom) or (cur_lloc like @nom)) and 
                                    ( cur_data_inici>=@data_inici) and (@id_estat = 0 or  cur_est_id = @id_estat) ";

                        DbDataReader reader = consulta.ExecuteReader();
                        List<BDCursa> curses = new List<BDCursa>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("cur_id"));
                            String name = reader.GetString(reader.GetOrdinal("cur_nom"));
                            DateTime di = reader.GetDateTime(reader.GetOrdinal("cur_data_inici"));
                            DateTime df = reader.GetDateTime(reader.GetOrdinal("cur_data_fi"));
                            string lloc = reader.GetString(reader.GetOrdinal("cur_lloc"));
                            int esportId = reader.GetInt32(reader.GetOrdinal("cur_esp_id"));
                            int estatId = reader.GetInt32(reader.GetOrdinal("cur_est_id"));
                            string desc = reader.GetString(reader.GetOrdinal("cur_desc"));
                            int limitInsc = reader.GetInt32(reader.GetOrdinal("cur_limit_inscr"));
                            string foto = reader.GetString(reader.GetOrdinal("cur_foto"));
                            string web = reader.GetString(reader.GetOrdinal("cur_web"));

                            curses.Add(new BDCursa(id, name,di,df,lloc,esportId,estatId,desc,limitInsc,foto,web));
                        }
                        return curses;
                    }
                }
            }
        }

        public static void insertCursa(BDCursa c)
        {

        }
    }
}
