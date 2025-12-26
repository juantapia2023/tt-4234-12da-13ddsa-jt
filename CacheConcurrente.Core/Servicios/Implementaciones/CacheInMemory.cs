using CacheConcurrente.Core.Modelos;
using CacheConcurrente.Core.Servicios.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheConcurrente.Core.Servicios.Implementaciones
{
    public class CacheInMemory : ICacheInMemory
    {
        private readonly int Capacidad;
        private readonly TimeSpan TiempoDeVencimiento;

        private readonly ConcurrentDictionary<string, LinkedListNode<EntradaCache>> Diccionario;
        private readonly LinkedList<EntradaCache> ListaLRU;
        private readonly object BloquearLista = new object();

        private int LlamadasTotales = 0;
        private int ExpulsionesLRU = 0;
        private int ExpiracionesTTL = 0;

        public CacheInMemory(int capacity, int defaultTtlSeconds)
        {
            Capacidad = capacity;
            TiempoDeVencimiento = TimeSpan.FromSeconds(defaultTtlSeconds);
            Diccionario = new ConcurrentDictionary<string, LinkedListNode<EntradaCache>>();
            ListaLRU = new LinkedList<EntradaCache>();
        }


        public string Get(string clave)
        {
            Interlocked.Increment(ref LlamadasTotales);

            // Intentar obtener el nodo del diccionario
            if (!Diccionario.TryGetValue(clave, out var nodo))
            {
                return null; // No existe
            }

            var entrada = nodo.Value;

            // Verificar TTL para saber si ya murio por tiempo
            if (entrada.EstaExpirado)
            {
                Delete(clave); // Limpieza automática
                Interlocked.Increment(ref ExpiracionesTTL);
                return null;
            }

            // Si está vivo actualizamos su importancia (LRU y TTL)
            lock (BloquearLista)
            {
                // Refrescar tiempo de vida
                entrada.ActualizarExpiracion(TiempoDeVencimiento);

                // Mover al principio de la lista ya que es el mas reciente ahora
                ListaLRU.Remove(nodo);
                ListaLRU.AddFirst(nodo);
            }

            return entrada.Valor;
        }

        public void Set(string clave, string valor)
        {
            Interlocked.Increment(ref LlamadasTotales);

            // en caso de que la clave ya exista solo se actualiza su valor en tiempo de vida y se mueve al principio de la lista
            if (Diccionario.TryGetValue(clave, out var nodoExistente))
            {
                lock (BloquearLista)
                {
                    nodoExistente.Value.Valor = valor;
                    nodoExistente.Value.ActualizarExpiracion(TiempoDeVencimiento);

                    // lo movemos al principio de la lsita porque acaba de ser usado
                    ListaLRU.Remove(nodoExistente);
                    ListaLRU.AddFirst(nodoExistente);
                }
                return;
            }

            // en caso que sea una clave nueva se crea una nueva EntradaCahe
            var nuevaEntrada = new EntradaCache(clave, valor, TiempoDeVencimiento);
            var nuevoNodo = new LinkedListNode<EntradaCache>(nuevaEntrada);

            lock (BloquearLista)
            {
                // si necesitamos liberar espacio usamos LRU para expulsar el ultimo elemento de la lista
                if (Diccionario.Count >= Capacidad)
                {
                    ExpulsarElementoMasViejo();
                }

                // se insertar en la lista y el diccionario
                ListaLRU.AddFirst(nuevoNodo);
                Diccionario.TryAdd(clave, nuevoNodo);
            }
        }

        private void ExpulsarElementoMasViejo()
        {
            // se quita el ultimo elemento de la lista ya que es el que lleva mas tiempo sin usarse
            var ultimo = ListaLRU.Last;
            if (ultimo != null)
            {
                ListaLRU.RemoveLast();
                Diccionario.TryRemove(ultimo.Value.Clave, out _);
                Interlocked.Increment(ref ExpulsionesLRU);
            }
        }

        public bool Delete(string clave)
        {
            // se elimina la clave del diccionario
            if (Diccionario.TryRemove(clave, out var nodo))
            {
                // si este elemento ya existia se saca de la lista tambien para liberar memoria
                lock (BloquearLista)
                {
                    ListaLRU.Remove(nodo);
                }
                return true;
            }
            return false;
        }

        public IEnumerable<KeyValuePair<string, string>> KeysAndValues()
        {
            // Retornamos una instantanea actual de la cache
            // Filtramos los que ya expiraron pero que no han sido consultados
            return Diccionario
                .Where(kvp => !kvp.Value.Value.EstaExpirado)
                .Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value.Value.Valor))
                .ToList();
        }

        public EstadisticasCache Estadisticas()
        {
            return new EstadisticasCache(
                LlamadasTotales: LlamadasTotales,
                ClavesActuales: Diccionario.Count,
                ExpiradosPorTTL: ExpiracionesTTL,
                ExpulsadosPorLRU: ExpulsionesLRU,
                CapacidadMaxima: Capacidad
            );
        }
    }
}
