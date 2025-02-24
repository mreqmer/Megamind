using ENT;
using MegamindMAUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegamindMAUI.VM.Utils;
using System.Diagnostics;

namespace MegamindMAUI.VM
{
    public class VMJuego : ClsVMBase
    {
        //TODO: Implementar propiedades del juego
        #region ATRIBUTOS
        private ObservableCollection<ModelFila> filasJuego = new ObservableCollection<ModelFila>();
        private ObservableCollection<Ficha> tablero = new ObservableCollection<Ficha>();
        private ObservableCollection<Ficha> combinacion = new ObservableCollection<Ficha>();
        private string colorSeleccionado="nada";
        private Ficha ficha = new Ficha();
        private int ronda = 0;
        private DelegateCommand btnJugarCommand;
        #endregion

        #region PROPIEDADES
        public ObservableCollection<ModelFila> FilasJuego { get { return filasJuego; } set { filasJuego = value; } }
        public ObservableCollection<Ficha> Tablero { get { return tablero; } }
        public ObservableCollection<Ficha> Combinacion { get { return combinacion;} set { combinacion = value; } }
        public string ColorSeleccionado { get { return colorSeleccionado; } set { colorSeleccionado = value; } }
        public int Ronda { get { return ronda; } set { ronda = value; } }
        public DelegateCommand BtnJugarCommand { get { return btnJugarCommand; } }

        //public Ficha Ficha { get { return ficha; } set { ficha. = colorSeleccionado; } }
        public Object var { set { var = value; } }
        #endregion

        #region CONSTRUCTORES

        public VMJuego()
        {
            coloresDisponibles();
            inicializaFilasJuego();
            inicializaCombinacion();
            calculaRondaJugable();
            btnJugarCommand = new DelegateCommand(btnJugarCommandExecute, btnJugarCommandCanExecute);
        }

        #endregion

        #region COMMAND

        private bool btnJugarCommandCanExecute()
        {
            //bool bandera = false;

            //if (filasJuego[Ronda].Juego[0].FichaColor!="nada" && filasJuego[Ronda].Juego[1].FichaColor != "nada" && filasJuego[Ronda].Juego[2].FichaColor != "nada" && filasJuego[Ronda].Juego[3].FichaColor != "nada")
            //{
            //    bandera = true;
            //}

            //return bandera;
            return true;
        }

        private async void btnJugarCommandExecute()
        {
            ronda++;
            calculaRondaJugable();
        }

        #endregion

        #region INICIALIZA
        private void coloresDisponibles()
        {
            tablero.Clear();
            tablero = new ObservableCollection<Ficha>
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
        }

        private void inicializaFilasJuego()
        {
            filasJuego.Clear();
            filasJuego = new ObservableCollection<ModelFila>
            {
                new ModelFila(0),
                new ModelFila(1),
                new ModelFila(2),
                new ModelFila(3),
                new ModelFila(4),
                new ModelFila(5),
                new ModelFila(6),
                new ModelFila(7),
                new ModelFila(8),
                new ModelFila(9)
            };
        }

        private void inicializaCombinacion()
        {
            combinacion.Clear();
            combinacion = new ObservableCollection<Ficha>
            {
                new Ficha("nada"),
                new Ficha("nada"),
                new Ficha("nada"),
                new Ficha("nada")
            };
        }
        #endregion

        #region METODOS
        private void calculaRondaJugable()
        {
            foreach (ModelFila fila in filasJuego)
            {
                if (ronda == fila.Id)
                {
                    fila.EsJugable = true;
                } else {fila.EsJugable = false; }
                OnPropertyChanged("FilasJuego");
            }
        }
        #endregion
    }
}
