using ENT;
using MegamindMAUI.VM.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using Servidor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegamindMAUI.VM
{
    [QueryProperty(nameof(NombreJugador), "NombreJugador")]
    public class VMSalas : ClsVMBase
    {

        #region PROPIEDADES
        //TODO borrar esto cuando se implemente la sala
        private string nombreJugador;
        private ObservableCollection<Sala> salas = new ObservableCollection<Sala>();

        private DelegateCommand btnUnirseSalaCommand;
        private Sala salaSeleccionada;
        #endregion

        #region ATRIBUTOS
        public ObservableCollection<Sala> Salas { get { return salas; } set { salas = value; } }
        public DelegateCommand BtnUnirseSalaCommand { get {  return btnUnirseSalaCommand; } }
        public string NombreJugador { get { return nombreJugador; } set { nombreJugador = value; } }
        public Sala SalaSeleccionada { get { return salaSeleccionada; } set { salaSeleccionada = value; OnPropertyChanged(nameof(SalaSeleccionada)); } }

        #endregion

        #region CONSTRUCTORES
        public VMSalas()
        {

            btnUnirseSalaCommand = new DelegateCommand(btnUnirseSalaCommand_Execute);
            inicializa();

        }
        #endregion

        #region COMMANDS

        public async void btnUnirseSalaCommand_Execute()
        {
            bool bandera = false;
            Console.WriteLine("Texto o valor a mostrar");

            Jugador jugador = new Jugador(NombreJugador, salaSeleccionada.NombreSala, 0);
            MainThread.BeginInvokeOnMainThread(
               async () =>
               {
                   await MegamindMAUI.Model.global.connection.InvokeAsync("UneSala", salaSeleccionada.NombreSala, jugador);
               }
           );
            MegamindMAUI.Model.global.connection.On<bool>("SalaUnida", (unido) =>
            {
                if (unido)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {

                        gotoJuego();

                    });
                }
                
            });

          

        }

        

        #endregion

        #region METODOS

        public void inicializa()
        {
            MainThread.BeginInvokeOnMainThread(
               async () =>
               {
                   await MegamindMAUI.Model.global.connection.InvokeAsync("MandaSalas");
               }
           );
            MegamindMAUI.Model.global.connection.On<List<Sala>>("RecibeSalas", (solucion) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    salas.Clear();
                    foreach (var s in solucion)
                    {
                        salas.Add(new Sala(s));
                    }
                   
                });
            });
        }

        public async void gotoJuego()
        {
            Jugador jugador = new Jugador(NombreJugador, salaSeleccionada.NombreSala, 0);
            var queryParams = new Dictionary<string, object>
                    {
                    { "jugador", jugador }
                    };
            await Shell.Current.GoToAsync("///Megamind", queryParams);
        }



        #endregion


    }
}
