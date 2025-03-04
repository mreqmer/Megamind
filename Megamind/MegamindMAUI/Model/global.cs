using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegamindMAUI.Model
{
    public class global
    {
        public static String url = "https://localhost:7227/GameHub";
        public static HubConnection connection;

        public static async Task InicializaConexion()
        {
            try
            {
                if (connection == null)
                {
                    connection = new HubConnectionBuilder()
                        .WithUrl(url)
                        .WithAutomaticReconnect() // 📌 Agregar reconexión automática
                        .Build();
                }

                if (connection.State == HubConnectionState.Disconnected)
                {
                    await connection.StartAsync();
                    Debug.WriteLine("✅ Conectado a SignalR.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error al conectar con SignalR: {ex.Message}");
            }
        }
    }
}
