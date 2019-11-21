using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ProyectoPI.Models
{
    public class EstadoReq
    {
        public string estado_actual { get; set; }
        public int Cantidad { get; set; }
    }

    public class GetReq
    {
        public string nombrePK { get; set; }
        public string nombre { get; set; }
        public string estado_actual { get; set; }
    }

    public class CantReq
    {
        public int Cantidad { get; set; }
        public string cedulaPK { get; set; }
        public string nombre { get; set; }

    }

    public class EstadoAsigReq
    {
        public string nombrePK { get; set; }
        public string correo { get; set; }
        public string nombre { get; set; }
        public string estado_actual { get; set; }
        public string FechaInicio { get; set; }
        public string FechaActual { get; set; }
        public string FechaEntrega { get; set; }
        public int dias { get; set; }
        public int diasf { get; set; }

    }

    public class NumHab
    {
        public string Habilidad { get; set; }
        public int Total { get; set; }
    }
    public class HistorialReq
    {
        public string Estado { get; set; }
        public int Total { get; set; }
    }
}