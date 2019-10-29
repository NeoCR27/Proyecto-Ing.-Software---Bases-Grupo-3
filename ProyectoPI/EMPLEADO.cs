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

    public partial class EMPLEADO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLEADO()
        {
            this.HABILIDADES = new HashSet<HABILIDADES>();
            this.PARTICIPA = new HashSet<PARTICIPA>();
            this.REQUERIMIENTOS = new HashSet<REQUERIMIENTOS>();
        }
        [Required]
        [MaxLength(9)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Cedula constituida unicamente por numeros")]
        [Display(Name = "Cedula")]
        public string cedulaPK { get; set; }

        [Required]
        [Phone]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Telefono constituido unicamente por valores numericos")]
        [Display(Name = "Telefono")]
        public string tel { get; set; }

        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Nombre no puede contener valores numericos")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Apellido no puede contener valores numericos")]
        [Display(Name = "Primer apellido")]
        public string primerApellido { get; set; }


        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Apellido no puede contener valores numericos")]
        [Display(Name = "Segundo apellido")]
        public string segundoApellido { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string correo { get; set; }

        [Required]
        [Display(Name = "Distrito")]
        public string distrito { get; set; }

        [Required]
        [Display(Name = "Canton")]
        public string canton { get; set; }

        [Required]
        [Display(Name = "Provincia")]
        public string provincia { get; set; }


        [Display(Name = "Direccion exacta")]
        public string direccionExacta { get; set; }

        //[Required] SE CAE!!!!!
        [RegularExpression("^[0-9]*$", ErrorMessage = "Horas laboradas constituida unicamente por valores numericos")]
        [Display(Name = "Horas laboradas")]
        public Nullable<int> horasLaboradas { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Edad constituida unicamente por valores numericos")]
        [Display(Name = "Edad")]
        public int edad { get; set; }

        [Required]
        [Display(Name = "Disponibilidad")]
        public string disponibilidad { get; set; }

        [Display(Name = "Rol")]
        public string rol { get; set; }

        [Required]
        [Display(Name = "Fecha de nacimiento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime fechaNacimiento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HABILIDADES> HABILIDADES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARTICIPA> PARTICIPA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REQUERIMIENTOS> REQUERIMIENTOS { get; set; }
    }
}
