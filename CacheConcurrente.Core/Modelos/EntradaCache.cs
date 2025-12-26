using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheConcurrente.Core.Modelos;

// Usamos una clase interna porque solo la caché necesita conocer estos detalles
internal class EntradaCache
{
    public string Clave { get; set; }
    public string Valor { get; set; }
    public DateTime FechaExpiracion { get; set; }

    public EntradaCache(string clave, string valor, TimeSpan ttl)
    {
        Clave = clave;
        Valor = valor;
        ActualizarExpiracion(ttl);
    }

    public void ActualizarExpiracion(TimeSpan ttl)
    {
        // El TTL se suma a la hora actual para definir el momento exacto de "muerte"
        FechaExpiracion = DateTime.Now.Add(ttl);
    }

    public bool EstaExpirado => DateTime.Now > FechaExpiracion;
}
