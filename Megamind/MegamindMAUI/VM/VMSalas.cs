using ENT;
using MegamindMAUI.VM.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using Servidor.Model;
using System;
using System.Collections.Generic;
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
        private List<Sala> salas = new List<Sala>();


        private DelegateCommand btnUnirseSalaCommand;
        private Sala salaSeleccionada;
        #endregion

        #region ATRIBUTOS
        public List<Sala> Salas { get { return salas; } set { salas = value; } }
        public DelegateCommand BtnUnirseSalaCommand { get {  return btnUnirseSalaCommand; } }
        public string NombreJugador { get { return nombreJugador; } set { nombreJugador = value; } }
        public Sala SalaSeleccionada { get { return salaSeleccionada; } set { salaSeleccionada = value; OnPropertyChanged(nameof(SalaSeleccionada)); } }

        #endregion

        #region CONSTRUCTORES
        public VMSalas()
        {

            btnUnirseSalaCommand = new DelegateCommand(btnUnirseSalaCommand_Execute);

        }
        #endregion

        #region COMMANDS

        public void btnUnirseSalaCommand_Execute()
        {
            Console.WriteLine("Texto o valor a mostrar");

            Jugador jugador = new Jugador(NombreJugador, salaSeleccionada.NombreSala, 0);
            MainThread.BeginInvokeOnMainThread(
               async () =>
               {
                   await MegamindMAUI.Model.global.connection.InvokeAsync("UneSala", salaSeleccionada.NombreSala);
               }
           );
        }

        #endregion

        #region METODOS

        public void inicializa()
        {
            MainThread.BeginInvokeOnMainThread(
               async () =>
               {
                   await MegamindMAUI.Model.global.connection.InvokeAsync("MandaSalas", salas);
               }
           );
        }

        #endregion


    }
}
