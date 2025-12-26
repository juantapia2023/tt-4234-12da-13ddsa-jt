using CacheConcurrente.Core.Servicios.Implementaciones;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheConcurrente.Tests
{
    public class CacheInMemoryTests
    {
        private readonly CacheInMemory Cache;
        private const int CapacidadTest = 3;
        private const int TtlSegundosTest = 2;

        public CacheInMemoryTests()
        {
            // Configuramos una cache pequeña para probar facilmente la expulsion
            Cache = new CacheInMemory(CapacidadTest, TtlSegundosTest);
        }

        [Fact]
        public void Set_AgregarNuevaEntrada_DebeGuardarCorrectamente()
        {
            // Arrange & Act
            Cache.Set("usuario:1", "Juan David");
            var resultado = Cache.Get("usuario:1");

            // Assert
            Assert.Equal("Juan David", resultado);
        }

        [Fact]
        public void Get_EntradaExpiradaPorTTL_DebeRetornarNull()
        {
            // Arrange
            Cache.Set("token", "12345");

            // Act: Esperamos a que pase el tiempo de vida (TTL)
            // Usamos un margen pequeño adicional para asegurar la expiración
            Thread.Sleep((TtlSegundosTest + 1) * 1000);

            var resultado = Cache.Get("token");

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void Set_ExpulsionPorLRU_DebeEliminarElMenosUsado()
        {
            // Arrange: Llenamos la capacidad (3)
            Cache.Set("a", "1");
            Cache.Set("b", "2");
            Cache.Set("c", "3");

            // Act: Accedemos a "a" y "b", haciendo que "c" sea el menos usado recientemente
            Cache.Get("a");
            Cache.Get("b");

            // Agregamos un cuarto elemento, esto debe expulsar a "c"
            Cache.Set("d", "4");

            // Assert
            Assert.Null(Cache.Get("c")); // Expulsado
            Assert.NotNull(Cache.Get("a"));
            Assert.NotNull(Cache.Get("b"));
            Assert.NotNull(Cache.Get("d"));
        }

        [Fact]
        public void Get_ActualizaTTL_DebeMantenerDatoVivo()
        {
            // Arrange
            Cache.Set("sesion", "activa");

            // Act: Esperamos casi todo el tiempo del TTL
            Thread.Sleep(1500);
            Cache.Get("sesion"); // Al hacer Get, el TTL se reinicia otros 2 seg

            Thread.Sleep(1500); // Pasan otros 1.5 seg
            var resultado = Cache.Get("sesion");

            // Assert: El dato sigue vivo porque el Get reinicio el cronometro
            Assert.Equal("activa", resultado);
        }

        [Fact]
        public void Stats_DebeReflejarMetricasCorrectas()
        {
            // Arrange
            Cache.Set("k1", "v1");
            Cache.Set("k2", "v2");
            Cache.Set("k3", "v3");
            Cache.Set("k4", "v4"); // Provoca 1 expulsion por LRU

            // Act
            dynamic estadisticas = Cache.Estadisticas();

            // Assert
            Assert.Equal(1, (int)estadisticas.ExpulsadosPorLRU);
            Assert.Equal(3, (int)estadisticas.ClavesActuales);
        }
    }
}
