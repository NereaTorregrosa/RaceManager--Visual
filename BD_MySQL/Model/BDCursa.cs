using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDCursa
    {
        private int id;
        private string nom;
        private DateTime dataInici;
        private DateTime dataFi;
        private string lloc;
        private int esportId;
        private int estatId;
        private string descripcio;
        private int limitInscripcions;
        private string urlFoto;
        private string urlWeb;
        private string errorEliminar;
        private int numParticipants;
        private string estat;

        public BDCursa(int id, string nom, DateTime dataInici, DateTime dataFi, string lloc, int esportId, int estatId, string descripcio, int limitInscripcions, string urlFoto, string urlWeb)
        {
            Id = id;
            Nom = nom;
            DataInici = dataInici;
            DataFi = dataFi;
            Lloc = lloc;
            EsportId = esportId;
            EstatId = estatId;
            Descripcio = descripcio;
            LimitInscripcions = limitInscripcions;
            UrlFoto = urlFoto;
            UrlWeb = urlWeb;
        }

        public BDCursa(string nom, DateTime dataInici, DateTime dataFi, string lloc, int esportId, int estatId, string descripcio, int limitInscripcions, string urlFoto, string urlWeb)
        {
            Nom = nom;
            DataInici = dataInici;
            DataFi = dataFi;
            Lloc = lloc;
            EsportId = esportId;
            EstatId = estatId;
            Descripcio = descripcio;
            LimitInscripcions = limitInscripcions;
            UrlFoto = urlFoto;
            UrlWeb = urlWeb;
        }

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public DateTime DataInici { get => dataInici; set => dataInici = value; }
        public DateTime DataFi { get => dataFi; set => dataFi = value; }
        public string Lloc { get => lloc; set => lloc = value; }
        public int EsportId { get => esportId; set => esportId = value; }
        public int EstatId { get => estatId; set => estatId = value; }
        public string Descripcio { get => descripcio; set => descripcio = value; }
        public int LimitInscripcions { get => limitInscripcions; set => limitInscripcions = value; }
        public string UrlFoto { get => urlFoto; set => urlFoto = value; }
        public string UrlWeb { get => urlWeb; set => urlWeb = value; }
        public  string ErrorEliminar { get => errorEliminar; set => errorEliminar = value; }
        public int NumParticipants { get => numParticipants; set => numParticipants = value; }
        public string Estat { get => estat; set => estat = value; }

        public static List<BDCursa> getCurses(String mNom = "", DateTime? dt = null, int? idEstat = null)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "nom", "%" + mNom + "%", System.Data.DbType.String);
                        DBUtils.createParam(consulta,
                                                "data_inici",
                                                dt == null ? DateTime.MinValue : dt,
                                                System.Data.DbType.DateTime);
                        DBUtils.createParam(consulta,"id_estat",idEstat,System.Data.DbType.Int32);
                        consulta.CommandText = @"select * from curses  where 
                                    ((cur_nom like @nom) or (cur_lloc like @nom)) and 
                                    ( cur_data_inici>=@data_inici) and (@id_estat = 0 or  cur_est_id = @id_estat) 
                                    order by cur_data_inici asc";

                        DbDataReader reader = consulta.ExecuteReader();
                        List<BDCursa> curses = new List<BDCursa>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("cur_id"));
                            String name = reader.GetString(reader.GetOrdinal("cur_nom"));
                            DateTime di = reader.GetDateTime(reader.GetOrdinal("cur_data_inici"));
                            DateTime df = reader.GetDateTime(reader.GetOrdinal("cur_data_fi"));
                            string lloc = reader.GetString(reader.GetOrdinal("cur_lloc"));
                            int esportId = reader.GetInt32(reader.GetOrdinal("cur_esp_id"));
                            int estatId = reader.GetInt32(reader.GetOrdinal("cur_est_id"));
                            string desc = reader.GetString(reader.GetOrdinal("cur_desc"));
                            int limitInsc = reader.GetInt32(reader.GetOrdinal("cur_limit_inscr"));
                            string foto = reader.GetString(reader.GetOrdinal("cur_foto"));
                            string web = reader.GetString(reader.GetOrdinal("cur_web"));
                            string estat = BDEstat.getEstatById(estatId);
                            BDCursa cursaNova = new BDCursa(id, name, di, df, lloc, esportId, estatId, desc, limitInsc, foto, web);
                            cursaNova.Estat = estat;
                            curses.Add(cursaNova);
                        }
                        return curses;
                    }
                }
            }
        }

        public static bool insertCursa(BDCursa c)
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
                            DBUtils.createParam(consulta, "nom", c.Nom, System.Data.DbType.String);
                            DBUtils.createParam(consulta, "data_inici", c.dataInici, System.Data.DbType.DateTime);
                            DBUtils.createParam(consulta, "data_fi", c.DataFi, System.Data.DbType.DateTime);
                            DBUtils.createParam(consulta, "lloc", c.Lloc, System.Data.DbType.String);
                            DBUtils.createParam(consulta, "esportId", c.EsportId, System.Data.DbType.Int32);
                            DBUtils.createParam(consulta, "estatId", c.EstatId, System.Data.DbType.Int32);
                            DBUtils.createParam(consulta, "descripcio", c.Descripcio, System.Data.DbType.String);
                            DBUtils.createParam(consulta, "limit_insc", c.LimitInscripcions, System.Data.DbType.Int32);
                            DBUtils.createParam(consulta, "url_foto", c.UrlFoto, System.Data.DbType.String);
                            DBUtils.createParam(consulta, "url_web", c.UrlWeb, System.Data.DbType.String);

                            consulta.CommandText = @"INSERT INTO curses (cur_nom,cur_data_inici,
                                                    cur_data_fi,cur_lloc,cur_esp_id,cur_est_id,
                                                    cur_desc,cur_limit_inscr,cur_foto,cur_web) 
                                                    VALUES (@nom, @data_inici,
                                                    @data_fi,
                                                    @lloc,
                                                    @esportId,
                                                    @estatId,
                                                    @descripcio,
                                                    @limit_insc,
                                                    @url_foto,
                                                    @url_web)";

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

        public static bool updateCursa(BDCursa c)
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

                        DBUtils.createParam(consulta, "nom", c.Nom, System.Data.DbType.String);
                        DBUtils.createParam(consulta, "data_inici", c.dataInici, System.Data.DbType.DateTime);
                        DBUtils.createParam(consulta, "data_fi", c.DataFi, System.Data.DbType.DateTime);
                        DBUtils.createParam(consulta, "lloc", c.Lloc, System.Data.DbType.String);
                        DBUtils.createParam(consulta, "esportId", c.EsportId, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "estatId", c.EstatId, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "descripcio", c.Descripcio, System.Data.DbType.String);
                        DBUtils.createParam(consulta, "limit_insc", c.LimitInscripcions, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "url_foto", c.UrlFoto, System.Data.DbType.String);
                        DBUtils.createParam(consulta, "url_web", c.UrlWeb, System.Data.DbType.String);
                        DBUtils.createParam(consulta, "id", c.Id, System.Data.DbType.Int32);

                        consulta.CommandText =
                            @"update curses set                             
                                cur_nom                  = @nom,
                                cur_data_inici           = @data_inici,
                                cur_data_fi              = @data_fi,
                                cur_lloc                 = @lloc,
                                cur_esp_id               = @esportId,
                                cur_est_id               = @estatId,
                                cur_desc                 = @descripcio,
                                cur_limit_inscr          = @limit_insc,
                                cur_foto                 = @url_foto,
                                cur_web                  = @url_web
                                where
                                    cur_id      = @id
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

        public static bool deleteCursa(int cursaId,BDCursa c)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();

                    // Comencem una transacció dins de la que volem executar updates
                    DbTransaction transaccio = connexio.BeginTransaction();
                    try
                    {
                        using (var consulta = connexio.CreateCommand())
                        {
                            consulta.Transaction = transaccio; // Associem la consulta a la transacció

                            DBUtils.createParam(consulta, "id", cursaId, System.Data.DbType.Int32);
                            consulta.CommandText = @"DELETE FROM curses WHERE cur_id = @id";

                            List<BDCircuit> circuitsCursa = BDCircuit.getCircuitsFromCursa(cursaId);
                            foreach (var circuit in circuitsCursa)
                            {
                                BDCircuitCategoria.deleteCircuitCategoria(circuit.Id);
                                BDCheckpoints.deleteCheckpoints(circuit.Id);
                            }
                            BDCircuit.deleteCircuits(cursaId);
                            int rowsAffected = consulta.ExecuteNonQuery();

                            if (rowsAffected != 1)
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
                    catch (Exception ex)
                    {
                        c.ErrorEliminar = ex.ToString();
                        transaccio.Rollback();
                        return false;
                    }

                }
            }


        }

        public static int getParticipantsCursa(int idCursa)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "idCursa", idCursa, System.Data.DbType.Int32);
                        consulta.CommandText = @"select total_participantes from vista_participants_per_cursa 
                                                where cur_id = @idCursa";

                        DbDataReader reader = consulta.ExecuteReader();
                        int numParticipants = 0;
                        while (reader.Read())
                        {
                           numParticipants = reader.GetInt32(reader.GetOrdinal("total_participantes"));
                        }
                        return numParticipants;
                    }
                }
            }
        }
    }
}
