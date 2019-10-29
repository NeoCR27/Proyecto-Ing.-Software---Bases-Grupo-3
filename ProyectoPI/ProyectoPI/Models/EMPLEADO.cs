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
    
    public partial class EMPLEADO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLEADO()
        {
            this.HABILIDADES = new HashSet<HABILIDADES>();
            this.PARTICIPA = new HashSet<PARTICIPA>();
            this.REQUERIMIENTOS = new HashSet<REQUERIMIENTOS>();
        }
    
        public string cedulaPK { get; set; }
        public string tel { get; set; }
        public string nombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string correo { get; set; }
        public string distrito { get; set; }
        public string canton { get; set; }
        public string provincia { get; set; }
        public string direccionExacta { get; set; }
        public Nullable<int> horasLaboradas { get; set; }
        public int edad { get; set; }
        public string disponibilidad { get; set; }
        public string rol { get; set; }
        public System.DateTime fechaNacimiento { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HABILIDADES> HABILIDADES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARTICIPA> PARTICIPA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REQUERIMIENTOS> REQUERIMIENTOS { get; set; }
    }
}
