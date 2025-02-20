using ENT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegamindMAUI.Model
{
    public class ModelFila
    {
        #region ATRIBUTOS
        private int id;
        private ObservableCollection<Ficha> juego = new ObservableCollection<Ficha>
        {
            new Ficha("nada"),
            new Ficha("nada"),
            new Ficha("nada"),
            new Ficha("nada")
        };
        private ObservableCollection<Pisticha> pistaPropia = new ObservableCollection<Pisticha>
        {
            new Pisticha("nada"),
            new Pisticha("nada"),
            new Pisticha("nada"),
            new Pisticha("nada")
        };
        private ObservableCollection<Pisticha> pistaRival = new ObservableCollection<Pisticha>
        {
            new Pisticha("nada"),
            new Pisticha("nada"),
            new Pisticha("nada"),
            new Pisticha("nada")
        };
        private bool esJugable = false;
        private bool esPistaVisible = true;
        #endregion

        #region PROPIEDADES
        public int Id { get {  return id; } set { id = value; } }
        public ObservableCollection<Ficha> Juego { get { return juego; } set { juego = value; } }
        public ObservableCollection<Pisticha > PistaPropia { get { return pistaPropia; } set { pistaPropia = value; } }
        public ObservableCollection<Pisticha> PistaRival { get { return pistaRival; } set { pistaRival = value; } }
        public bool EsJugable 
        { 
            get 
            {
                return esJugable; 
            } set 
            {
                esJugable = value;
                if (esJugable)
                {
                    esPistaVisible = false;
                }
                else
                {
                    esPistaVisible = true;
                }
            }
        }
        public bool EsPistaVisible { get { return esPistaVisible; } }
        #endregion

        #region CONSTRUCTORES
        public ModelFila() 
        {

        }
        public ModelFila(bool esJugable)
        {
            this.esJugable = esJugable;
        }
        public ModelFila(int id, ObservableCollection<Ficha> juego, ObservableCollection<Pisticha> pistaPropia, ObservableCollection<Pisticha> pistaRival)
        {
            this.id = id;
            this.juego = juego;
            this.pistaPropia = pistaPropia;
            this.pistaRival = pistaRival;
        }
        #endregion
    }
}
