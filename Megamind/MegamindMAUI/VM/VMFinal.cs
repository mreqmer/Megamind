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
    [QueryProperty(nameof(Jugador), "Jugador")]
    public class VMFinal : ClsVMBase
    {
        #region ATRIBUTOS
        private string ganadorNombre = "";
        private string ganadorPuntuacion = "";
        private string perdedorNombre = "";
        private string perdedorPuntuacion = "";
        private string sala = "";
        private string resultado1;
        private string resultado2;
        private DelegateCommand btnVolverCommand;
        private Jugador jugador;
        #endregion

        #region PROPIEDADES
        public string GanadorNombre { get { return ganadorNombre; } set { ganadorNombre = value; OnPropertyChanged(nameof(GanadorNombre)); } }
        public string GanadorPuntuacion { get { return ganadorPuntuacion; } set { ganadorPuntuacion = value; OnPropertyChanged(nameof(GanadorPuntuacion)); } }
        public string PerdedorNombre { get { return perdedorNombre; } set { perdedorNombre = value; OnPropertyChanged(nameof(PerdedorNombre)); } }
        public string PerdedorPuntuacion { get { return perdedorPuntuacion; } set { perdedorPuntuacion = value; OnPropertyChanged(nameof(PerdedorPuntuacion)); } }
        public string Sala { get { return sala; } set { sala = value;  OnPropertyChanged(nameof(Sala)); } }
        public string Resultado1 { get { return resultado1; } set { resultado1 = value; OnPropertyChanged(nameof(Resultado1)); } }
        public string Resultado2 { get { return resultado2; } set { resultado2 = value; OnPropertyChanged(nameof(Resultado2)); } }
        public DelegateCommand BtnVolverCommand { get { return btnVolverCommand; } }
        public Jugador Jugador { get { return jugador; } set { jugador = value; Inicializa(); OnPropertyChanged(nameof(Jugador)); }  }
        #endregion

        #region CONSTRUCTORES
        public VMFinal()
        {
            btnVolverCommand = new DelegateCommand(btnVolverCommandExecute);
            MegamindMAUI.Model.global.connection.On<Sala>("MandaFinal", (salaCompleta) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    InicializaDatos(salaCompleta);
                });
            });


        }
        #endregion

        #region METODOS

        /// <summary>
        /// Inicializa los datos de la pantalla para calcular el ganador y el perdedor
        /// </summary>
        /// <param name="sala"></param>
        private void InicializaDatos(Sala sala)
        {
            resultado1 = "Ganador";
            resultado2 = "Perdedor";
            if (sala.Jugador1?.Puntuacion < sala.Jugador2?.Puntuacion)
            {
                ganadorNombre = sala.Jugador1?.Nombre ?? "";
                ganadorPuntuacion = (sala.Jugador1?.Puntuacion + 1)?.ToString() ?? "";
                perdedorNombre = sala.Jugador2?.Nombre ?? "";
                perdedorPuntuacion = (sala.Jugador2?.Puntuacion + 1)?.ToString() ?? "";
            }
            else if (sala.Jugador2?.Puntuacion < sala.Jugador1?.Puntuacion)
            {
                ganadorNombre = sala.Jugador2?.Nombre ?? "";
                ganadorPuntuacion = (sala.Jugador2?.Puntuacion + 1)?.ToString() ?? "";
                perdedorNombre = sala.Jugador1?.Nombre ?? "";
                perdedorPuntuacion = (sala.Jugador1?.Puntuacion + 1)?.ToString() ?? "";
            }
            else
            {
                ganadorNombre = sala.Jugador1?.Nombre ?? "";
                ganadorPuntuacion = (sala.Jugador1?.Puntuacion + 1)?.ToString() ?? "";
                resultado1 = "Empate";
                perdedorNombre = sala.Jugador2?.Nombre ?? "";
                perdedorPuntuacion = (sala.Jugador2?.Puntuacion + 1)?.ToString() ?? "";
                resultado2 = "Empate";
            }
            OnPropertyChanged(nameof(GanadorNombre));
            OnPropertyChanged(nameof(GanadorPuntuacion));
            OnPropertyChanged(nameof(PerdedorNombre));
            OnPropertyChanged(nameof(PerdedorPuntuacion));
            OnPropertyChanged(nameof(Resultado1));
            OnPropertyChanged(nameof(Resultado2));
        }
        /// <summary>
        /// Inicializa la pantalla y llama al servidor para calcular el ganador y el perdedor
        /// </summary>
        private async void Inicializa()
        {
            sala = jugador.Sala;

            await MegamindMAUI.Model.global.connection.InvokeAsync("PideFinal", sala);
        }
        #endregion

        #region COMMANDS

        /// <summary>
        /// Comando para volver a la pantalla de inicio y dejar la sala
        /// </summary>
        public async void btnVolverCommandExecute()
        {
            App.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("///Inicio");

            MainThread.BeginInvokeOnMainThread(
              async () =>
              {
                  await MegamindMAUI.Model.global.connection.InvokeAsync("DejaSala", jugador);
              }
          );

        }
        #endregion
        

    }
}
