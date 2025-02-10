using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT
{
    public enum ColorFicha
    {
        ROJO, NEGRO, BLANCO, AZUL, AMARILLO, VERDE, MORADO, ROSA, NADA
    }

    public class Ficha
    {
        #region ATRIBUTOS
        private ColorFicha color;
        #endregion

        #region PROPIEDADES
        public ColorFicha Color { get { return color; } set { color = value; } }
        #endregion

        #region CONSTRUCTORES
        public Ficha()
        {
            Color = ColorFicha.NADA;
        }
        public Ficha(ColorFicha color)
        {
            Color = color;
        }
        #endregion
    }
}
