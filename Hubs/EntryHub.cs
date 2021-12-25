using System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smarthome.Hubs
{
    public class EntryHub : Hub
    {
        public async Task TestData(string data)
        {
            Console.WriteLine(data);
        }
    }
}