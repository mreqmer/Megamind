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
    [QueryProperty(nameof(Jugador), "jugador")]
    public class VMJuego : ClsVMBase
    {
        //TODO: Implementar propiedades del juego
        #region ATRIBUTOS
        private ObservableCollection<ModelFila> filasJuego = new ObservableCollection<ModelFila>();
        private ObservableCollection<Ficha> tablero = new ObservableCollection<Ficha>();
        private ObservableCollection<Ficha> combinacionVisible = new ObservableCollection<Ficha> { new Ficha("nada"), new Ficha("nada"), new Ficha("nada"), new Ficha("nada") };
        private ObservableCollection<Ficha> combinacion = new ObservableCollection<Ficha>();
        private Ficha colorSeleccionado;
        private Ficha ficha;
        private int ronda = 0;
        private DelegateCommand btnJugarCommand;
        private Ficha fichaACambiar;
        private int resuelto = 0;
        //private Jugador jugador = new Jugador("Pedro" ,"aaa", 0);
        private Jugador jugador = new Jugador();

        #endregion

        #region PROPIEDADES
        public ObservableCollection<ModelFila> FilasJuego { get { return filasJuego; } set { filasJuego = value; BtnJugarCommand.RaiseCanExecuteChanged(); } }
        public ObservableCollection<Ficha> Tablero { get { return tablero; } }
        public ObservableCollection<Ficha> Combinacion { get { return combinacion;} set { combinacion = value; } }
        public ObservableCollection<Ficha> CombinacionVisible { get { return combinacionVisible; }  }
        public Ficha ColorSeleccionado { get { return colorSeleccionado; } set { colorSeleccionado = value; OnPropertyChanged(nameof(ColorSeleccionado)); } }
        public int Ronda { get { return ronda; } set { ronda = value; } }
        public DelegateCommand BtnJugarCommand { get { return btnJugarCommand; } }

        public Ficha Ficha { get { return ficha; } set { ficha = colorSeleccionado; OnPropertyChanged(nameof(Ficha)); } }
        public Ficha FichaACambiar { get { return fichaACambiar; } set { fichaACambiar = value; OnPropertyChanged("FichaACambiar"); cambiaficha(fichaACambiar); } }
        public int Resuelto { get { return resuelto; } set { resuelto = value; } }
        public Jugador Jugador { get { return jugador; } set { jugador = value;
                OnJugadorCargado(); } }
        #endregion

        #region CONSTRUCTORES

        public VMJuego()
        {
            coloresDisponibles();
            inicializaFilasJuego();

            MegamindMAUI.Model.global.connection.On<ObservableCollection<Pisticha>, int>("RecibePisticha", (pistaRecibida, rondaRival) =>
            {
                filasJuego[rondaRival].PistaRival = pistaRecibida;
            });

            MegamindMAUI.Model.global.connection.On("Espera", () =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    resuelto++;
                    mandaAlResultado();
                });
            });

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
            trabajaPisticha();

            bool ganado = await compruebaResultado();

            if (!ganado)
            {
                ronda++;
                calculaRondaJugable();
            }
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


        private async void OnJugadorCargado()
        {
            // Asegurémonos de que el jugador tenga un valor antes de continuar
            if (jugador != null)
            {
                // Lógica que depende del jugador, por ejemplo, unirse a la sala y obtener la solución
                await inicializaCombinacion();
            }
        }
        private async Task inicializaCombinacion()
        {
            List<int> solucion = new List<int>();
            
            MainThread.BeginInvokeOnMainThread(
                async () =>
                {
                    await MegamindMAUI.Model.global.connection.InvokeAsync("MandaSolucion", jugador.Sala);
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
            if (colorSeleccionado != null)
            {
                filasJuego[ronda].Juego[index].FichaColor = colorSeleccionado.FichaColor;
            }
            BtnJugarCommand.RaiseCanExecuteChanged();
        }


        private async void trabajaPisticha()
        {
            var pistas = new List<Pisticha>();

            var combinacionRestante = combinacion.ToList();
            var filaActual = filasJuego[ronda].Juego.ToList();

            // Primera pasada: Buscar fichas negras (colores correctos en la posición correcta)
            for (int i = 0; i < 4; i++)
            {
                if (filaActual[i].FichaColor == combinacionRestante[i].FichaColor)
                {
                    pistas.Add(new Pisticha("Rojo")); // Ficha negra (posición correcta)
                    filaActual[i] = null; // Marcar como procesado
                    combinacionRestante[i] = null; // Marcar como procesado
                }
            }

            // Segunda pasada: Buscar fichas blancas (colores correctos en la posición incorrecta)
            for (int i = 0; i < 4; i++)
            {
                if (filaActual[i] != null) // Si no ha sido procesado
                {
                    int index = combinacionRestante.FindIndex(f => f != null && f.FichaColor == filaActual[i].FichaColor);
                    if (index != -1)
                    {
                        pistas.Add(new Pisticha("Blanco")); // Ficha blanca (color correcto, posición incorrecta)
                        combinacionRestante[index] = null; // Marcar como procesado
                    }
                }
            }

            // Rellenar con "Nada" si no hay suficientes pistas
            while (pistas.Count < 4)
            {
                pistas.Add(new Pisticha("Nada"));
            }

            // Asignar las pistas a la fila actual
            filasJuego[ronda].PistaPropia = new ObservableCollection<Pisticha>(pistas);

            // Enviar las pistas al servidor
            await MegamindMAUI.Model.global.connection.InvokeAsync("MandaPisticha", jugador.Sala, filasJuego[ronda].PistaPropia, ronda);

        }

        public async Task mandaAlResultado()
        {
            if (resuelto == 2)
            {
                var queryParams = new Dictionary<string, object>
                 {
                 { "Jugador", jugador }
                 };

                await Shell.Current.GoToAsync("///Final", queryParams);
            }
        }

        private async Task<bool> compruebaResultado()
        {
            bool terminado = false;

            if ((filasJuego[ronda].PistaPropia[0].FichaColor == "Rojo.png" && filasJuego[ronda].PistaPropia[1].FichaColor == "Rojo.png"
                && filasJuego[ronda].PistaPropia[2].FichaColor == "Rojo.png" && filasJuego[ronda].PistaPropia[3].FichaColor == "Rojo.png") || ronda == 9)
            {
                foreach (ModelFila f in filasJuego)
                {
                    f.EsJugable = false;
                    OnPropertyChanged(nameof(FilasJuego));
                }

                MainThread.BeginInvokeOnMainThread(
                    async () =>
                    {
                        
                        Jugador auxiliar = jugador;
                        auxiliar.Puntuacion = ronda;
                        await MegamindMAUI.Model.global.connection.InvokeAsync("Terminado", auxiliar);
                       
                    }
                );
                terminado = true;
            }

            return terminado;
        }

        #endregion
    }
}
