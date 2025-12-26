using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalasDeReunion.Core.Modelos;
using SalasDeReunion.Core.Servicios.Implementaciones;
using Xunit;

namespace SalasDeReunion.Tests.Servicios;
public class ReunionServiceTests
{
    private readonly ReunionService Servicio;

    public ReunionServiceTests()
    {
        // Instanciamos el servicio para usarlo en todas las pruebas
        Servicio = new ReunionService();
    }

    [Fact]
    public void CalcularMinimoSalas_SinSolapamientos_RetornaUnaSala()
    {
        // CASOS SIN SOLAPAMIENTOS
        // Escenario en donde una reunion termina y mucho despues empieza la otra.
        var reuniones = new List<Reunion>
        {
            new Reunion(new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
            new Reunion(new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0))
        };

        var resultado = Servicio.minimoSalasReuniones(reuniones);

        Assert.Equal(1, resultado);
    }

    [Fact]
    public void CalcularMinimoSalas_ConSolapamientos_RetornaMultiplesSalas()
    {
        // CASOS CON SOLAPAMIENTOS
        // Escenario del ejemplo: [08:30, 09:45], [07:45, 12:30], [14:00, 16:00]
        var reuniones = new List<Reunion>
        {
            new Reunion(new TimeSpan(8, 30, 0), new TimeSpan(9, 45, 0)),
            new Reunion(new TimeSpan(7, 45, 0), new TimeSpan(12, 30, 0)),
            new Reunion(new TimeSpan(14, 0, 0), new TimeSpan(16, 0, 0))
        };

        var resultado = Servicio.minimoSalasReuniones(reuniones);

        Assert.Equal(2, resultado);
    }

    [Fact]
    public void CalcularMinimoSalas_CasoDeBorde_InicioIgualAFin_ReutilizaSala()
    {
        // CASO DE BORDE (Inicio == Fin)
        // Escenario en donde una reunion termina a las 10:00 y la otra empieza exactamente a las 10:00.
        // NO debería contar como solapamiento.
        var reuniones = new List<Reunion>
        {
            new Reunion(new TimeSpan(8, 0, 0), new TimeSpan(10, 0, 0)),
            new Reunion(new TimeSpan(10, 0, 0), new TimeSpan(12, 0, 0))
        };

        var resultado = Servicio.minimoSalasReuniones(reuniones);

        Assert.Equal(1, resultado);
    }

    [Fact]
    public void CalcularMinimoSalas_RendimientoPequenaEscala_FuncionaCorrectamente()
    {
        // RENDIMIENTO CON MUCHAS REUNIONES
        // Creamos 1000 reuniones que se solapan parcialmente.
        var reuniones = new List<Reunion>();
        for (int i = 0; i < 1000; i++)
        {
            // Todas empiezan en momentos diferentes pero terminan mucho despues
            reuniones.Add(new Reunion(TimeSpan.FromMinutes(i), TimeSpan.FromMinutes(2000)));
        }

        var resultado = Servicio.minimoSalasReuniones(reuniones);

        // En el minuto 999 habra 1000 personas en salas diferentes.
        Assert.Equal(1000, resultado);
    }

    [Fact]
    public void CalcularMinimoSalas_ListaVacia_RetornaCero()
    {
        var resultado = Servicio.minimoSalasReuniones(new List<Reunion>());
        Assert.Equal(0, resultado);
    }
}
