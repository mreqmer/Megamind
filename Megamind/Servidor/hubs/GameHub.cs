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
        public static Jugador jugador5 = new Jugador("Pepi", "2211", 20);
        public static Jugador jugador6 = new Jugador("Ruben", "2211", 20);

        static List<Sala> salas = new List<Sala> {
            new Sala("aaa", jugador1),
            new Sala("bbb", jugador3),
            new Sala("ccc", jugador1, jugador2),
            new Sala("aaddda", jugador1, jugador2),
            new Sala("2211", jugador5, jugador6)
        };

        //Debug only
        public static List<Sala> ObtenerSalasActivas()
        {
            return salas;
        }

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
            bool salaExiste = salas.Any(s => s.NombreSala.Equals(sala.NombreSala));

            if (salaExiste)
            {
                await Clients.Caller.SendAsync("SalaCreada", false);
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, sala.NombreSala);
            salas.Add(sala);

            await Clients.Caller.SendAsync("SalaCreada", true);

            List<Sala> salasVacias = salas.Where(s => s.Jugador2 == null).ToList();
            await Clients.All.SendAsync("RecibeSalas", salasVacias);

            // Notificar al cliente que se ha unido a la sala
            await Clients.Caller.SendAsync("SalaUnida", true);
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
                    if (salas[i].Jugador1 == null || salas[i].Jugador1.Nombre.Equals(jugador.Nombre))
                    {
                        salas[i].Jugador1 = jugador;
                    }
                    else
                    {
                        salas[i].Jugador2 = jugador;
                    }
                    encontrado = true;
                }
                i++;
            }

            if (encontrado)
            {
                await Clients.Group(jugador.Sala).SendAsync("Espera");
            }
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

        public async Task PideFinal(string grupo)
        {
            Sala sala = new Sala();
            foreach (Sala s in salas)
            {
                if (s.NombreSala == grupo)
                {
                    sala = s;
                }
            }
            await Clients.Group(sala.NombreSala).SendAsync("MandaFinal", sala);
        }

    }
}
