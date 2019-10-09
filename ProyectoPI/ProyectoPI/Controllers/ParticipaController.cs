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

namespace ProyectoPI.Views
{
    public class PARTICIPAController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        //Get team
        public ActionResult showTeam()
        {
            var pARTICIPA = db.PARTICIPA.Include(p => p.EMPLEADO).Include(p => p.PROYECTO);
            return View(pARTICIPA.ToList());
        }

        // GET: PARTICIPA
        public ActionResult Index()
        {
            string query = "SELECT Part.idProyectoFK AS 'id', Emp.nombre + ' ' + Emp.primerApellido + ' ' + Emp.segundoApellido AS 'employee', Emp.correo AS 'email', Part.rol AS 'role', Proy.nombre AS 'proyName' FROM PARTICIPA Part Join EMPLEADO Emp ON Emp.cedulaPK = Part.cedulaEmpleadoFK Join PROYECTO proy ON proy.idPK = Part.idProyectoFK WHERE Part.idProyectoFK = proy.idPK ORDER BY part.idProyectoFK";
         
            return View((db.Database.SqlQuery<TeamViewModel>(query)).ToList());
        }

        // GET: PARTICIPA/Details/5
        public ActionResult Details()
        {
         
            return View();
        }

        // GET: PARTICIPA/Create
        public ActionResult Create()
        {
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel");
            ViewBag.idProyectoFK = new SelectList(db.PROYECTO, "idPK", "nombre");
            return View();
        }

        // POST: PARTICIPA/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "rol,cedulaEmpleadoFK,idProyectoFK")] PARTICIPA pARTICIPA)
        {
            if (ModelState.IsValid)
            {
                db.PARTICIPA.Add(pARTICIPA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", pARTICIPA.cedulaEmpleadoFK);
            ViewBag.idProyectoFK = new SelectList(db.PROYECTO, "idPK", "nombre", pARTICIPA.idProyectoFK);
            return View(pARTICIPA);
        }

        // GET: PARTICIPA/Edit/5
        public ActionResult Edit(string id = "2411", string searchBy = "", string searchFilter = "")
        {
            //Se Arma el string de query
            //  string currentID = "0";
            string queryTeam = "SELECT Part.idProyectoFK AS 'ID Proyecto', Emp.nombre + ' ' + Emp.primerApellido + ' ' + Emp.segundoApellido AS 'employee', Emp.correo AS 'email', Part.rol AS 'role', Emp.disponibilidad FROM PARTICIPA Part Join EMPLEADO Emp ON Emp.cedulaPK = Part.cedulaEmpleadoFK WHERE Part.idProyectoFK = " + id;
            string queryEmployees = "Select Distinct emp.cedulaPK AS 'personalID', emp.Nombre + ' ' + emp.primerApellido + ' ' + emp.segundoApellido AS 'name' , hab.tipoPK AS 'habilityType' , hab.valorPK AS 'hability'From EMPLEADO emp Join HABILIDADES hab ON emp.cedulaPK = hab.cedulaEmpleadoFK WHERE hab.tipoPK LIKE '%" + searchBy + "%' AND hab.valorPK LIKE '%" + searchFilter + "%' Order BY emp.cedulaPK";
            //Se hace el query a la base de datos
            IList<TeamViewModel> teamQueryResult = (db.Database.SqlQuery<TeamViewModel>(queryTeam)).ToList();
            IList<EmployeeHabilityModel> employeeQueryResult = (db.Database.SqlQuery<EmployeeHabilityModel>(queryEmployees)).ToList();
            /* foreach(var employee in employeeQueryResult)
             {
                if(currentID.Equals(employee.personalID))
                 {
                     employee.hability += ", Otro"; 
                 }
                 currentID = employee.personalID;
             }*/
            //Se pasa a viewData para llamar desde la vista
            ViewData["team"] = teamQueryResult;
            ViewData["employees"] = employeeQueryResult;
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

            // GET: PARTICIPA/Delete/5
            public ActionResult Delete(string id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PARTICIPA pARTICIPA = db.PARTICIPA.Find(id);
                if (pARTICIPA == null)
                {
                    return HttpNotFound();
                }
                return View(pARTICIPA);
            }

            // POST: PARTICIPA/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(string id)
            {
                PARTICIPA pARTICIPA = db.PARTICIPA.Find(id);
                db.PARTICIPA.Remove(pARTICIPA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
            public void agregar(string idProyectoFK, string cedulaEmpleadoFK, string rol)
            {
                string query = "INSERT INTO PARTICIPA (cedulaEmpleadoFK,idProyectoFK,rol) VALUES (@cedulaEmpleadoFK,@idProyectoFK,@rol)";
                db.Database.ExecuteSqlCommand(query,
                    new SqlParameter("@cedulaEmpleadoFK", cedulaEmpleadoFK),
                    new SqlParameter("@idProyectoFK", idProyectoFK),
                    new SqlParameter("@rol", rol)
                );
            }
    }
}
