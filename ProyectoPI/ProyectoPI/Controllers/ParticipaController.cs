using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoPI.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace ProyectoPI.Controllers
{
    public class PARTICIPAController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();
       // private SeguridadController seguridadController = new SeguridadController();

        // GET: PARTICIPA
        public /*async Task<ActionResult>*/ ActionResult Index()
        {
           /* string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.my_role = rol;
            if(rol == "Tester" || rol == "Lider")
            {
                var nombreLiderActual = (from emp in db.EMPLEADO
                                         where emp.correo == mail
                                         select new
                                         {
                                             emp.cedulaPK
                                         }).ToList();
                
                string empActual = nombreLiderActual.First().ToString();
                int tamano = empActual.Length - 2;
                empActual = empActual.Substring(1);
                empActual = empActual.Substring(0, empActual.Length - 2);
                System.Diagnostics.Debug.WriteLine(empActual);
                ViewBag.empActual = empActual;
            }*/
            string query = "SELECT Part.idProyectoFK AS 'IDProyecto', Emp.nombre + ' ' + Emp.primerApellido + ' ' + Emp.segundoApellido AS 'empleado', Emp.correo AS 'email', Part.rol AS 'rol', Proy.nombre AS 'proyNom' FROM PARTICIPA Part Join EMPLEADO Emp ON Emp.cedulaPK = Part.cedulaEmpleadoFK Join PROYECTO proy ON proy.idPK = Part.idProyectoFK WHERE Part.idProyectoFK = proy.idPK ORDER BY part.idProyectoFK";
            return View((db.Database.SqlQuery<EquipoModel>(query)).ToList());
        }

        // GET: PARTICIPA/Edit/5
        public ActionResult Edit(string id = "2411", string buscarPor = "", string filtroBusqueda = "")
        {
            //Se Arma los  string de query
            ViewBag.id = id;
            string queryEquipo = "SELECT Part.idProyectoFK AS 'IDProyecto', Emp.nombre + ' ' + Emp.primerApellido + ' ' + Emp.segundoApellido AS 'empleado', Emp.correo AS 'email', Part.rol AS 'rol', Emp.disponibilidad AS 'dispon', Emp.cedulaPK AS 'idEmp' FROM PARTICIPA Part Join EMPLEADO Emp ON Emp.cedulaPK = Part.cedulaEmpleadoFK WHERE Part.idProyectoFK = " + id;
            string queryEmpleado = "Select Distinct emp.cedulaPK AS 'personalID', emp.Nombre + ' ' + emp.primerApellido + ' ' + emp.segundoApellido AS 'nombre' , hab.tipoPK AS 'tipoHabilidad' , hab.valorPK AS 'habilidad' From EMPLEADO emp Join HABILIDADES hab ON emp.cedulaPK = hab.cedulaEmpleadoFK WHERE hab.tipoPK LIKE '%" + buscarPor + "%' AND hab.valorPK LIKE '%" + filtroBusqueda + "%' AND Emp.rol != 'Lider' AND Emp.rol != 'Jefe' AND Emp.Disponibilidad != 0 Order BY emp.cedulaPK";
            //Se hace el query a la base de datos
            IList<EquipoModel> resultadoQueryEquipo = (db.Database.SqlQuery<EquipoModel>(queryEquipo)).ToList();
            IList<HabilidadEmpleadoModel> resultadoQueryEmpleado = (db.Database.SqlQuery<HabilidadEmpleadoModel>(queryEmpleado)).ToList();
            /* foreach(var employee in employeeQueryResult)
             {
                if(currentID.Equals(employee.personalID))
                 {
                     employee.hability += ", Otro"; 
                 }
                 currentID = employee.personalID;
             }*/
            //Se pasa a viewData para llamar desde la vista
            ViewData["equipo"] = resultadoQueryEquipo;
            ViewData["empleados"] = resultadoQueryEmpleado;           
            return View();
        }

     

        //@Html.HiddenFor(x => x.Item1.role);
        //@Html.HiddenFor(x => x.Item1.Proyecto);

        // POST: PARTICIPA/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit([Bind(Include = "rol,cedulaEmpleadoFK,idProyectoFK")] PARTICIPA pARTICIPA)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(pARTICIPA).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", pARTICIPA.cedulaEmpleadoFK);
                ViewBag.idProyectoFK = new SelectList(db.PROYECTO, "idPK", "nombre", pARTICIPA.idProyectoFK);
                return View(pARTICIPA);
            }
        //Agrega un tester recibiendo los datos para crear la relación con el proyecto
        [HttpPost]
        public ActionResult AgregarTester(string idProyecto, string idEmpleado)
        {
            EMPLEADO eMPLEADO = db.EMPLEADO.Find(idEmpleado);
            eMPLEADO.disponibilidad = false;
            db.Entry(eMPLEADO).State = EntityState.Modified;
            db.SaveChanges();
            agregar(idProyecto, idEmpleado, "Tester");
            return RedirectToAction("Edit", new { id = idProyecto });
        }
        //Elimina un tester de la relación con el proyecto.
        [HttpPost]
        public ActionResult RemoverTester(string idProyecto, string idEmpleado)
        {
            EMPLEADO eMPLEADO = db.EMPLEADO.Find(idEmpleado);
            eMPLEADO.disponibilidad = true;
            db.Entry(eMPLEADO).State = EntityState.Modified;
            db.SaveChanges();
            Eliminar(idProyecto, idEmpleado);
            return RedirectToAction("Edit", new { id = idProyecto });
        }

        //Agrega un empleado a un proyecto con un rol específico haciendo el query en la DB
        public void agregar(string idProyectoFK, string cedulaEmpleadoFK, string rol)
        {
            string query = "INSERT INTO PARTICIPA (cedulaEmpleadoFK,idProyectoFK,rol) VALUES (@cedulaEmpleadoFK,@idProyectoFK,@rol)";
            db.Database.ExecuteSqlCommand(query,
                new SqlParameter("@cedulaEmpleadoFK", cedulaEmpleadoFK),
                new SqlParameter("@idProyectoFK", idProyectoFK),
                new SqlParameter("@rol", rol)
            );
        }

        //Elimina un empleado de un proyecto haciendo el query en la DB
        public void Eliminar(string idProyectoFK, string cedulaEmpleadoFK)
        {
            PARTICIPA pARTICIPA = db.PARTICIPA.Find(cedulaEmpleadoFK, idProyectoFK);
            db.PARTICIPA.Remove(pARTICIPA);
            db.SaveChanges();
        }
    }
}
