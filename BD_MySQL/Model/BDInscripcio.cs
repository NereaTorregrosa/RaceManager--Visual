using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDInscripcio
    {
        private int id;
        private int idParticipant;
        private DateTime data_inscripcio;
        private int dorsal;
        private bool retirat;
        private int idBeacon;
        private int idCircuitCategoria;

        public BDInscripcio(int id, int idParticipant, DateTime data_inscripcio, int dorsal, bool retirat, int idBeacon, int idCircuitCategoria)
        {
            Id = id;
            IdParticipant = idParticipant;
            Data_inscripcio = data_inscripcio;
            Dorsal = dorsal;
            Retirat = retirat;
            IdBeacon = idBeacon;
            IdCircuitCategoria = idCircuitCategoria;
        }

        public BDInscripcio( int idParticipant, DateTime data_inscripcio, int dorsal, bool retirat, int idBeacon, int idCircuitCategoria)
        {
            IdParticipant = idParticipant;
            Data_inscripcio = data_inscripcio;
            Dorsal = dorsal;
            Retirat = retirat;
            IdBeacon = idBeacon;
            IdCircuitCategoria = idCircuitCategoria;
        }

        public int Id { get => id; set => id = value; }
        public int IdParticipant { get => idParticipant; set => idParticipant = value; }
        public DateTime Data_inscripcio { get => data_inscripcio; set => data_inscripcio = value; }
        public int Dorsal { get => dorsal; set => dorsal = value; }
        public bool Retirat { get => retirat; set => retirat = value; }
        public int IdBeacon { get => idBeacon; set => idBeacon = value; }
        public int IdCircuitCategoria { get => idCircuitCategoria; set => idCircuitCategoria = value; }

        public static BDInscripcio getDadesInscripcioFromParticipant(int idParticipant, int idCursa)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "id_participant", idParticipant, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "id_cursa", idCursa, System.Data.DbType.Int32);
                        consulta.CommandText = @"select * from view_participants_inscripcio_cursa where ins_par_id = @id_participant and cur_id = @id_cursa";

                        DbDataReader reader = consulta.ExecuteReader();
                        BDInscripcio inscripcio = null;
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("ins_id"));
                            DateTime dataInsc = reader.GetDateTime(reader.GetOrdinal("ins_data"));
                            int dorsal = reader.GetInt32(reader.GetOrdinal("ins_dorsal"));
                            bool retirat = reader.GetBoolean(reader.GetOrdinal("ins_retirat"));
                            int idBeacon = 0;
                            if (!reader.IsDBNull(reader.GetOrdinal("ins_bea_id")))
                            {
                                idBeacon = reader.GetInt32(reader.GetOrdinal("ins_bea_id"));
                            }
                            int idCircuitCategoria = reader.GetInt32(reader.GetOrdinal("ins_ccc_id"));
                            inscripcio = new BDInscripcio(id,idParticipant,dataInsc,dorsal,retirat,idBeacon,idCircuitCategoria);
                        }
                        return inscripcio;
                    }
                }
            }
        }

        public static bool updateInscripcio(BDInscripcio i)
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

                        DBUtils.createParam(consulta, "dorsal", i.Dorsal, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "idBeacon", i.IdBeacon, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "retirat", i.Retirat, System.Data.DbType.Boolean);
                        DBUtils.createParam(consulta, "id", i.Id, System.Data.DbType.Int32);

                        consulta.CommandText =
                            @"update inscripcions set                             
                                ins_dorsal            = @dorsal,
                                ins_bea_id            = @idBeacon,
                                ins_retirat           = @retirat
                                where
                                    ins_id      = @id
                                ";
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

        public static bool IsDorsalDuplicated(int cccId, int dorsal)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        consulta.CommandText = @"
                    SELECT COUNT(*)
                    FROM inscripcions
                    WHERE ins_ccc_id = @cccId AND ins_dorsal = @dorsal
                ";

                        DBUtils.createParam(consulta, "cccId", cccId, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "dorsal", dorsal, System.Data.DbType.Int32);

                        int count = Convert.ToInt32(consulta.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
        }

        public static bool insertInscripcio(BDInscripcio i)
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
                            DBUtils.createParam(consulta, "parId", i.IdParticipant, System.Data.DbType.Int32);
                            DBUtils.createParam(consulta, "data", i.Data_inscripcio, System.Data.DbType.DateTime);
                            DBUtils.createParam(consulta, "dorsal", i.Dorsal, System.Data.DbType.Int32);
                            DBUtils.createParam(consulta, "retirat", i.Retirat, System.Data.DbType.Boolean);
                            if(i.IdBeacon == 0)
                            {
                                DBUtils.createParam(consulta, "beaconId", null, System.Data.DbType.Int32);
                            }
                            else
                            {
                                DBUtils.createParam(consulta, "beaconId", i.IdBeacon, System.Data.DbType.Int32);
                            }
                            DBUtils.createParam(consulta, "idCC", i.IdCircuitCategoria, System.Data.DbType.Int32);

                            consulta.CommandText = @"INSERT INTO inscripcions (ins_par_id,ins_data,
                                                    ins_dorsal,ins_retirat,ins_bea_id,ins_ccc_id) 
                                                    VALUES (@parId, @data,
                                                    @dorsal,
                                                    @retirat,
                                                    @beaconId,
                                                    @idCC)";

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
