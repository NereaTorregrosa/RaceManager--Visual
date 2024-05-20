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
                            int idBeacon = reader.GetInt32(reader.GetOrdinal("ins_bea_id"));
                            int idCircuitCategoria = reader.GetInt32(reader.GetOrdinal("ins_ccc_id"));
                            inscripcio = new BDInscripcio(id,idParticipant,dataInsc,dorsal,retirat,idBeacon,idCircuitCategoria);
                        }
                        return inscripcio;
                    }
                }
            }
        }
    }
}
