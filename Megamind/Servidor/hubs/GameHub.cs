using ENT;
using Microsoft.AspNetCore.SignalR;
using Servidor.Model;
using System.Collections.ObjectModel;
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
            new Sala("aaa", jugador1),
            new Sala("bbb", jugador3),
            new Sala("ccc", jugador1, jugador2),
            new Sala("aaddda", jugador1, jugador2)
        };
        

        public async Task UneSala(String sala, Jugador jugador)
        {
            bool encontrado = false;
            int contador = 0;
            
            while (!encontrado && contador < salas.Count)
            {
                if (salas[contador].NombreSala.Equals(sala)){
                    salas[contador].Jugador2 = jugador;
                    encontrado = true;
                }
                contador++;
            }

            if (encontrado)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, sala);
                await Clients.Group(sala).SendAsync("SalaUnida", encontrado);
            }else
            {
                await Clients.Group(sala).SendAsync("SalaUnida", encontrado);
            }
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
            List<Sala> salasVacias = new List<Sala>();

            foreach(Sala s in salas)
            {
                if (s.Jugador2 == null)
                {
                    salasVacias.Add(s);
                }
            }
            await Clients.All.SendAsync("RecibeSalas", salasVacias);
        }

        public async Task MandaSolucion(string salaId)
        {
            List<int> numeros = calculaSolucion();
            await Clients.Group(salaId).SendAsync("RecibeSolucion", numeros);
        }

        public async Task Terminado(Jugador jugador)
        {
            int i = 0;
            bool encontrado = false;

            while (i < salas.Count && !encontrado)
            {
                if (salas[i].NombreSala.Equals(jugador.Sala))
                {
                    if(jugador1.Nombre.Equals(jugador.Nombre))
                    {
                        salas[i].Jugador1 = jugador;
                    }
                    else
                    {
                        salas[i].Jugador2 = jugador;
                    }
                    i++;
                }
            }

            await Clients.Group(jugador.Sala).SendAsync("Espera");
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

        public async Task MandaPisticha(string grupo, ObservableCollection<Pisticha> pista, int ronda)
        {
            await Clients.OthersInGroup(grupo).SendAsync("RecibePisticha", pista, ronda);
        }

    }
}
