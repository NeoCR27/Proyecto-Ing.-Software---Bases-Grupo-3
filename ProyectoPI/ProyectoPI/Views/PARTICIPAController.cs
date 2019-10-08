using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoPI.Models;

namespace ProyectoPI.Views
{
    public class PARTICIPAController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();
        private EMPLEADOController empleados = new EMPLEADOController();
        private PROYECTOesController proyect = new PROYECTOesController();
        private HABILIDADESController habilidad = new HABILIDADESController();

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
        public ActionResult Edit(string id = "2411", string searchFilter = null)
        {
            string queryTeam = "SELECT Part.idProyectoFK AS 'ID Proyecto', Emp.nombre + ' ' + Emp.primerApellido + ' ' + Emp.segundoApellido AS 'employee', Emp.correo AS 'email', Part.rol AS 'role' FROM PARTICIPA Part Join EMPLEADO Emp ON Emp.cedulaPK = Part.cedulaEmpleadoFK WHERE Part.idProyectoFK = " + id;
            
            //db.Database.SqlQuery<TeamViewModel>(query)).ToList();
            ViewData["employees"] = (db.EMPLEADO.Where(x => x.nombre.StartsWith(searchFilter) || searchFilter == null)).ToList();
           // var tuple = new Tuple<TeamViewModel, empDesc>(new TeamViewModel(), new empDesc());
            return View((db.Database.SqlQuery<TeamViewModel>(queryTeam)).ToList());
        }
       /* @foreach(var employee in ViewData["employees"] as IEnumerable<ProyectoPI.Models.TeamViewModel>)
        {
                                         < li >
                                             @employee.employee, @employee.email ,@employee.role
                                         </ li >
                                     }*/
       /*    @foreach (var employee in ViewData["employees"] as IEnumerable<ProyectoPI.Models.TeamViewModel>)
       {
           <li>
               @employee.employee, @employee.email ,@employee.role
           </li>
       }*/

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
}
}
