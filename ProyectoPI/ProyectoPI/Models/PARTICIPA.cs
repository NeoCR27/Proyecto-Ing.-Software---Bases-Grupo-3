//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PARTICIPA
    {
        public string rol { get; set; }
        public string cedulaEmpleadoFK { get; set; }
        public string idProyectoFK { get; set; }
    
        public virtual EMPLEADO EMPLEADO { get; set; }
        public virtual PROYECTO PROYECTO { get; set; }
    }
}
