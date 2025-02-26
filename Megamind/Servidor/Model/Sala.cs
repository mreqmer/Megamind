using ENT;
using System.Collections.ObjectModel;

namespace Servidor.Model
{
    public class Sala
    {
        #region ATRIBUTOS
        private int id;
        private Jugador jugador1;
        private Jugador jugador2;
        #endregion

        #region PROPIEDADES
        public int Id { get { return id; } set { id = value; } }
        public Jugador Jugador1 { get { return jugador1; } set { jugador1 = value; } }
        public Jugador Jugador2 { get { return jugador2; } set { jugador2 = value; } }
        #endregion

        #region CONSTRUCTORES
        public Sala()
        {

        }
        public Sala(int id, Jugador jugador1)
        {
            this.id = id;
            this.jugador1 = jugador1;
        }

        public Sala(int id, Jugador jugador1, Jugador jugador2)
        {
            this.id = id;
            this.jugador1 = jugador1;
            this.jugador2 = jugador2;
        }

        public Sala(Sala sala)
        {
            this.id = sala.id;
            this.jugador1 = sala.jugador1;
            this.jugador2 = sala.jugador2;
        }
        #endregion
    }
}
