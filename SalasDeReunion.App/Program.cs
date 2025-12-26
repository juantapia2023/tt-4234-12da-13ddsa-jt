using Microsoft.Extensions.DependencyInjection;
using SalasDeReunion.Core.Modelos;
using SalasDeReunion.Core.Servicios.Implementaciones;
using SalasDeReunion.Core.Servicios.Interfaces;

var servicios = new ServiceCollection()
    .AddSingleton<IReunionService, ReunionService>()
    .BuildServiceProvider();

var servicioReuniones = servicios.GetRequiredService<IReunionService>();

var listaDeReuniones = new List<Reunion>
{
    new Reunion(new TimeSpan(08, 30, 0), new TimeSpan(09, 45, 0)),
    new Reunion(new TimeSpan(07, 45, 0), new TimeSpan(12, 30, 0)),
    new Reunion(new TimeSpan(14, 0, 0), new TimeSpan(16, 0, 0))
};

Console.WriteLine("--- Calculador de Salas de Reunión ---");
int resultado = servicioReuniones.minimoSalasReuniones(listaDeReuniones);

Console.WriteLine($"Total de reuniones: {listaDeReuniones.Count}");
Console.WriteLine($"Mínimo de salas necesarias: {resultado}");
Console.WriteLine("---------------------------------------");