using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT
{
    public class Pisticha
    {
        #region ATRIBUTOS
        private List<string> pistichaColores = new List<string>{
            "rojo", "negro", "azul", "amarillo",
            "rosa", "verde", "blanco", "morado", "nada"
        };
        private String pistichaColor;
        #endregion

        #region PROPIEDADES
        public List<string> Colores { get { return pistichaColores; } }
        public String FichaColor { get { return pistichaColor; } set { pistichaColor = value; } }
        #endregion

        #region CONSTRUCTORES
        public Pisticha()
        {
            pistichaColor = "nada.png";
        }

        public Pisticha(string color)
        {
            if (pistichaColores.Contains(color))
            {
                pistichaColor = color + ".png";
            }
            else
            {
                pistichaColor = "nada.png";
            }
        }
        #endregion
    }
}
