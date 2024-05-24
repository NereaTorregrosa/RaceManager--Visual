using System;
using System.Collections.Generic;
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
    }
}
