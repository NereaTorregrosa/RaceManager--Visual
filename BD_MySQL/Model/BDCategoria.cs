using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDCategoria
    {
        private int id;
        private int esportId;
        private string nom;

        public BDCategoria(int id, int esportId, string nom)
        {
            Id = id;
            EsportId = esportId;
            Nom = nom;
        }

        public int Id { get => id; set => id = value; }
        public int EsportId { get => esportId; set => esportId = value; }
        public string Nom { get => nom; set => nom = value; }

        public static OC<BDCategoria> getCategoriesFromEsport(int idEsport)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "idEsport", idEsport, System.Data.DbType.Int32);
                        consulta.CommandText = @"select * from categories where cat_esp_id = @idEsport";
                        DbDataReader reader = consulta.ExecuteReader();
                        OC<BDCategoria> categories = new OC<BDCategoria>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("cat_id"));
                            string nom = reader.GetString(reader.GetOrdinal("cat_nom"));
                            int esport = reader.GetInt32(reader.GetOrdinal("cat_esp_id"));
                            categories.Add(new BDCategoria(id,esport,nom));
                        }
                        return categories;
                    }
                }
            }
        }

        public static BDCategoria getCategoriaById(int id)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "id", id, System.Data.DbType.Int32);

                        consulta.CommandText = @"select * from categories where cat_id = @id";
                        DbDataReader reader = consulta.ExecuteReader();
                        BDCategoria c = null;
                        
                        while (reader.Read())
                        {
                            string nomCategoria = reader.GetString(reader.GetOrdinal("cat_nom"));
                            int esport = reader.GetInt32(reader.GetOrdinal("cat_esp_id"));
                            c = new BDCategoria(id, esport,nomCategoria);
                        }
                        return c;
                    }
                }
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is BDCategoria categoria &&
                   Nom == categoria.Nom;
        }
    }
}
