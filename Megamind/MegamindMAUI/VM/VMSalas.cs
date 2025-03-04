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
    public class VMSalas
    {

        #region PROPIEDADES
        //TODO borrar esto cuando se implemente la sala
        private string nombreJugador;
        private List<Sala> salas = new List<Sala>();
        //{
        //    new Jugador("Jugador 1", "1", 10),
        //    new Jugador("Jugador 2", "2", 20),
        //    new Jugador("Jugador 3", "3", 30),
        //    new Jugador("Jugador 4", "4", 40),
        //    new Jugador("Jugador 5", "5", 50),
        //    new Jugador("Jugador 6", "6", 60),
        //    new Jugador("Jugador 7", "7", 70),
        //    new Jugador("Jugador 8", "8", 80),
        //    new Jugador("Jugador 9", "9", 90),
        //    new Jugador("Jugador 10", "10", 100)
        //};

        private DelegateCommand btnUnirseSalaCommand;
        #endregion

        #region ATRIBUTOS
        public List<Sala> Salas { get { return salas; } set { salas = value; } }
        public DelegateCommand BtnUnirseSalaCommand { get {  return btnUnirseSalaCommand; } }
        public string NombreJugador { get { return nombreJugador; } set { nombreJugador = value; } }

        #endregion

        #region CONSTRUCTORES
        public VMSalas()
        {

            btnUnirseSalaCommand = new DelegateCommand(btnUnirseaSalaCommand_Execute);
        }
        #endregion

        #region COMMANDS

        public void btnUnirseaSalaCommand_Execute()
        {
            //TODO logica de la nueva sala
            Jugador jugador = new Jugador(NombreJugador, "", 0);
            MainThread.BeginInvokeOnMainThread(
                async () =>
                {
                    await MegamindMAUI.Model.global.connection.InvokeAsync("UneSala", Salas[0]);
                }
            );
        }

        #endregion


    }
}
