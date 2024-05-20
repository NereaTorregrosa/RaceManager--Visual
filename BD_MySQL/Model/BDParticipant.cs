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

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Cognoms { get => cognoms; set => cognoms = value; }
        public DateTime Data_naixement { get => data_naixement; set => data_naixement = value; }
        public string Telefon { get => telefon; set => telefon = value; }
        public string Email { get => email; set => email = value; }
        public bool EsFederat { get => esFederat; set => esFederat = value; }
        public string Nif { get => nif; set => nif = value; }

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
    }
}
