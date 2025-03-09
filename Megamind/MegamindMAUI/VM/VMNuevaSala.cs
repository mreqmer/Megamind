using ENT;
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
        private bool esCargando = false;
        private bool esBotonVisible = true;
        private bool esTextoDesbloqueado = true;
        private bool esMensajeSalaExistente = false;
        #endregion

        #region ATRIBUTOS
        public bool EsCargando { get { return esCargando; } set { esCargando = value; OnPropertyChanged(nameof(EsCargando)); } }
        public bool EsBotonVisible { get { return esBotonVisible; } set { esBotonVisible = value; OnPropertyChanged(nameof(EsCargando)); } }
        public bool EsTextoDesbloqueado { get { return esTextoDesbloqueado; } set { esTextoDesbloqueado = value; OnPropertyChanged(nameof(EsTextoDesbloqueado)); } }
        public string NombreUsuario { get { return nombreUsuario; } set { nombreUsuario = value; OnPropertyChanged(nameof(NombreUsuario));  } }
        public string NombreSala { get { return nombreSala; } set { nombreSala = value; OnPropertyChanged(nameof(NombreSala)); } }
        public DelegateCommand BtnCrearSalaCommand { get { return btnCrearSalaCommand; } }
        public bool EsMensajeSalaExistente { get { return esMensajeSalaExistente; } set { esMensajeSalaExistente = value; OnPropertyChanged(nameof(EsMensajeSalaExistente)); } }
        #endregion

        #region CONSTRUCTORES

        public VMNuevaSala()
        {
            btnCrearSalaCommand = new DelegateCommand(btnCrearSalaCommandExecute);

            MegamindMAUI.Model.global.connection.On<bool>("SalaCreada", (creada) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (creada)
                    {
                        esBotonVisible = false;
                        OnPropertyChanged(nameof(EsBotonVisible));
                        esCargando = true;
                        OnPropertyChanged(nameof(EsCargando));
                        esTextoDesbloqueado = false;
                        OnPropertyChanged(nameof(EsTextoDesbloqueado));

                        // Esperar a recibir la confirmación de que se ha unido a la sala
                        MegamindMAUI.Model.global.connection.On<bool>("SalaUnida", (unido) =>
                        {
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                if (unido)
                                {
                                    await gotoJuego();
                                }
                                else
                                {
                                    esMensajeSalaExistente = true;
                                    OnPropertyChanged(nameof(EsMensajeSalaExistente));
                                    RestaurarEstadosUI();
                                }
                            });
                        });
                    }
                    else
                    {
                        esMensajeSalaExistente = true;
                        OnPropertyChanged(nameof(EsMensajeSalaExistente));
                        RestaurarEstadosUI();
                    }
                });
            });
        }


        #endregion

        #region COMMANDS

        public async void btnCrearSalaCommandExecute()
        {
            Jugador jugador = new Jugador(NombreUsuario, NombreSala, 0);
            
            Sala nuevaSala = new Sala(nombreSala, jugador);

            //  MainThread.BeginInvokeOnMainThread(
            //    async () =>
            //    {
            //        await MegamindMAUI.Model.global.connection.InvokeAsync("CreaSala", nuevaSala);
            //    }
            //);
            esCargando = true;
            OnPropertyChanged(nameof(EsCargando));
            esTextoDesbloqueado = false;
            OnPropertyChanged(nameof(EsTextoDesbloqueado));
            esMensajeSalaExistente = false;
            OnPropertyChanged(nameof(EsMensajeSalaExistente));

            await MegamindMAUI.Model.global.connection.InvokeAsync("CreaSala", nuevaSala);
        }

        private void RestaurarEstadosUI()
        {
            esCargando = false;
            OnPropertyChanged(nameof(EsCargando));
            esTextoDesbloqueado = true;
            OnPropertyChanged(nameof(EsTextoDesbloqueado));
        }


        public async Task gotoJuego()
        {
            
            Jugador jugador = new Jugador(nombreUsuario, nombreSala, 0);

            var queryParams = new Dictionary<string, object>
                    {
                    { "jugador", jugador }
                    };
            await Shell.Current.GoToAsync("///Megamind", queryParams);
        }

        #endregion


    }
}
