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
    [QueryProperty(nameof(Sala), "NombreSala")]
    public class VMFinal : ClsVMBase
    {
        //TODO implementar el servidor
        private string ganadorNombre = "";
        private string ganadorPuntuacion = "";
        private string perdedorNombre = "";
        private string perdedorPuntuacion = "";
        private string sala = "";
        private string resultado1;
        private string resultado2;
        private DelegateCommand btnVolverCommand;


        public string GanadorNombre { get { return ganadorNombre; } set { ganadorNombre = value; OnPropertyChanged(nameof(GanadorNombre)); } }
        public string GanadorPuntuacion { get { return ganadorPuntuacion; } set { ganadorPuntuacion = value; OnPropertyChanged(nameof(GanadorPuntuacion)); } }
        public string PerdedorNombre { get { return perdedorNombre; } set { perdedorNombre = value; OnPropertyChanged(nameof(PerdedorNombre)); } }
        public string PerdedorPuntuacion { get { return perdedorPuntuacion; } set { perdedorPuntuacion = value; OnPropertyChanged(nameof(PerdedorPuntuacion)); } }
        public string Sala { get { return sala; } set { sala = value; Inicializa(); OnPropertyChanged(nameof(Sala)); } }
        public string Resultado1 { get { return resultado1; } set { resultado1 = value; OnPropertyChanged(nameof(Resultado1)); } }
        public string Resultado2 { get { return resultado2; } set { resultado2 = value; OnPropertyChanged(nameof(Resultado2)); } }
        public DelegateCommand BtnVolverCommand { get { return btnVolverCommand; } }

        public VMFinal()
        {
            btnVolverCommand = new DelegateCommand(btnVolverCommandExecute);
        }
        private async void InicializaDatos(Sala sala)
        {
            resultado1 = "Ganador";
            resultado1 = "Perdedor";
            if (sala.Jugador1.Puntuacion > sala.Jugador2.Puntuacion)
            {
                ganadorNombre = sala.Jugador1.Nombre;
                ganadorPuntuacion = sala.Jugador1.Puntuacion.ToString();
                perdedorNombre = sala.Jugador2.Nombre;
                perdedorPuntuacion = sala.Jugador2.Puntuacion.ToString();
            } else if (sala.Jugador1.Puntuacion > sala.Jugador2.Puntuacion)
            {
                ganadorNombre = sala.Jugador2.Nombre;
                ganadorPuntuacion = sala.Jugador2.Puntuacion.ToString();
                perdedorNombre = sala.Jugador1.Nombre;
                perdedorPuntuacion = sala.Jugador1.Puntuacion.ToString();
            }
            else
            {
                ganadorNombre = sala.Jugador1.Nombre;
                ganadorPuntuacion = sala.Jugador1.Puntuacion.ToString();
                resultado1 = "Empate";
                perdedorNombre = sala.Jugador2.Nombre;
                perdedorPuntuacion = sala.Jugador2.Puntuacion.ToString();
                resultado1 = "Empate";
            }
        }

        public void btnVolverCommandExecute()
        {
            Console.WriteLine("pulsado");
        }

        private async void Inicializa()
        {
            await MegamindMAUI.Model.global.connection.InvokeAsync("PideFinal", sala);

            MegamindMAUI.Model.global.connection.On<Sala>("MandaFinal", (salaCompleta) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    InicializaDatos(salaCompleta);
                });
            });
        }

    }
}
