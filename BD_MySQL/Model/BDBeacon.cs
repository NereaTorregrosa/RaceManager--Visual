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
    }
}
