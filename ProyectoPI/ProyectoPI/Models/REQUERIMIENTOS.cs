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
    
    public partial class REQUERIMIENTOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REQUERIMIENTOS()
        {
            this.PRUEBAS = new HashSet<PRUEBAS>();
            this.EMPLEADO1 = new HashSet<EMPLEADO>();
        }
    
        public string idFK { get; set; }
        public string nombrePK { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public System.DateTime fechaEntrega { get; set; }
        public Nullable<int> horasReales { get; set; }
        public string dificultad { get; set; }
        public string cedulaFK { get; set; }
        public Nullable<int> horasEstimadas { get; set; }
        public string Descripcion { get; set; }
        public string estado_actual { get; set; }
        public string estado_final { get; set; }
        public string descripcion_resultado { get; set; }
    
        public virtual EMPLEADO EMPLEADO { get; set; }
        public virtual PROYECTO PROYECTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRUEBAS> PRUEBAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLEADO> EMPLEADO1 { get; set; }
    }
}
