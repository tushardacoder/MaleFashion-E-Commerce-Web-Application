using MaleFashion.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Infrastructure
{
    public class ServerTime : IServerTime
    {
        public DateTime DateTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
