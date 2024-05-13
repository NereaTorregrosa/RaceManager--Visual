using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_MySQL.Model
{
    public class BDCircuit
    {
        private int id;
        private int cursaId;
        private int numero;
        private double distancia;
        private string nom;
        private decimal preu;
        private DateTime tempsEstimat;
        private string categoria;

        public BDCircuit(int id, int cursaId, int numero, double distancia, string nom, decimal preu, DateTime tempsEstimat)
        {
            Id = id;
            CursaId = cursaId;
            Numero = numero;
            Distancia = distancia;
            Nom = nom;
            Preu = preu;
            TempsEstimat = tempsEstimat;
        }

        public BDCircuit(int cursaId, int numero, double distancia, string nom, decimal preu, DateTime tempsEstimat)
        {
            CursaId = cursaId;
            Numero = numero;
            Distancia = distancia;
            Nom = nom;
            Preu = preu;
            TempsEstimat = tempsEstimat;
        }

        public int Id { get => id; set => id = value; }
        public int CursaId { get => cursaId; set => cursaId = value; }
        public int Numero { get => numero; set => numero = value; }
        public double Distancia { get => distancia; set => distancia = value; }
        public string Nom { get => nom; set => nom = value; }
        public decimal Preu { get => preu; set => preu = value; }
        public DateTime TempsEstimat { get => tempsEstimat; set => tempsEstimat = value; }
        public string Categoria { get => categoria; set => categoria = value; }

        public static List<BDCircuit> getCircuitsFromCursa(int idCursa)
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        DBUtils.createParam(consulta, "id_cursa",idCursa, System.Data.DbType.Int32);
                        consulta.CommandText = @"select * from circuits where cir_cur_id = @id_cursa";
                        DbDataReader reader = consulta.ExecuteReader();
                        List<BDCircuit> circuits = new List<BDCircuit>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("cir_id"));
                            int cursaId = reader.GetInt32(reader.GetOrdinal("cir_cur_id"));
                            int numero = reader.GetInt32(reader.GetOrdinal("cir_num"));
                            double distancia = reader.GetDouble(reader.GetOrdinal("cir_distancia"));
                            string nom = reader.GetString(reader.GetOrdinal("cir_nom"));
                            decimal preu = reader.GetDecimal(reader.GetOrdinal("cir_preu"));
                            DateTime temps = reader.GetDateTime(reader.GetOrdinal("cir_temps_estimat"));
                            BDCircuit circuit = new BDCircuit(id, cursaId, numero, distancia, nom, preu, temps);
                            int idcategoria = BDCircuitCategoria.getCategoriaId(id);
                            if(idcategoria != 0)
                            {
                                string categoria = BDCategoria.getCategoriaById(idcategoria).Nom;
                                circuit.Categoria = categoria;
                            }
                            
                            circuits.Add(circuit);
                        }
                        return circuits;
                    }
                }
            }
        }

        public static bool insertCircuit(BDCircuit c)
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
                            DBUtils.createParam(consulta, "numero", c.Numero, System.Data.DbType.Int32);
                            DBUtils.createParam(consulta, "distancia", c.Distancia, System.Data.DbType.Double);
                            DBUtils.createParam(consulta, "cursaId", c.CursaId, System.Data.DbType.Int32);
                            DBUtils.createParam(consulta, "preu", c.Preu, System.Data.DbType.Decimal);
                            DBUtils.createParam(consulta, "tempsEstimat", c.TempsEstimat, System.Data.DbType.DateTime);

                            consulta.CommandText = @"INSERT INTO circuits (cir_cur_id,cir_num,
                                                    cir_distancia,cir_nom,cir_preu,cir_temps_estimat) 
                                                    VALUES (@cursaId, @numero,
                                                    @distancia,
                                                    @nom,
                                                    @preu,
                                                    @tempsEstimat)";

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

        public static int ObtenirUltimCircuitId()
        {
            using (var context = new MySqlDbContext())
            {
                using (var connexio = context.Database.GetDbConnection())
                {
                    connexio.Open();
                    using (var consulta = connexio.CreateCommand())
                    {
                        consulta.CommandText = @"SELECT cir_id FROM circuits ORDER BY cir_id DESC LIMIT 1";
                        int ultimCircuitId = -1;
                        object resultat = consulta.ExecuteScalar();
                        if (resultat != null && resultat != DBNull.Value)
                        {
                            ultimCircuitId = Convert.ToInt32(resultat);
                        }
                        return ultimCircuitId;
                    }
                }
            }
        }

        public static bool updateCircuit(BDCircuit c)
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
                        DBUtils.createParam(consulta, "cursaId", c.CursaId, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "numero", c.Numero, System.Data.DbType.Int32);
                        DBUtils.createParam(consulta, "distancia", c.Distancia, System.Data.DbType.Double);
                        DBUtils.createParam(consulta, "preu", c.Preu, System.Data.DbType.Decimal);
                        DBUtils.createParam(consulta, "tempsEstimat", c.TempsEstimat, System.Data.DbType.DateTime);
                        DBUtils.createParam(consulta, "id", c.Id, System.Data.DbType.Int32);

                        consulta.CommandText =
                            @"update circuits set                             
                                cir_cur_id            = @cursaId,
                                cir_num               = @numero,
                                cir_distancia         = @distancia,
                                cir_nom               = @nom,
                                cir_preu              = @preu,
                                cir_temps_estimat     = @tempsEstimat
                                where
                                    cir_id      = @id
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
    }
}
