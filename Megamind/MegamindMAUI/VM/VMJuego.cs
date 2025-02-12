using ENT;
using MegamindMAUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegamindMAUI.VM
{
    public class VMJuego
    {
        //TODO: Implementar propiedades del juego
        #region ATRIBUTOS
        private ObservableCollection<ModelFila> filasJuego;
        private ObservableCollection<Ficha> tablero = new ObservableCollection<Ficha>
        {
            new Ficha("negro"),
            new Ficha("blanco"),
            new Ficha("rojo"),
            new Ficha("azul"),
            new Ficha("amarillo"),
            new Ficha("morado"),
            new Ficha("verde"),
            new Ficha("rosa")
        };
        private ObservableCollection<Ficha> combinacion = new ObservableCollection<Ficha>
        {
            new Ficha("nada"),
            new Ficha("nada"),
            new Ficha("nada"),
            new Ficha("nada")
        };

    #endregion

    #region PROPIEDADES
    public ObservableCollection<ModelFila> FilasJuego { get { return filasJuego; } set { filasJuego = value; } }
        public ObservableCollection<Ficha> Tablero { get { return tablero; } }
        public ObservableCollection<Ficha> Combinacion { get { return combinacion;} set { combinacion = value; } }

        #endregion

        #region CONSTRUCTORES

        public VMJuego()
        {

        }

        #endregion
    }
}
