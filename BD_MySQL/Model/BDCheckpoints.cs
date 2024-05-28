using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDCheckpoints
    {
        private int id;
        private double kilometre;
        private int idCircuit;
        private BDCheckpoints checkpointNou;

        public BDCheckpoints()
        {
        }
        public BDCheckpoints(int id, double kilometre, int idCircuit)
        {
            Id = id;
            Kilometre = kilometre;
            IdCircuit = idCircuit;
        }

        public BDCheckpoints(double kilometre, int idCircuit)
        {
            Kilometre = kilometre;
            IdCircuit = idCircuit;
        }

        public int Id { get => id; set => id = value; }
        public double Kilometre { get => kilometre; set => kilometre = value; }
        public int IdCircuit { get => idCircuit; set => idCircuit = value; }
        public BDCheckpoints CheckpointNou { get => checkpointNou; set => checkpointNou = value; }

        public BDCheckpoints Clone()
        {
            return new BDCheckpoints(Id, Kilometre, IdCircuit);
        }

        public static OC<BDCheckpoints> getCheckpointsFromCircuit(int idCircuit)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "idCircuit", idCircuit, System.Data.DbType.Int32);
                        consulta.CommandText = @"select * from checkpoints where chk_cir_id = @idCircuit";
                        DbDataReader reader = consulta.ExecuteReader();
                        OC<BDCheckpoints> checkpoints = new OC<BDCheckpoints>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("chk_id"));
                            int km = reader.GetInt32(reader.GetOrdinal("chk_km"));
                            int circuitId = reader.GetInt32(reader.GetOrdinal("chk_cir_id"));
                            checkpoints.Add(new BDCheckpoints(id, km, circuitId));
                        }
                        return checkpoints;
                    }
                }
            }
        }

        public static bool insertCheckpoint(BDCheckpoints c)
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
                            DBUtils.createParam(consulta, "km", c.Kilometre, System.Data.DbType.Double);
                            DBUtils.createParam(consulta, "idCircuit", c.IdCircuit, System.Data.DbType.Int32);

                            consulta.CommandText = @"INSERT INTO checkpoints (chk_km,chk_cir_id) 
                                                    VALUES (@km, @idCircuit)";

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

        public static bool CheckpointExists(int circuitId, double kilometre)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "circuitId", circuitId, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "kilometre", kilometre, System.Data.DbType.Double);
                        consulta.CommandText = @"select count(*) from checkpoints where chk_cir_id = @circuitId and chk_km = @kilometre";
                        int count = Convert.ToInt32(consulta.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
        }

        public static bool UpdateCheckpoint(BDCheckpoints c)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {

                    connexio.Open();

                    DbTransaction transaccio = connexio.BeginTransaction();

                    using (var consulta = context.Database.GetDbConnection().CreateCommand())
                    {

                        DBUtils.createParam(consulta, "newKilometre", c.kilometre, System.Data.DbType.Double);
                        DBUtils.createParam(consulta, "newCircuitId", c.IdCircuit, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "checkpointId", c.Id, System.Data.DbType.Int32);
                        consulta.CommandText = @"UPDATE checkpoints SET 
                                        chk_km = @newKilometre, 
                                        chk_cir_id = @newCircuitId 
                                        WHERE chk_id = @checkpointId";
                        int filesModificades = consulta.ExecuteNonQuery();
                        if (filesModificades != 1)
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

        public static void deleteCheckpoints(int circuitId)
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

                        DBUtils.createParam(consulta, "id", circuitId, System.Data.DbType.Int32);
                        consulta.CommandText = @"DELETE FROM checkpoints WHERE chk_cir_id = @id";

                        int rowsAffected = consulta.ExecuteNonQuery();

                        if (rowsAffected <=0)
                        {
                            transaccio.Rollback();
                        }
                        else
                        {
                            transaccio.Commit();
                        }
                    }
                }
            }


        }

        public static int numeroCheckpoints(int circuitId)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "circuitId", circuitId, System.Data.DbType.Int32);
                        consulta.CommandText = @"select count(*) from checkpoints where chk_cir_id = @circuitId";
                        int count = Convert.ToInt32(consulta.ExecuteScalar());
                        return count;
                    }
                }
            }
        }

        public static int getCheckpointById(int idCheckpoint)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "idCheckpoint", idCheckpoint, System.Data.DbType.Int32);
                        consulta.CommandText = @"select * from checkpoints where chk_id = @idCheckpoint";
                        DbDataReader reader = consulta.ExecuteReader();
                        int km = 0;
                        while (reader.Read())
                        {
                            km = reader.GetInt32(reader.GetOrdinal("chk_km"));
                        }
                        return km;
                    }
                }
            }
        }
    }
}
