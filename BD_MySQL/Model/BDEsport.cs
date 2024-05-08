using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDEsport
    {
        private int id;
        private string nom;

        public BDEsport(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }

        public static OC<BDEsport> getEsports()
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        consulta.CommandText = @"select * from esports";
                        DbDataReader reader = consulta.ExecuteReader();
                        OC<BDEsport> esports = new OC<BDEsport>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("esp_id"));
                            string nom = reader.GetString(reader.GetOrdinal("esp_nom"));
                            esports.Add(new BDEsport(id, nom));
                        }
                        return esports;
                    }
                }
            }
        }

        public static string getEsportById(int id)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "id", id, System.Data.DbType.Int32);

                        consulta.CommandText = @"select esp_nom from esports where esp_id = @id";
                        DbDataReader reader = consulta.ExecuteReader();
                        string nomEsport = "";
                        while (reader.Read())
                        {
                            nomEsport = reader.GetString(reader.GetOrdinal("esp_nom"));
                        }
                        return nomEsport;
                    }
                }
            }
        }
    }
}
