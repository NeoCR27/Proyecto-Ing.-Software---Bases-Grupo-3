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

    public partial class CLIENTE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CLIENTE()
        {
            this.PROYECTO = new HashSet<PROYECTO>();
        }

        [MaxLength(20,ErrorMessage = "El máximo de caracteres es 20")]
        [Required(ErrorMessage="El campo Nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [MaxLength(20, ErrorMessage = "El máximo de caracteres es 20")]
        [Required(ErrorMessage = "El campo Primer Apellido es obligatorio")]
        [Display(Name = "Primer Apellido")]
        public string primerApellido { get; set; }

        [MaxLength(20, ErrorMessage = "El máximo de caracteres es 20")]
        [Display(Name = "Segundo Apellido")]
        public string segundoApellido { get; set; }

        [MaxLength(20, ErrorMessage = "El máximo de caracteres es 20")]
        [Required(ErrorMessage = "El campo Cédula es obligatorio")]
        [Display(Name = "Cédula")]
        public string cedulaPK { get; set; }
        
        [MaxLength(15, ErrorMessage = "El máximo de caracteres es 15")]
        [Required(ErrorMessage = "El campo Teléfono es obligatorio")]
        [Display(Name = "Teléfono")]
        public string tel { get; set; }

        [MaxLength(40, ErrorMessage = "El máximo de caracteres es 40")]
        [EmailAddress(ErrorMessage = "El campo Correo tiene que ser un correo válido")]
        [Required(ErrorMessage = "El campo Correo es obligatorio")]
        [Display(Name = "Correo")]
        public string correo { get; set; }

        [MaxLength(20, ErrorMessage = "El máximo de caracteres es 20")]
        [Required(ErrorMessage = "El campo Provincia es obligatorio")]
        [Display(Name = "Provincia")]
        public string provincia { get; set; }

        [MaxLength(20, ErrorMessage = "El máximo de caracteres es 20")]
        [Required(ErrorMessage = "El campo Cantón es obligatorio")]
        [Display(Name = "Cantón")]
        public string canton { get; set; }

        [MaxLength(20, ErrorMessage = "El máximo de caracteres es 20")]
        [Required(ErrorMessage = "El campo Distrito es obligatorio")]
        [Display(Name = "Distrito")]
        public string distrito { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(200, ErrorMessage = "El máximo de caracteres es 200")]
        [Display(Name = "Dirección Exacta")]
        public string direccionExacta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROYECTO> PROYECTO { get; set; }
    }
}
