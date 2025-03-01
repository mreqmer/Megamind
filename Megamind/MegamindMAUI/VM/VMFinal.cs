using MegamindMAUI.VM.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegamindMAUI.VM
{
    public class VMFinal : ClsVMBase
    {
        //TODO implementar el servidor
        private string ganadorNombre = "Juanito";
        private string ganadorPuntuacion = "1235";
        private string perdedorNombre = "Pepita";
        private string perdedorPuntuacion = "555";
        private DelegateCommand btnVolverCommand;


        public string GanadorNombre { get { return ganadorNombre; } }
        public string GanadorPuntuacion { get { return ganadorPuntuacion; } }
        public string PerdedorNombre { get { return perdedorNombre; } }
        public string PerdedorPuntuacion { get { return perdedorPuntuacion; } }
        public DelegateCommand BtnVolverCommand { get { return btnVolverCommand; } }

        public VMFinal()
        {
            btnVolverCommand = new DelegateCommand(btnVolverCommandExecute);
        }

        public void btnVolverCommandExecute()
        {
            Console.WriteLine("pulsado");
        }


    }
}
