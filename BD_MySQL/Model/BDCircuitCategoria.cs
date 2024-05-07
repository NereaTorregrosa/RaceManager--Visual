using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDCircuitCategoria
    {
        private int id;
        private int categoriaId;
        private int circuitId;

        public BDCircuitCategoria(int id, int categoriaId, int circuitId)
        {
            Id = id;
            CategoriaId = categoriaId;
            CircuitId = circuitId;
        }

        public BDCircuitCategoria(int categoriaId, int circuitId)
        {
            CategoriaId = categoriaId;
            CircuitId = circuitId;
        }

        public int Id { get => id; set => id = value; }
        public int CategoriaId { get => categoriaId; set => categoriaId = value; }
        public int CircuitId { get => circuitId; set => circuitId = value; }

        public static int getCategoriaId(int idCircuit)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "id", idCircuit, System.Data.DbType.Int32);

                        consulta.CommandText = @"select ccc_cat_id from circuits_categories where ccc_cir_id = @id";
                        DbDataReader reader = consulta.ExecuteReader();
                        int idCategoria = 0;
                        while (reader.Read())
                        {
                            idCategoria = reader.GetInt32(reader.GetOrdinal("ccc_cat_id"));
                        }
                       
                        return idCategoria;
                    }
                }
            }
        }
    }
}
