using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Ddd.Data.Options
{
    public class ConnectionStrings : Dictionary<string, string>
    {

        public const string DefaultConnectionStringName = "Default";

        public string Default
        {
            get => this[DefaultConnectionStringName];
            set => this[DefaultConnectionStringName] = value;
        }
    }
    
}
