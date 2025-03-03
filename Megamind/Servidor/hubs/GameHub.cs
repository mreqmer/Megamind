using ENT;
using Microsoft.AspNetCore.SignalR;
using Servidor.Model;
using System.Xml.Linq;

namespace Servidor.hubs
{
    public class GameHub : Hub
    {
        static List<Sala> salas = new List<Sala>();

        public async Task UneSala(string salaId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, salaId);
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
            //TODO
            await Clients.Group(salaId).SendAsync("RecibeSolucion", "a");
        }

        
    }
}
