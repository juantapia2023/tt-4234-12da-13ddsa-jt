using Microsoft.Extensions.DependencyInjection;
using CacheConcurrente.Core.Modelos;
using CacheConcurrente.Core.Servicios.Implementaciones;
using CacheConcurrente.Core.Servicios.Interfaces;

var servicios = new ServiceCollection()
    // cache con 3 espacios y 5 segundos de vida
    .AddSingleton<ICacheInMemory>(new CacheInMemory(capacity: 3, defaultTtlSeconds: 5))
    .BuildServiceProvider();

var cache = servicios.GetRequiredService<ICacheInMemory>();

Console.WriteLine("--- Prueba de Caché Concurrente (LRU + TTL) ---");

// Llenar la cache
cache.Set("A", "Dato A");
cache.Set("B", "Dato B");
cache.Set("C", "Dato C");
Console.WriteLine("Caché llena (A, B, C).");

// Provocar LRU
Console.WriteLine("Agregando 'D', debería expulsar a 'A' (el menos usado)...");
cache.Set("D", "Dato D");

// Mostrar estado
var datos = cache.KeysAndValues();
Console.WriteLine("Claves actuales en caché:");
foreach (var kvp in datos)
{
    Console.WriteLine($"- {kvp.Key}: {kvp.Value}");
}

// estadísticas
var reporte = cache.Estadisticas();
Console.WriteLine($"\nEstadísticas: {reporte}");

Console.WriteLine("\nEsperando 6 segundos para probar TTL...");
Thread.Sleep(6000);

Console.WriteLine($"¿Existe 'B' después del tiempo?: {cache.Get("B") ?? "NULL (Expiró)"}");