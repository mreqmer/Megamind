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
using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel;

namespace MegamindMAUI.VM
{

    public class VMJuego : ClsVMBase
    {
        //TODO: Implementar propiedades del juego
        #region ATRIBUTOS
        private ObservableCollection<ModelFila> filasJuego = new ObservableCollection<ModelFila>();
        private ObservableCollection<Ficha> tablero = new ObservableCollection<Ficha>();
        private ObservableCollection<Ficha> combinacion = new ObservableCollection<Ficha>();
        private Ficha colorSeleccionado;
        private Ficha ficha;
        private int ronda = 0;
        private DelegateCommand btnJugarCommand;
        private Ficha fichaACambiar;
        private int resuelto = 0;

        #endregion

        #region PROPIEDADES
        public ObservableCollection<ModelFila> FilasJuego { get { return filasJuego; } set { filasJuego = value; BtnJugarCommand.RaiseCanExecuteChanged(); } }
        public ObservableCollection<Ficha> Tablero { get { return tablero; } }
        public ObservableCollection<Ficha> Combinacion { get { return combinacion;} set { combinacion = value; } }
        public Ficha ColorSeleccionado { get { return colorSeleccionado; } set { colorSeleccionado = value; OnPropertyChanged(nameof(ColorSeleccionado)); } }
        public int Ronda { get { return ronda; } set { ronda = value; } }
        public DelegateCommand BtnJugarCommand { get { return btnJugarCommand; } }

        public Ficha Ficha { get { return ficha; } set { ficha = colorSeleccionado; OnPropertyChanged(nameof(Ficha)); } }
        public Ficha FichaACambiar { get { return fichaACambiar; } set { fichaACambiar = value; OnPropertyChanged("FichaACambiar"); cambiaficha(fichaACambiar); } }
        public int Resuelto { get { return resuelto; } set { resuelto = value; } }
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
            bool bandera = false;
            if (filasJuego[ronda].Juego[0].FichaColor != "nada.png" && filasJuego[ronda].Juego[1].FichaColor != "nada.png" 
                && filasJuego[ronda].Juego[2].FichaColor != "nada.png" && filasJuego[ronda].Juego[3].FichaColor != "nada.png")
            {
                bandera = true;
            }
            return bandera;
        }

        private async void btnJugarCommandExecute()
        {
            cambiaPistichaPropia();

            compruebaResultado();

            mandaAlResultado();

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



        private async Task inicializaCombinacion()
        {
            List<int> solucion = new List<int>();
            await global.InicializaConexion();
            MainThread.BeginInvokeOnMainThread(
                async () =>
                {
                    await MegamindMAUI.Model.global.connection.InvokeAsync("UneSala", "aaa");
                }
            );
            MainThread.BeginInvokeOnMainThread(
                async () =>
                {
                    await MegamindMAUI.Model.global.connection.InvokeAsync("MandaSolucion", "aaa");
                }
            );
            MegamindMAUI.Model.global.connection.On<List<int>>("RecibeSolucion", (solucion) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    combinacion.Clear();
                    foreach (var num in solucion)
                    {
                        combinacion.Add(new Ficha(num));
                    }
                    OnPropertyChanged(nameof(Combinacion));
                });
            });
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

        private void cambiaficha(Ficha ficha)
        {
            int index = filasJuego[ronda].Juego.IndexOf(ficha);
            filasJuego[ronda].Juego[index].FichaColor = colorSeleccionado.FichaColor;
            BtnJugarCommand.RaiseCanExecuteChanged();
        }


        private void cambiaPistichaPropia()
        {
            for (int i = 0; i < 4; i++)
            {
                if (filasJuego[ronda].Juego[i].FichaColor == combinacion[i].FichaColor)
                {
                    // Si est� en la posici�n correcta, asignamos "Rojo"
                    filasJuego[ronda].PistaPropia[i] = new Pisticha("Rojo");
                }
                else if (combinacion.Any(ficha => ficha.FichaColor == filasJuego[ronda].Juego[i].FichaColor))
                {
                    // Si est� en la combinaci�n pero en otra posici�n, asignamos "Blanco"
                    filasJuego[ronda].PistaPropia[i] = new Pisticha("Blanco");
                }
                else
                {
                    // Si no est� en la combinaci�n, asignamos "Nada"
                    filasJuego[ronda].PistaPropia[i] = new Pisticha("Nada");
                }
            }

            filasJuego[ronda].PistaPropia = new ObservableCollection<Pisticha>(filasJuego[ronda].PistaPropia.OrderBy(p => p));

        }

        public async void mandaAlResultado()
        {
            if (resuelto == 1)
            {
                await Shell.Current.GoToAsync("///Final");
            }
        }

        private void compruebaResultado()
        {
            if (filasJuego[ronda].PistaPropia[1].FichaColor == "Rojo.png" && filasJuego[ronda].PistaPropia[1].FichaColor == "Rojo.png"
                && filasJuego[ronda].PistaPropia[1].FichaColor == "Rojo.png" && filasJuego[ronda].PistaPropia[1].FichaColor == "Rojo.png")
            {
                MainThread.BeginInvokeOnMainThread(
                    async () =>
                    {
                        await MegamindMAUI.Model.global.connection.InvokeAsync("Terminado", "aaa");
                    }
                );
                MegamindMAUI.Model.global.connection.On("Espera", () =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        resuelto++;
                    });

                });
            }
        }

        #endregion
    }
}
