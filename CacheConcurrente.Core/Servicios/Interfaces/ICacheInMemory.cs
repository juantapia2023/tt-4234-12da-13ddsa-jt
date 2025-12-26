using CacheConcurrente.Core.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheConcurrente.Core.Servicios.Interfaces
{
    public interface ICacheInMemory
    {
        void Set(string key, string value);
        string Get(string key);
        bool Delete(string key);
        IEnumerable<KeyValuePair<string, string>> KeysAndValues();

        public EstadisticasCache Estadisticas();
    }
}
