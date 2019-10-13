using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProyectoPI.Models;

namespace ProyectoPI.Controllers
{
    public class CLIENTEController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();
        private SeguridadController seguridadController = new SeguridadController();

        // GET: CLIENTE
        public async Task<ActionResult> Index()
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.myRol = rol;
            System.Diagnostics.Debug.WriteLine(rol);
            if (rol == "Cliente")
            {
                var miInfo = db.CLIENTE.Where(x => x.correo == mail);
                return View(miInfo.ToList());
            }

            return View(db.CLIENTE.ToList());
        }

        // GET: CLIENTE/Details/5
        public async Task<ActionResult> Details(string id)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.myRol = rol;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        // GET: CLIENTE/Create
        public async Task<ActionResult> Create()
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.myRol = rol;
            return View();
        }

        // POST: CLIENTE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cedulaPK,tel,nombre,primerApellido,segundoApellido,correo,distrito,canton,provincia,direccionExacta")] CLIENTE cLIENTE)
        {
            CLIENTE duplicate = db.CLIENTE.Find(cLIENTE.cedulaPK);
            if (duplicate == null)
            {
                if (ModelState.IsValid)
                {
                    db.CLIENTE.Add(cLIENTE);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("NoDuplicados");
            }

            return View(cLIENTE);
        }

        // GET: CLIENTE/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.myRol = rol;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        // POST: CLIENTE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cedulaPK,tel,nombre,primerApellido,segundoApellido,correo,distrito,canton,provincia,direccionExacta")] CLIENTE cLIENTE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLIENTE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cLIENTE);
        }

        // GET: CLIENTE/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.myRol = rol;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        // POST: CLIENTE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            db.CLIENTE.Remove(cLIENTE);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult NoDuplicados()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<SelectListItem> GetClientes()
        { // Retorna los nombres y cédulas de los clientes
            List<CLIENTE> clientes = (db.CLIENTE.ToList());

            List<SelectListItem> informacion = clientes.ConvertAll(e => {
                return new SelectListItem()
                {
                    Text = e.nombre + " " + e.primerApellido,
                    Value = e.cedulaPK,
                    Selected = false
                };
            });

            foreach(CLIENTE e in clientes)
            {
                System.Diagnostics.Debug.WriteLine(e.nombre);
            }
            return informacion;
        }
    }
}
