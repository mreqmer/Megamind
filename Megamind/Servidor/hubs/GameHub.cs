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

        //public static Jugador jugador1 = new Jugador("Jugador 1", "1", 10);
        //public static Jugador jugador2 = new Jugador("Jugador 2", "2", 20);
        //public static Jugador jugador3 = new Jugador("Jugador 3", "2", 20);
        //public static Jugador jugador4 = new Jugador("Jugador 4", "2", 20);
        //public static Jugador jugador5 = new Jugador("Pepi", "2211", 20);
        //public static Jugador jugador6 = new Jugador("Ruben", "2211", 20);

        //static List<Sala> salas = new List<Sala> {
        //    new Sala("aaa", jugador1),
        //    new Sala("bbb", jugador3),
        //    new Sala("ccc", jugador1, jugador2),
        //    new Sala("aaddda", jugador1, jugador2),
        //    new Sala("2211", jugador5, jugador6)
        //};

        static List<Sala> salas = new List<Sala>();

        //Debug only
        public static List<Sala> ObtenerSalasActivas()
        {
            return salas;
        }

        /// <summary>
        /// Une un jugador a una sala
        /// </summary>
        /// <param name="sala"></param>
        /// <param name="jugador"></param>
        /// <returns></returns>
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
                List<Sala> salasVacias = salas.Where(s => s.Jugador2 == null).ToList();
                await Clients.All.SendAsync("RecibeSalas", salasVacias);
            }
            else
            {
                await Clients.Group(sala).SendAsync("SalaUnida", encontrado);
            }
        }

        /// <summary>
        /// Crea una sala, si no existe ya
        /// </summary>
        /// <param name="sala"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deja una sala y elimina al jugador de la lista de jugadores de la sala correspondiente
        /// </summary>
        /// <param name="jugador"></param>
        /// <returns></returns>
        public async Task DejaSala(Jugador jugador)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, jugador.Sala);

            int i = 0;
            bool jugadorEliminado = false;

            while (i < salas.Count && !jugadorEliminado)
            {

                if (salas[i].Jugador1 != null && salas[i].Jugador1.Nombre == jugador.Nombre)
                {
                    salas[i].Jugador1 = null;
                    jugadorEliminado = true;
                }
                else if (salas[i].Jugador2 != null && salas[i].Jugador2.Nombre == jugador.Nombre)
                {
                    salas[i].Jugador2 = null;
                    jugadorEliminado = true;
                }
                if(salas[i].Jugador1 == null && salas[i].Jugador2 == null)
                {
                    jugadorEliminado = true;
                    salas.Remove(salas[i]);
                }

                i++;
            }


        }

        /// <summary>
        /// Manda las salas vacías a todos los clientes
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Manda la solución a los jugadores de una sala concreta
        /// </summary>
        /// <param name="salaId"></param>
        /// <returns></returns>
        public async Task MandaSolucion(string salaId)
        {
            List<int> numeros = calculaSolucion();
            await Clients.Group(salaId).SendAsync("RecibeSolucion", numeros);
        }

        /// <summary>
        /// Manda la pista a los jugadores de una sala concreta
        /// </summary>
        /// <param name="jugador"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Calcula la solución del juego
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Manda la pista a los jugadores de una sala concreta 
        /// </summary>
        /// <param name="grupo"></param>
        /// <param name="pista"></param>
        /// <param name="ronda"></param>
        /// <returns></returns>
        public async Task MandaPisticha(string grupo, ObservableCollection<Pisticha> pista, int ronda)
        {
            await Clients.OthersInGroup(grupo).SendAsync("RecibePisticha", pista, ronda);
        }

        /// <summary>
        /// Manda el final del juego a los jugadores de una sala concreta
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns></returns>
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

        
        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    // Obtener el nombre del jugador desde el contexto
        //    if (Context.Items.TryGetValue("NombreJugador", out var nombreJugadorObj) && nombreJugadorObj is string nombreJugador)
        //    {
        //        // Obtener todas las salas donde el jugador desconectado está presente
        //        var salasConJugador = salas.Where(s =>
        //            (s.Jugador1 != null && s.Jugador1.Nombre == nombreJugador) || // Comparar por nombre
        //            (s.Jugador2 != null && s.Jugador2.Nombre == nombreJugador)    // Comparar por nombre
        //        ).ToList();

        //        foreach (var sala in salasConJugador)
        //        {
        //            if (sala.Jugador1 != null && sala.Jugador1.Nombre == nombreJugador)
        //            {
        //                sala.Jugador1 = null;
        //                await Clients.Group(sala.NombreSala).SendAsync("JugadorDesconectado", "Jugador 1 se ha desconectado.");
        //            }
        //            else if (sala.Jugador2 != null && sala.Jugador2.Nombre == nombreJugador)
        //            {
        //                sala.Jugador2 = null;
        //                await Clients.Group(sala.NombreSala).SendAsync("JugadorDesconectado", "Jugador 2 se ha desconectado.");
        //            }

        //            // Si no quedan jugadores en la sala, cerrar la sala
        //            if (sala.Jugador1 == null && sala.Jugador2 == null)
        //            {
        //                salas.Remove(sala);
        //                await Clients.Group(sala.NombreSala).SendAsync("SalaCerrada", "La sala se ha cerrado porque todos los jugadores se han desconectado.");
        //                await Groups.RemoveFromGroupAsync(Context.ConnectionId, sala.NombreSala);
        //            }
        //        }
        //    }

        //    await base.OnDisconnectedAsync(exception);
        //}

    }
}
