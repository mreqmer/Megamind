using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT
{
    public enum ColorPisticha
    {
        NEGRO, BLANCO, NADA
    }
    public class Pisticha
    {
        #region ATRIBUTOS
        private ColorPisticha color;
        #endregion

        #region PROPIEDADES
        public ColorPisticha Color { get { return color; } set { color = value; } }
        #endregion

        #region CONSTRUCTORES
        public Pisticha()
        {
            Color = ColorPisticha.NADA;
        }
        public Pisticha(ColorPisticha color)
        {
            Color = color;
        }
        #endregion
    }
}
