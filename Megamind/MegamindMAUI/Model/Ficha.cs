using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MegamindMAUI.Model.Utils;

namespace ENT
{

    public class Ficha : ClsVMBase
    {
        #region ATRIBUTOS
        private List<string> colores = new List<string>{
            "rojo", "negro", "azul", "amarillo",
            "rosa", "verde", "blanco", "morado", "nada"
        };
        private String fichaColor;
        #endregion

        #region PROPIEDADES
        public List<string> Colores { get { return colores; } }
        public String FichaColor { get { return fichaColor; } set { fichaColor = value; OnPropertyChanged("FichaColor"); } }
        #endregion

        #region CONSTRUCTORES
        public Ficha()
        {
            fichaColor = "nada.png";
        }

        public Ficha(string color)
        {
            if (colores.Contains(color))
            {
                fichaColor = color + ".png";
            }
            else
            {
                fichaColor = "nada.png";
            }
        }

        public Ficha(int color)
        {
            if (color >= 0 && color < colores.Count)
            {
                fichaColor = $"{colores[color]}.png";
            }
            else
            {
                fichaColor = "nada.png";
            }
        }

        #endregion
    }
}
