using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDUsuari
    {
        private long id;
        private string username;
        private string password;
        private bool admin;

        public BDUsuari(long id, string username, string password, bool admin)
        {
            Id = id;
            Username = username;
            Password = password;
            Admin = admin;
        }

        public long Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public bool Admin { get => admin; set => admin = value; }

        public static bool isLoginCorrect(string user, string pass) {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "username", user , System.Data.DbType.String);
                        DBUtils.createParam(consulta, "password", pass , System.Data.DbType.String);
                        consulta.CommandText = @"select * from usuaris where 
                                                (usr_login = @username) and 
                                                (usr_password = @password) and 
                                                (usr_admin = 1)";
                        DbDataReader reader = consulta.ExecuteReader();
                        if (reader.Read())
                        {
                            return true;
                        }
                        else {
                            return false;
                        }
                        
                    }
                }
            }
        }
    }
}
