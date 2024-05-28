using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDParticipant
    {
        private int id;
        private string nif;
        private string nom;
        private string cognoms;
        private DateTime data_naixement;
        private string telefon;
        private string email;
        private bool esFederat;
        private int numFederat;

        public BDParticipant(int id, string nom, string cognoms, DateTime data_naixement, string telefon, string email, bool esFederat, string nif)
        {
            Id = id;
            Nom = nom;
            Cognoms = cognoms;
            Data_naixement = data_naixement;
            Telefon = telefon;
            Email = email;
            EsFederat = esFederat;
            Nif = nif;
        }

        public BDParticipant(string nom, string cognoms, DateTime data_naixement, string telefon, string email, bool esFederat, string nif, int numFederat)
        {
            Nom = nom;
            Cognoms = cognoms;
            Data_naixement = data_naixement;
            Telefon = telefon;
            Email = email;
            EsFederat = esFederat;
            Nif = nif;
            NumFederat = numFederat;
        }

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Cognoms { get => cognoms; set => cognoms = value; }
        public DateTime Data_naixement { get => data_naixement; set => data_naixement = value; }
        public string Telefon { get => telefon; set => telefon = value; }
        public string Email { get => email; set => email = value; }
        public bool EsFederat { get => esFederat; set => esFederat = value; }
        public string Nif { get => nif; set => nif = value; }
        public int NumFederat { get => numFederat; set => numFederat = value; }

        public static List<BDParticipant> getParticipantsFromCursa(int idCursa, String filtre = "")
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "filtre", "%" + filtre + "%", System.Data.DbType.String);
                        DBUtils.createParam(consulta, "id_cursa", idCursa, System.Data.DbType.Int32);
                        consulta.CommandText = @"select * from view_participants_inscripcio_cursa where ((cur_id = @id_cursa) and
                                                (((par_nif like @filtre) or (par_nom like @filtre) or (par_cognoms like @filtre) 
                                                or (par_telefon like @filtre))))";

                        DbDataReader reader = consulta.ExecuteReader();
                        List<BDParticipant> participants = new List<BDParticipant>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("par_id"));
                            String name = reader.GetString(reader.GetOrdinal("par_nom"));
                            string cognoms = reader.GetString(reader.GetOrdinal("par_cognoms"));
                            DateTime dataNiax = reader.GetDateTime(reader.GetOrdinal("par_data_naixement"));
                            string telefon = reader.GetString(reader.GetOrdinal("par_telefon"));
                            string email = reader.GetString(reader.GetOrdinal("par_email"));
                            string nif = reader.GetString(reader.GetOrdinal("par_nif"));
                            bool esFederat = reader.GetBoolean(reader.GetOrdinal("par_es_federat"));
                            BDParticipant participant = new BDParticipant(id,name,cognoms,dataNiax,telefon,email,esFederat,nif);
                            participants.Add(participant);
                        }
                        return participants;
                    }
                }
            }
        }

        public static Task<bool> checkIfParticipantExist(string dni)
        {
            return Task.Run(() =>
            {
                using (var context = new MySqlDbContext())
                {
                    using (var connexio = context.Database.GetDbConnection())
                    {
                        connexio.Open();
                        using (var consulta = connexio.CreateCommand())
                        {
                            DBUtils.createParam(consulta, "dni", dni, System.Data.DbType.String);
                            consulta.CommandText = @"select * from participant where par_nif = @dni";
                            using (var reader = consulta.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }

                        }
                    }
                }
            });
        }


        public static BDParticipant getParticipantByDNI(string dni)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "dni", dni, System.Data.DbType.String);
                        consulta.CommandText = @"select * from participant where par_nif = @dni";

                        DbDataReader reader = consulta.ExecuteReader();
                        BDParticipant p = null;
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("par_id"));
                            String name = reader.GetString(reader.GetOrdinal("par_nom"));
                            string cognoms = reader.GetString(reader.GetOrdinal("par_cognoms"));
                            DateTime dataNiax = reader.GetDateTime(reader.GetOrdinal("par_data_naixement"));
                            string telefon = reader.GetString(reader.GetOrdinal("par_telefon"));
                            string email = reader.GetString(reader.GetOrdinal("par_email"));
                            string nif = reader.GetString(reader.GetOrdinal("par_nif"));
                            bool esFederat = reader.GetBoolean(reader.GetOrdinal("par_es_federat"));
                            p= new BDParticipant(id, name, cognoms, dataNiax, telefon, email, esFederat, nif);
                        }
                        return p;
                    }
                }
            }
        }

        public static int ObtenirUltimParticipantId()
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        consulta.CommandText = @"SELECT par_id FROM participant ORDER BY par_id DESC LIMIT 1";
                        int ultimParticipantId = -1;
                        object resultat = consulta.ExecuteScalar();
                        if (resultat != null && resultat != DBNull.Value)
                        {
                            ultimParticipantId = Convert.ToInt32(resultat);
                        }
                        return ultimParticipantId;
                    }
                }
            }
        }

        public static bool insertParticipant(BDParticipant p)
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
                            DBUtils.createParam(consulta, "nif", p.Nif, System.Data.DbType.String);
                            DBUtils.createParam(consulta, "nom", p.Nom, System.Data.DbType.String);
                            DBUtils.createParam(consulta, "cognoms", p.Cognoms, System.Data.DbType.String);
                            DBUtils.createParam(consulta, "dataNaix", p.Data_naixement, System.Data.DbType.DateTime);
                            DBUtils.createParam(consulta, "telefon", p.Telefon, System.Data.DbType.String);
                            DBUtils.createParam(consulta, "email", p.Email, System.Data.DbType.String);
                            DBUtils.createParam(consulta, "esFederat", p.EsFederat, System.Data.DbType.Boolean);
                            DBUtils.createParam(consulta, "numFederat", p.NumFederat, System.Data.DbType.Int32);

                            consulta.CommandText = @"INSERT INTO participant (par_nif,par_nom,
                                                    par_cognoms,par_data_naixement,par_telefon,par_email,
                                                    par_es_federat,par_num_federat) 
                                                    VALUES (@nif, @nom,
                                                    @cognoms,
                                                    @dataNaix,
                                                    @telefon,
                                                    @email,@esFederat,@numFederat)";

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
