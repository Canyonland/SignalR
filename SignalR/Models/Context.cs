using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SignalR.Models
{
    public class Context:DbContext
    {
        public Context(): base("YuFamilyDB")
        {

        }

        public DbSet<WindowsClient> WindowsClients {get;set;}
    }
}