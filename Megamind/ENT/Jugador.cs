using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT
{
    public class Jugador
    {
        #region ATRIBUTOS
        private String nombre;
        private String sala = "0";
        private int puntuacion = 0;
        #endregion

        #region PROPIEDADES
        public String Nombre { get { return nombre; } set { nombre = value; } }
        public String Sala { get { return sala; } set { sala = value; } }
        public int Puntuacion { get { return puntuacion; } set { puntuacion = value; } }
        #endregion

        #region CONSTRUCTORES
        public Jugador()
        {
        }

        public Jugador(String nombre, String sala, int puntuacion)
        {
            this.nombre = nombre;
            this.sala = sala;
            this.puntuacion = puntuacion;
        }

        public Jugador(Jugador jug)
        {
            this.nombre = jug.nombre;
            this.sala = jug.sala;
            this.puntuacion = jug.puntuacion;
        }
        #endregion
    }
}
