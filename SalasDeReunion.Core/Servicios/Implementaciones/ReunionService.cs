using SalasDeReunion.Core.Modelos;
using SalasDeReunion.Core.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalasDeReunion.Core.Servicios.Implementaciones
{
    public class ReunionService : IReunionService
    {

        public int minimoSalasReuniones(List<Reunion> reuniones)
        {
            if (reuniones == null || reuniones.Count == 0) return 0;

            // Extraemos y ordenamos las horas de inicio y horas finales por separado
            var horasDeInicio = reuniones.Select(r => r.inicio).OrderBy(t => t).ToArray();
            var horasDeFin = reuniones.Select(r => r.fin).OrderBy(t => t).ToArray();

            int salasOcupadas = 0;
            int maximasSalas = 0;
            int indiceFin = 0;

            // Recorremos las horas de inicio
            for (int indiceInicio = 0; indiceInicio < reuniones.Count; indiceInicio++)
            {
                // Si la reunión inicia antes de que la más antigua termine, solapamiento detectado
                if (horasDeInicio[indiceInicio] < horasDeFin[indiceFin])
                {
                    salasOcupadas++;
                }
                else
                {
                    // Una reunión termino antes o justo cuando esta empieza, movemos el puntero de fin
                    // No sumamos sala porque reutilizamos la que se libero
                    indiceFin++;
                }

                // Guardamos el pico máximo de salas ocupadas
                if (salasOcupadas > maximasSalas)
                {
                    maximasSalas = salasOcupadas;
                } 
            }

            return maximasSalas;
        }
    }
}
