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
    using System.ComponentModel.DataAnnotations;

    public partial class PRUEBAS
    {
        public string idProyFK { get; set; }
        public string nombreReqFK { get; set; }

        [MaxLength(100, ErrorMessage = "El máximo de caracteres es 100")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string nombrePK { get; set; }

        [MaxLength(50, ErrorMessage = "El máximo de caracteres es 50")]
        [Display(Name = "Estado final")]
        public string EstadoFinal { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(100, ErrorMessage = "El máximo de caracteres es 100")]
        [Display(Name = "Detalles de los resultados")]
        public string resultadoDetalles { get; set; }

        public virtual REQUERIMIENTOS REQUERIMIENTOS { get; set; }
    }
}