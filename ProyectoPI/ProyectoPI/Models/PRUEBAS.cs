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
    
    public partial class PRUEBAS
    {
        public string idProyFK { get; set; }
        public string nombreReqFK { get; set; }
        public string nombrePK { get; set; }
        public string resultFinal { get; set; }
        public string estado { get; set; }
        public string proposito { get; set; }
        public string entradaDatos { get; set; }
        public string flujo { get; set; }
        public string resultEsperado { get; set; }
        public string prioridad { get; set; }
        public string descripcionErr { get; set; }
        public byte[] imagenErr { get; set; }
        public string estadoErr { get; set; }
    
        public virtual REQUERIMIENTOS REQUERIMIENTOS { get; set; }
    }
}