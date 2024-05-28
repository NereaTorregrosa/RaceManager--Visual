using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDRegistres
    {
        private int id;
        private int idInscripcio;
        private int idCheckpoint;
        private DateTime temps;
        private String nomParticipant;
        private int dorsalParticipant;
        private int idParticipant;
        private int kmCheckpoint;
        private bool isRetirat;

        public BDRegistres(int id, int idInscripcio, int idCheckpoint, DateTime temps)
        {
            Id = id;
            IdInscripcio = idInscripcio;
            IdCheckpoint = idCheckpoint;
            Temps = temps;
        }

        public int Id { get => id; set => id = value; }
        public int IdInscripcio { get => idInscripcio; set => idInscripcio = value; }
        public int IdCheckpoint { get => idCheckpoint; set => idCheckpoint = value; }
        public DateTime Temps { get => temps; set => temps = value; }
        public string NomParticipant { get => nomParticipant; set => nomParticipant = value; }
        public int DorsalParticipant { get => dorsalParticipant; set => dorsalParticipant = value; }
        public int IdParticipant { get => idParticipant; set => idParticipant = value; }
        public int KmCheckpoint { get => kmCheckpoint; set => kmCheckpoint = value; }
        public bool IsRetirat { get => isRetirat; set => isRetirat = value; }

        public static OC<BDRegistres> registresCursaParcial(int idCursa,int idCircuit, string nomFiltre="",int? dorsalFiltre = null)
        {

            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "idCursa", idCursa, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "idCircuit", idCircuit, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "nomFiltre", "%" + nomFiltre + "%", System.Data.DbType.String);
                        DBUtils.createParam(consulta, "dorsalFiltre", dorsalFiltre, System.Data.DbType.Int32);
                        consulta.CommandText = @"SELECT 
                                                p.*, 
                                                i.*, 
                                                r.*
                                                FROM 
                                                    participant p
                                                INNER JOIN 
                                                    inscripcions i ON p.par_id = i.ins_par_id
                                                INNER JOIN 
                                                    registres r ON i.ins_id = r.reg_ins_id
                                                INNER JOIN 
                                                    checkpoints chk ON r.reg_chk_id = chk.chk_id
                                                INNER JOIN 
                                                    circuits c ON chk.chk_cir_id = c.cir_id
                                                INNER JOIN 
                                                    curses cur ON c.cir_cur_id = cur.cur_id
                                                WHERE 
                                                cur.cur_id = @idCursa 
                                                AND c.cir_id = @idCircuit
                                                AND r.reg_temps = (
                                                    SELECT MAX(r2.reg_temps)
                                                    FROM registres r2
                                                    INNER JOIN inscripcions i2 ON r2.reg_ins_id = i2.ins_id
                                                    WHERE i2.ins_par_id = p.par_id
                                                )
                                                AND ((p.par_nom like @nomFiltre) or (p.par_cognoms like @nomFiltre)) 
                                                AND ((i.ins_dorsal = @dorsalFiltre) or (@dorsalFiltre is NULL))";

                        DbDataReader reader = consulta.ExecuteReader();
                        OC<BDRegistres> registres = new OC<BDRegistres>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("reg_id"));
                            int idInscripcio = reader.GetInt32(reader.GetOrdinal("reg_ins_id"));
                            DateTime temps = reader.GetDateTime(reader.GetOrdinal("reg_temps"));
                            string nom = reader.GetString(reader.GetOrdinal("par_nom"));
                            string cognoms = reader.GetString(reader.GetOrdinal("par_cognoms"));
                            int dorsal = reader.GetInt32(reader.GetOrdinal("ins_dorsal"));
                            int idParticipant = reader.GetInt32(reader.GetOrdinal("par_id"));
                            int idCheckpoint = reader.GetInt32(reader.GetOrdinal("reg_chk_id"));
                            int kmCheckpoint = BDCheckpoints.getCheckpointById(idCheckpoint); 
                            BDRegistres registreNou = new BDRegistres(id,idInscripcio,idCheckpoint,temps);
                            bool retirat = BDInscripcio.getRetirat(registreNou.IdInscripcio);
                            registreNou.NomParticipant = nom + " "+cognoms;
                            registreNou.DorsalParticipant = dorsal;
                            registreNou.IdParticipant = idParticipant;
                            registreNou.KmCheckpoint = kmCheckpoint;
                            registreNou.IsRetirat = retirat;
                            registres.Add(registreNou);
                        }
                        return registres;
                    }
                }
            }
        }

        public static int numeroCheckpointsPassats(int circuitId, int idParticipant,int cursaId)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "circuitId", circuitId, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "cursaId", cursaId, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "idParticipant", idParticipant, System.Data.DbType.Int32);
                        consulta.CommandText = @"SELECT COUNT(c.chk_id) AS NumeroCheckpointsPasats
                                                from participant p
                                                INNER JOIN inscripcions i ON p.par_id  = i.ins_par_id 
                                                INNER JOIN registres r ON i.ins_id = r.reg_ins_id 
                                                LEFT JOIN checkpoints c ON r.reg_chk_id  = c.chk_id 
                                                INNER JOIN 
                                                    circuits ci ON c.chk_cir_id = ci.cir_id 
                                                INNER JOIN 
                                                    curses cur ON ci.cir_cur_id  = cur.cur_id
                                                WHERE p.par_id  = @idParticipant and 
                                                cur.cur_id = @cursaId
                                                AND ci.cir_id = @circuitId
                                                GROUP BY p.par_nom";
                        int count = Convert.ToInt32(consulta.ExecuteScalar());
                        return count;
                    }
                }
            }
        }

        public static OC<BDRegistres> registresCursaTotal(int idCursa, int idCircuit, int idParticipant)
        {

            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "idCursa", idCursa, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "idCircuit", idCircuit, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "idParticipant", idParticipant, System.Data.DbType.Int32);
                        consulta.CommandText = @"SELECT  
                                                p.*, 
                                                i.*, 
                                                r.*
                                                FROM 
                                                    participant p
                                                INNER JOIN 
                                                    inscripcions i ON p.par_id = i.ins_par_id
                                                INNER JOIN 
                                                    registres r ON i.ins_id = r.reg_ins_id
                                                INNER JOIN 
                                                    checkpoints chk ON r.reg_chk_id = chk.chk_id
                                                INNER JOIN 
                                                    circuits c ON chk.chk_cir_id = c.cir_id
                                                INNER JOIN 
                                                    curses cur ON c.cir_cur_id = cur.cur_id
                                                WHERE 
                                                cur.cur_id = @idCursa 
                                                AND c.cir_id = @idCircuit
                                                AND p.par_id = @idParticipant";

                        DbDataReader reader = consulta.ExecuteReader();
                        OC<BDRegistres> registres = new OC<BDRegistres>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("reg_id"));
                            int idInscripcio = reader.GetInt32(reader.GetOrdinal("reg_ins_id"));
                            DateTime temps = reader.GetDateTime(reader.GetOrdinal("reg_temps"));
                            string nom = reader.GetString(reader.GetOrdinal("par_nom"));
                            string cognoms = reader.GetString(reader.GetOrdinal("par_cognoms"));
                            int dorsal = reader.GetInt32(reader.GetOrdinal("ins_dorsal"));
                            int idCheckpoint = reader.GetInt32(reader.GetOrdinal("reg_chk_id"));
                            BDRegistres registreNou = new BDRegistres(id, idInscripcio, idCheckpoint, temps);
                            int kmCheckpoint = BDCheckpoints.getCheckpointById(idCheckpoint);
                            bool retirat = BDInscripcio.getRetirat(registreNou.IdInscripcio);
                            registreNou.NomParticipant = nom + " " + cognoms;
                            registreNou.DorsalParticipant = dorsal;
                            registreNou.IdParticipant = idParticipant;
                            registreNou.KmCheckpoint = kmCheckpoint;
                            registreNou.IsRetirat = retirat;
                            registres.Add(registreNou);
                        }
                        return registres;
                    }
                }
            }
        }
    }
}
