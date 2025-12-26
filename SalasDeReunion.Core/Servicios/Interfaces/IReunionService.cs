using SalasDeReunion.Core.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalasDeReunion.Core.Servicios.Interfaces
{
    /// <summary>
    /// Define los métodos relacionados con el calculo de salas necesarias para la programación de reuniones.
    /// </summary>
    public interface IReunionService
    {
        /// <summary>
        /// Calcula el número mínimo de salas de reunión necesarias para 
        /// cubrir todas las reuniones sin solapamientos.
        /// </summary>
        /// <param name="reuniones">
        /// Lista de reuniones que contienen la hora de inicio y fin.
        /// </param>
        /// <returns>
        /// Número mínimo de salas requeridas para que todas las reuniones
        /// puedan realizarse sin conflictos de horario.
        /// </returns>
        int minimoSalasReuniones(List<Reunion> reuniones);
    }
}
