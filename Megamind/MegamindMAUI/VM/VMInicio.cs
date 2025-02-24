using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT;
using MegamindMAUI.VM.Utils;

namespace MegamindMAUI.VM
{
    public class VMInicio : ClsVMBase
    {
        #region Atributos
        private string username = "";
        private DelegateCommand btnInicioCommand;
        private double opacidadBtnInicio = 1;
        private bool esInicioVisible = true;
        private DelegateCommand btnPlayCommand;
        private double opacidadBtnPlay = 0;
        private bool esPlayVisible = false;
        #endregion

        #region Propiedades
        public string Username { get { return username; } set { username = value; OnPropertyChanged(nameof(Username)); BtnPlayCommand.RaiseCanExecuteChanged(); } }
        public DelegateCommand BtnInicioCommand { get { return btnInicioCommand; } }
        public double OpacidadBtnInicio { get { return opacidadBtnInicio; } set { opacidadBtnInicio = value; } }
        public bool EsInicioVisible { get { return esInicioVisible; } }
        public DelegateCommand BtnPlayCommand { get { return btnPlayCommand; } }
        public double OpacidadBtnPlay { get { return opacidadBtnPlay; } set { opacidadBtnPlay = value; } }
        public bool EsPlayVisible { get { return esPlayVisible; } set { esPlayVisible = value; } }
        #endregion

        #region Constructores
        public VMInicio()
        {
            btnInicioCommand = new DelegateCommand(btnInicioCommandExecute);
            btnPlayCommand = new DelegateCommand(btnPlayCommandExecute, btnPlayCommandCanExecute);
        }
        #endregion

        #region Commands

        /// <summary>
        /// boton inicio, que al ser pulsado hace una animacion de fadeout y empieza una de fadein para el input del nombre y el play
        /// </summary>
        public async void btnInicioCommandExecute()
        {
            Console.WriteLine("pulsado");
            esInicioVisible = false;
            await FadeOutBtnInicio(0.5);
            esPlayVisible = true;
            await FadeInBtnPlay(0.5);
        }

        /// <summary>
        /// Comprueba que el campo nombre no esté vacío para poder ser pulsado
        /// </summary>
        /// <returns></returns>
        public bool btnPlayCommandCanExecute()
        {
            bool execute = false;
            if ( username!= "" && username != null)
            {
                execute = true;
            }
            return execute;
        }

        /// <summary>
        /// Botón de ejecución para ir a la vista con el juego
        /// </summary>
        public async void btnPlayCommandExecute()
        {
            await Shell.Current.GoToAsync("///Juego");
        }
        #endregion


        #region Metodos
        /// <summary>
        /// Animacion de fadeout para que se quite la visibilidad de inicio
        /// </summary>
        /// <param name="duracion"></param>
        /// <returns></returns>
        public async Task FadeOutBtnInicio(double duracion)
        {
            double stepDuration = duracion > 0 ? duracion / 60.0 : 0; // 60 FPS
            double opacityStep = 1.0 / 60.0;

            if (stepDuration > 0 && OpacidadBtnInicio > 0)
            {
                while (OpacidadBtnInicio > 0)
                {
                    OpacidadBtnInicio = Math.Max(0, OpacidadBtnInicio - opacityStep);
                    OnPropertyChanged(nameof(OpacidadBtnInicio));

                    if (OpacidadBtnInicio == 0)
                    {
                        esInicioVisible = false;
                        OnPropertyChanged(nameof(EsInicioVisible));
                    }

                    await Task.Delay((int)(stepDuration * 1000));
                }
            }
        }

        /// <summary>
        /// Animacion de fadein para que aparezca el input del username y el botón play
        /// </summary>
        /// <param name="duracion"></param>
        /// <returns></returns>
        public async Task FadeInBtnPlay(double duracion)
        {
            double stepDuration = duracion > 0 ? duracion / 60.0 : 0; // 60 FPS
            double opacityStep = 1.0 / 60.0;

            if (stepDuration > 0 && OpacidadBtnPlay < 1)
            {
                esPlayVisible = true;
                OnPropertyChanged(nameof(EsPlayVisible));
                while (opacidadBtnPlay < 1)
                {
                    OpacidadBtnPlay = Math.Min(1, OpacidadBtnPlay + opacityStep);
                    OnPropertyChanged(nameof(OpacidadBtnPlay));

                    await Task.Delay((int)(stepDuration * 1000));
                }
            }
        }
        #endregion

    }
}
