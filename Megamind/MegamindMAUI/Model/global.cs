using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegamindMAUI.Model
{
    public class global
    {

        public static String url = "https://localhost:7227/GameHub";
        public static HubConnection connection;
    }
}
