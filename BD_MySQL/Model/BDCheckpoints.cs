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
    }
}
