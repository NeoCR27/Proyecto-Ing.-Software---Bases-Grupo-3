﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Gr03Proy4Entities : DbContext
    {
        public Gr03Proy4Entities()
            : base("name=Gr03Proy4Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CLIENTE> CLIENTE { get; set; }
        public virtual DbSet<EMPLEADO> EMPLEADO { get; set; }
        public virtual DbSet<HABILIDADES> HABILIDADES { get; set; }
        public virtual DbSet<PARTICIPA> PARTICIPA { get; set; }
        public virtual DbSet<PROYECTO> PROYECTO { get; set; }
        public virtual DbSet<REQUERIMIENTOS> REQUERIMIENTOS { get; set; }
        public virtual DbSet<PRUEBAS> PRUEBAS { get; set; }
    
        public virtual int Consulta_Cant_Req_Estado(string idproy)
        {
            var idproyParameter = idproy != null ?
                new ObjectParameter("idproy", idproy) :
                new ObjectParameter("idproy", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Consulta_Cant_Req_Estado", idproyParameter);
        }
    
        public virtual int Consulta_Cantidad_Req_Tester(string idproy)
        {
            var idproyParameter = idproy != null ?
                new ObjectParameter("idproy", idproy) :
                new ObjectParameter("idproy", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Consulta_Cantidad_Req_Tester", idproyParameter);
        }
    
        public virtual ObjectResult<string> Get_Proy_Correo(string correo)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("correo", correo) :
                new ObjectParameter("correo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("Get_Proy_Correo", correoParameter);
        }
    }
}
