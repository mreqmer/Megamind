using ENT;
using MegamindMAUI.VM.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegamindMAUI.VM
{
    public class VMSalas
    {

        #region PROPIEDADES
        //TODO borrar esto cuando se implemente la sala
        private List<Jugador> salas = new List<Jugador>
        {
            new Jugador("Jugador 1", 1, 10),
            new Jugador("Jugador 2", 2, 20),
            new Jugador("Jugador 3", 3, 30),
            new Jugador("Jugador 4", 4, 40),
            new Jugador("Jugador 5", 5, 50),
            new Jugador("Jugador 6", 6, 60),
            new Jugador("Jugador 7", 7, 70),
            new Jugador("Jugador 8", 8, 80),
            new Jugador("Jugador 9", 9, 90),
            new Jugador("Jugador 10", 10, 100)
        };

        private DelegateCommand btnNuevaSalaCommand;
        #endregion

        #region ATRIBUTOS
        public List<Jugador> Salas { get { return salas; } set { salas = value; } }
        public DelegateCommand BtnNuevaSalaCommand { get {  return btnNuevaSalaCommand; } }
        #endregion

        #region CONSTRUCTORES
        public VMSalas()
        {
            btnNuevaSalaCommand = new DelegateCommand(btnNuevaSalaCommand_Execute);
        }
        #endregion

        #region COMMANDS

        public void btnNuevaSalaCommand_Execute()
        {
            //TODO logica de la nueva sala
        }

        #endregion


    }
}
