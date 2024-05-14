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

        public static bool insertCircuitCategoria(int idCategoria, int idCircuit)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();

                    DbTransaction transaccion = connexio.BeginTransaction();

                    try
                    {
                        using (var consulta = connexio.CreateCommand())
                        {
                            consulta.Transaction = transaccion;
                            DBUtils.createParam(consulta, "idCategoria", idCategoria, System.Data.DbType.Int32);
                            DBUtils.createParam(consulta, "idCircuit", idCircuit, System.Data.DbType.Int32);

                            consulta.CommandText = @"INSERT INTO circuits_categories (ccc_cat_id,ccc_cir_id) 
                                                    VALUES (@idCategoria, @idCircuit)";

                            consulta.ExecuteNonQuery();
                            transaccion.Commit();
                            return true;

                        }
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        return false;
                    }

                }
            }
        }

        public static bool deleteCircuitCategoria(int circuitId)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();

                    // Comencem una transacció dins de la que volem executar updates
                    DbTransaction transaccio = connexio.BeginTransaction();

                    using (var consulta = connexio.CreateCommand())
                    {
                        consulta.Transaction = transaccio; // Associem la consulta a la transacció

                        // Ajusta la consulta para realizar la eliminación
                        DBUtils.createParam(consulta, "id", circuitId, System.Data.DbType.Int32);
                        consulta.CommandText = @"DELETE FROM circuits_categories WHERE ccc_cir_id = @id";

                        int rowsAffected = consulta.ExecuteNonQuery();

                        if (rowsAffected != 1)
                        {
                            transaccio.Rollback();
                            return false;
                        }
                        else
                        {
                            transaccio.Commit();
                            return true;
                        }
                    }
                }
            }
        }
    }
}
