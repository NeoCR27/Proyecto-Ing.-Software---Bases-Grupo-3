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
    public class PruebasProy
    {
        public string EstadoFinal { get; set; }
        public string TesterResponsable { get; set; }
        public int CantidadPruebas { get; set; }
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

    public class getLiderReq
    {
        public string nombre { get; set; }

        public string cedula { get; set; }
        public int totales { get; set; }
    }

    public class getLiderReqDificultad
    {
        public int req { get; set; }
        public string cedula { get; set; }

    }
    public class getLideres
    {
        public string nombre { get; set; }
        public string cedula { get; set; }

    }

    public class getProyectos
    {
        public string nombre { get; set; }
        public string cedula { get; set; }
        public string proyecto { get; set; }
        public string proyectoNombre { get; set; }
    }

    public class getPorcentajes
    {
        public int parcial { get; set; }
        public int porcentaje { get; set; }
    }

    public class DuracionProy
    {
        public string nombreReq { get; set; }
        public int duracionEstimada { get; set; }
        public int duracionReal { get; set; }
        public string dificultad { get; set; }
        public string estadoFinal { get; set; }
    }

    public class HorasReq
    {
        public string nombreReq { get; set; }
        public int horasEstimadas { get; set; }
        public int horasReales { get; set; }
    }

    public class TesterParticipacion
    {
        public string nombre { get; set; }
        public string cedula { get; set; }

        public int baja { get; set; }
        public int intermedia { get; set; }
        public int alta { get; set; }
    }
    public class TesterParticipacionGlobal
    {
        public string nombre { get; set; }

        public int total { get; set; }
        public int participacion { get; set; }
    }
    public class TesterParticipacionGlobalReq
    {
        public string nombre { get; set; }

        public string proyecto { get; set; }
        public string idProy { get; set; }

        public int participacion { get; set; }
    }
}
