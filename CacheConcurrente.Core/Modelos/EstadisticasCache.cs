using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheConcurrente.Core.Modelos
{
    public record EstadisticasCache(
        int LlamadasTotales,
        int ClavesActuales,
        int ExpiradosPorTTL,
        int ExpulsadosPorLRU,
        int CapacidadMaxima
    );
}
