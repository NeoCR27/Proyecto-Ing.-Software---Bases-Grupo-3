
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace ProyectoPI.Models
{
    using System;
    using System.Collections.Generic;

    public partial class PARTICIPA
    {
        public string rol { get; set; }
        public string cedulaEmpleadoFK { get; set; }
        public string idProyectoFK { get; set; }
        public string nombreEmpleado { get; set; }

        public virtual EMPLEADO EMPLEADO { get; set; }
        public virtual PROYECTO PROYECTO { get; set; }
    }
    //Clase para query de empleados por
    public class EquipoModel
    {
        public string IDProyecto { get; set; }
        public string empleado { get; set; }
        public string email { get; set; }
        public string rol { get; set; }
        public string proyNom { get; set; }
        public string dispon { get; set; }
        public string idEmp { get; set; }
        public string idCli { get; set; }

    }
    //Clase para query de empleados por habilidad
    public class HabilidadEmpleadoModel
    {
        public string personalID { get; set; }
        public string nombre { get; set; }
        public string tipoHabilidad { get; set; }
        public string habilidad { get; set; }
    }
}