﻿using ENT;
using MegamindMAUI.VM.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegamindMAUI.Model;
using Microsoft.AspNetCore.SignalR.Client;
using Servidor.Model;

namespace MegamindMAUI.VM
{
    public class VMNuevaSala : ClsVMBase
    {

        #region PROPIEDADES
        private string nombreUsuario = "";
        private string nombreSala = "";
        private DelegateCommand btnCrearSalaCommand;
        #endregion

        #region ATRIBUTOS
        public string NombreUsuario { get { return nombreUsuario; } set { nombreUsuario = value; OnPropertyChanged(nameof(NombreUsuario));  } }
        public string NombreSala { get { return nombreSala; } set { nombreSala = value; OnPropertyChanged(nameof(NombreSala)); } }
        public DelegateCommand BtnCrearSalaCommand { get { return btnCrearSalaCommand; } }
        #endregion

        #region CONSTRUCTORES

        public VMNuevaSala()
        {
            btnCrearSalaCommand = new DelegateCommand(btnCrearSalaCommandExecute);
        }

        #endregion

        #region COMMANDS

        public async void btnCrearSalaCommandExecute()
        {
            Jugador jugador = new Jugador(NombreUsuario, NombreSala, 0);
            
            Sala nuevaSala = new Sala(nombreSala, jugador);

            MainThread.BeginInvokeOnMainThread(
              async () =>
              {
                  await MegamindMAUI.Model.global.connection.InvokeAsync("CreaSala", nuevaSala);
              }
          );
            await Shell.Current.GoToAsync("///Salas");
        }
        
        #endregion


    }
}
