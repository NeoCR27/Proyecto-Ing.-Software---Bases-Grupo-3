//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HABILIDADES
    {
        public string valorPK { get; set; }
        public string tipoPK { get; set; }
        public string cedulaEmpleadoFK { get; set; }
    
        public virtual EMPLEADO EMPLEADO { get; set; }
    }
}