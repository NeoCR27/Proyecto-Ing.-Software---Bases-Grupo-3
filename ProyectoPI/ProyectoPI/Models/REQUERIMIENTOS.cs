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

    public partial class REQUERIMIENTOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REQUERIMIENTOS()
        {
            this.EMPLEADO1 = new HashSet<EMPLEADO>();
            this.PRUEBAS = new HashSet<PRUEBAS>();
        }

        public string idFK { get; set; }


        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "El máximo de caracteres es 100")]
        [Required(ErrorMessage = "El campo Nombre del requerimiento es obligatorio")]
        public string nombrePK { get; set; }


        [Display(Name = "Fecha de Inicio")]
      
        public System.DateTime fechaInicio { get; set; }


        [Display(Name = "Fecha de Entrega")]

        public System.DateTime fechaEntrega { get; set; }

        [Display(Name = "Horas Reales")]
        public Nullable<int> horasReales { get; set; }

         
        [Display(Name = "Dificultad")]

        public string dificultad { get; set; }

        [Display(Name = "Tester")]
        public string cedulaFK { get; set; }


        [Display(Name = "Horas Estimadas")]

        public Nullable<int> horasEstimadas { get; set; }


        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Display(Name = "Estado Actual")]
        public string estado_actual { get; set; }

        [Display(Name = "Estado Final")]
        public string estado_final { get; set; }

        [Display(Name = "Descripción Resultado")]
        [DataType(DataType.MultilineText)]
        public string descripcion_resultado { get; set; }

        public virtual EMPLEADO EMPLEADO { get; set; }
        public virtual PROYECTO PROYECTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLEADO> EMPLEADO1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRUEBAS> PRUEBAS { get; set; }
    }
    public class testerDisp
    {
        public string NombreEmpleado { get; set; }
        public string cedulaPK { get; set; }
    }

    public class PruebasPorEstado
    {
        public string estadoFinal { get; set; }
        public int cantidad { get; set; }
    }
}
