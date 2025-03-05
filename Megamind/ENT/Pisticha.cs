using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT
{
    public class Pisticha : IComparable<Pisticha>
    {
        #region ATRIBUTOS
        private List<string> pistichaColores = new List<string>{
            "rojo", "blanco", "nada"
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
            if (pistichaColores.Contains(color.ToLower()))
            {
                pistichaColor = color + ".png";
            }
            else
            {
                pistichaColor = "nada.png";
            }
        }

        public int CompareTo(Pisticha? other)
        {
            if (other == null) return 1;

            // Definir prioridad de colores
            Dictionary<string, int> prioridad = new Dictionary<string, int>
            {
                { "rojo.png", 1 },
                { "blanco.png", 2 },
                { "nada.png", 3 }
            };

            int thisPriority = prioridad[pistichaColor.ToLower()];
            int otherPriority = prioridad[other.pistichaColor.ToLower()];

            // Comparar según prioridad
            return thisPriority.CompareTo(otherPriority);
        }


        #endregion
    }
}
