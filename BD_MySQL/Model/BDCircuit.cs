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
        private TimeSpan tempsEstimat;
        private string categoria;

        public BDCircuit(int id, int cursaId, int numero, double distancia, string nom, decimal preu, TimeSpan tempsEstimat)
        {
            Id = id;
            CursaId = cursaId;
            Numero = numero;
            Distancia = distancia;
            Nom = nom;
            Preu = preu;
            TempsEstimat = tempsEstimat;
        }

        public BDCircuit(int cursaId, int numero, double distancia, string nom, decimal preu, TimeSpan tempsEstimat)
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
        public TimeSpan TempsEstimat { get => tempsEstimat; set => tempsEstimat = value; }
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
                            TimeSpan tempsEstimat = temps.TimeOfDay;
                            BDCircuit circuit = new BDCircuit(id, cursaId, numero, distancia, nom, preu, tempsEstimat);
                            int idcategoria = BDCircuitCategoria.getCategoriaId(id);
                            string categoria = BDCategoria.getCategoriaById(idcategoria);
                            circuit.Categoria = categoria;
                            circuits.Add(circuit);
                        }
                        return circuits;
                    }
                }
            }
        }

        public bool insertCircuit(int idCursa)
        {
            return false;
        }
    }
}
