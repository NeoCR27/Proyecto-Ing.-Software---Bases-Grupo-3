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
    public class HABILIDADESController : Controller
    {


        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        // GET: HABILIDADES
        public ActionResult Index()
        {
            var hABILIDADES = db.HABILIDADES.Include(h => h.EMPLEADO);
            return View(hABILIDADES.ToList());
        }

        // GET: HABILIDADES/Details/5
        public ActionResult Details(string id)
        {
            SqlConnection connection = new SqlConnection("@DataInitial Catalog= Gr03Proy4");
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command;
            DataSet d_set = new DataSet();
            DataTable d_table;
            DataRow d_row;

            adapter.SelectCommand = new SqlCommand("SELECT E.nombre, H.valorPK FROM EMPLEADO E JOIN HABILIDADES HON E.cedulaPK = H.cedulaEmpleadoFK; ");

            adapter.Fill(d_set, "my_data");
            d_table = d_set.Tables["my_data"];
            d_row = d_table.Rows[0];
            
            /*using (var context = new BloggingContext())
            {
                var query = context.Blogs.SqlQuery("SELECT * FROM dbo.Blogs").ToList();
            }*/
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HABILIDADES hABILIDADES = db.HABILIDADES.Find(id);
            if (hABILIDADES == null)
            {
                return HttpNotFound();
            }
            return View(hABILIDADES);
        }

        // GET: HABILIDADES/Create
        public ActionResult Create()
        {
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel");
            return View();
        }

        // POST: HABILIDADES/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "valorPK,tipoPK,cedulaEmpleadoFK")] HABILIDADES hABILIDADES)
        {
            if (ModelState.IsValid)
            {
                db.HABILIDADES.Add(hABILIDADES);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", hABILIDADES.cedulaEmpleadoFK);
            return View(hABILIDADES);
        }

        // GET: HABILIDADES/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HABILIDADES hABILIDADES = db.HABILIDADES.Find(id);
            if (hABILIDADES == null)
            {
                return HttpNotFound();
            }
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", hABILIDADES.cedulaEmpleadoFK);
            return View(hABILIDADES);
        }

        // POST: HABILIDADES/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "valorPK,tipoPK,cedulaEmpleadoFK")] HABILIDADES hABILIDADES)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hABILIDADES).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", hABILIDADES.cedulaEmpleadoFK);
            return View(hABILIDADES);
        }

        // GET: HABILIDADES/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HABILIDADES hABILIDADES = db.HABILIDADES.Find(id);
            if (hABILIDADES == null)
            {
                return HttpNotFound();
            }
            return View(hABILIDADES);
        }

        // POST: HABILIDADES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HABILIDADES hABILIDADES = db.HABILIDADES.Find(id);
            db.HABILIDADES.Remove(hABILIDADES);
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
