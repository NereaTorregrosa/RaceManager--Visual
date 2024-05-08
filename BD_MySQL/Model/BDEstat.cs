using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDEstat
    {
        private int id;
        private string nom;

        public BDEstat(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }

        public static string getEstatById(int id)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "id", id, System.Data.DbType.Int32);

                        consulta.CommandText = @"select est_nom from estats_cursa where est_id = @id";
                        DbDataReader reader = consulta.ExecuteReader();
                        string nomEstat = "";
                        while (reader.Read())
                        {
                            nomEstat = reader.GetString(reader.GetOrdinal("est_nom"));
                        }
                        return nomEstat;
                    }
                }
            }
        }
    }
}
