using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Config.Hubs
{
    public interface IClient
    {
        Task ReceiveMessage(string message);
    }
}
