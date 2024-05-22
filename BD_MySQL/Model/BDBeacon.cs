using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDBeacon
    {
        private int id;
        private string code;

        public BDBeacon(int id, string code)
        {
            Id = id;
            Code = code;
        }

        public int Id { get => id; set => id = value; }
        public string Code { get => code; set => code = value; }

        public static string getCodeFromBeaconById(int idBeacon)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "id_beacon", idBeacon, System.Data.DbType.Int32);
                        consulta.CommandText = @"select bea_code from beacons where bea_id = @id_beacon";

                        DbDataReader reader = consulta.ExecuteReader();
                        string code = "";
                        while (reader.Read())
                        {
                            code = reader.GetString(reader.GetOrdinal("bea_code"));
                        }
                        return code;
                    }
                }
            }
        }

        public static List<String> getBeaconsSenseUtilitzar(int cursaId)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "idCursa", cursaId, System.Data.DbType.Int32);
                        consulta.CommandText = @"SELECT b.bea_code 
                                            FROM beacons b
                                            LEFT JOIN inscripcio i ON b.bea_id  = i.ins_bea_id 
                                            LEFT JOIN circuits_categories cc ON i.ins_ccc_id = cc.ccc_id
                                            LEFT JOIN circuits c ON cc.ccc_cir_id = c.cir_id
                                            LEFT JOIN curses cu ON c.cir_cur_id = cu.cur_id AND cu.cur_id = @idCursa
                                            WHERE cu.cur_id IS NULL";

                        DbDataReader reader = consulta.ExecuteReader();
                        List<String> codes = new List<String>();
                        while (reader.Read())
                        {
                            string code = reader.GetString(reader.GetOrdinal("bea_code"));
                            codes.Add(code);
                        }
                        return codes;
                    }
                }
            }
        }

        public static int getIdFromBeaconByCode(string codeBeacon)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "code", codeBeacon, System.Data.DbType.String);
                        consulta.CommandText = @"select bea_id from beacons where bea_code = @code";

                        DbDataReader reader = consulta.ExecuteReader();
                        int id = 0;
                        while (reader.Read())
                        {
                            id= reader.GetInt32(reader.GetOrdinal("bea_id"));
                        }
                        return id;
                    }
                }
            }
        }
    }
}
