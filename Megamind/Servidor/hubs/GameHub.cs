using ENT;
using Microsoft.AspNetCore.SignalR;
using Servidor.Model;
using System.Xml.Linq;

namespace Servidor.hubs
{
    public class GameHub : Hub
    {
        static List<Sala> salas = new List<Sala>();

        public async Task UneSala(Sala sala)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sala.NombreSala);
            //salas[]
            
        }
        public async Task CreaSala(Sala sala)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sala.NombreSala);
            salas.Add(sala);
        }

        public async Task DejaSala(string salaId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, salaId);
        }

        public async Task MandaSalas()
        {
            await Clients.All.SendAsync("RecibeSalas", salas);
        }

        public async Task MandaSolucion(string salaId)
        {
            List<int> numeros = calculaSolucion();
            await Clients.Group(salaId).SendAsync("RecibeSolucion", numeros);
        }

        public async Task Terminado(string salaId)
        {
            await Clients.Group(salaId).SendAsync("Espera");
        }



        private List<int> calculaSolucion()
        {
            int indice = 0;
            int num;
            Random random = new Random();
            List<int> solucion = new List<int>();

            while (indice < 4)
            {
                num = random.Next(0, 8);
                if (!solucion.Contains(num))
                {
                    solucion.Add(num);
                    indice++;
                }
            }
            return solucion;
        }

    }
}
