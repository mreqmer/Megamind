using ENT;
using MegamindMAUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegamindMAUI.VM.Utils;

namespace MegamindMAUI.VM
{
    public class VMJuego
    {
        //TODO: Implementar propiedades del juego
        #region ATRIBUTOS
        private ObservableCollection<ModelFila> filasJuego = new ObservableCollection<ModelFila>
        {
            new ModelFila(true),
            new ModelFila(),
            new ModelFila(),
            new ModelFila(),
            new ModelFila(),
            new ModelFila(),
            new ModelFila(),
            new ModelFila(),
            new ModelFila(),
            new ModelFila()
        };
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
        private string colorSeleccionado="nada";
        private DelegateCommand btnJugarCommand;
        private int ronda = 0;
        #endregion

        #region PROPIEDADES
        public ObservableCollection<ModelFila> FilasJuego { get { return filasJuego; } set { filasJuego = value; } }
        public ObservableCollection<Ficha> Tablero { get { return tablero; } }
        public ObservableCollection<Ficha> Combinacion { get { return combinacion;} set { combinacion = value; } }
        public string ColorSeleccionado { get { return colorSeleccionado; } set { colorSeleccionado = value; } }
        public DelegateCommand BtnJugarCommand { get { return btnJugarCommand; } }
        public int Ronda { get { return ronda; } set { ronda = value; } }
        #endregion

        #region CONSTRUCTORES

        public VMJuego()
        {

            btnJugarCommand = new DelegateCommand(btnJugarCommandExecute, btnJugarCommandCanExecute);
        }

        #endregion

        #region COMMAND

        private bool btnJugarCommandCanExecute()
        {
<<<<<<< HEAD
            //TODO
            return true;
=======
            throw new NotImplementedException();
>>>>>>> 98be680 (ENT Jugador, Y Actualizacion VMJuego)
        }

        private async void btnJugarCommandExecute()
        {
            filasJuego[ronda].EsJugable = false;
        }
        #endregion
    }
}
