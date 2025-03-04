using ENT;
using Microsoft.AspNetCore.SignalR;
using Servidor.Model;
using System.Xml.Linq;

namespace Servidor.hubs
{
    public class GameHub : Hub
    {
        //static List<Sala> salas = new List<Sala>();

        public static Jugador jugador1 = new Jugador("Jugador 1", "1", 10);
        public static Jugador jugador2 = new Jugador("Jugador 2", "2", 20);
        public static Jugador jugador3 = new Jugador("Jugador 3", "2", 20);
        public static Jugador jugador4 = new Jugador("Jugador 4", "2", 20);

        static List<Sala> salas = new List<Sala> { 
            new Sala("aaa", jugador1, jugador2),
            new Sala("bbb", jugador3, jugador4),
            new Sala("ccc", jugador1, jugador2),
            new Sala("aaddda", jugador1, jugador2)
        };
        

        public async Task UneSala(String sala)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sala);
            
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
