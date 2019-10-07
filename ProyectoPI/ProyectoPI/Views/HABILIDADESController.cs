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

namespace ProyectoPI.Views
{
    /*
SqlConnection connection = new SqlConnection("@DataInitial Catalog= Gr03Proy4");
SqlDataAdapter adapter = new SqlDataAdapter();
SqlCommand command;
DataSet d_set = new DataSet();
DataTable d_table;
DataRow d_row;

adapter.SelectCommand = new SqlCommand("SELECT E.nombre, H.valorPK FROM EMPLEADO E JOIN HABILIDADES H ON E.cedulaPK = H.cedulaEmpleadoFK; ");

adapter.Fill(d_set, "my_data");
d_table = d_set.Tables["my_data"];
d_row = d_table.Rows[0];

/*using (var context = new BloggingContext())
{
    var query = context.Blogs.SqlQuery("SELECT * FROM dbo.Blogs").ToList();
}*/
    public class HABILIDADESController : Controller
    {

        private EMPLEADOController emp_controller = new EMPLEADOController();
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        // GET: HABILIDADES
        public ActionResult Index(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var hABILIDADES = db.HABILIDADES.Include(h => h.EMPLEADO);
            var hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id);
            SelectList nombre = emp_controller.get_nombres(id);
            ViewBag.nombre = nombre;
            return View(hABILIDADES.ToList());
        }

        // GET: HABILIDADES/Details/5
        public ActionResult Details(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //string sql_query = "SELECT valorPK FROM HABILIDADES WHERE 1 = cedulaEmpleadoFK;";
            //List<String> query = db.Database.SqlQuery<String>(sql_query).ToList();
            ViewBag.habilidades = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id), "", "valorPK");

            SelectList nombre = emp_controller.get_nombres(id);
            ViewBag.nombre = nombre;

            return View();
        }

        public ActionResult detalles_empleado(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string sql_query = "SELECT valorPK, tipoPK FROM HABILIDADES WHERE cedulaEmpleadoFK = 1;";
            //IList<String> query = db.Database.SqlQuery<String>(sql_query).ToList();

            ViewBag.habilidades = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id), "", "valorPK", "tipoPK");

            //IList<HABILIDADES> habilidades = (db.Database.SqlQuery<HABILIDADES>(sql_query)).ToList();
            //ViewData["data"] = habilidades;

            SelectList nombre = emp_controller.get_nombres(id);
            ViewBag.nombre = nombre;

            return View();
        }

        // GET: HABILIDADES/Create
        public ActionResult Create()
        {
            var hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == "1");
            ViewBag.cedulaEmpleadoFK = emp_controller.get_cedulas();
            string[] values = new[] { "Tecnica", "Blanda" };
            ViewBag.tipoPK = new SelectList(values);

            //return View(hABILIDADES.ToList());
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
                return RedirectToAction("../EMPLEADO/index");
            }

            //ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", hABILIDADES.cedulaEmpleadoFK);
            return View(hABILIDADES);
        }

        // GET: HABILIDADES/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id);
            //ViewBag.datos = new SelectList(db.HABILIDADES.Where(x=>x.cedulaEmpleadoFK == id), "cedulaEmpleadoFK", "tipoPK", "valorPK");

            //ViewBag.cedulaEmpleadoFK = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id), "", "valorPK", "tipoPK");
            return View(hABILIDADES.ToList());
        }

        // POST: HABILIDADES/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598. [Bind(Include = "valorPK,tipoPK,cedulaEmpleadoFK")] HABILIDADES hABILIDADES 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
            foreach (string key in collection.AllKeys)
            {
                Response.Write("Key = " + key);
                Response.Write(collection[key]);
                Response.Write("<br/>");

            }
            /*if (ModelState.IsValid)
            {
                db.Entry(hABILIDADES).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", hABILIDADES.cedulaEmpleadoFK);
            */
            return View();
        }

        // GET: HABILIDADES/Delete/5
        public ActionResult Delete(string id, string valor, string tipo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //string query = "SELECT * FROM HABILIDADES WHERE tipoPK = @tipo AND valorPK = @valor AND cedulaEmpleadoFK = @id";
            ViewBag.habilidades = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor), "cedulaEmpleadoFK", "tipoPK", "valorPK");

            var hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor);

            return View(hABILIDADES);
        }

        // POST: HABILIDADES/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string valor, string tipo)
        {

            //var hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor);
            //Console.WriteLine(id, valor);

            //HABILIDADES hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id);
            HABILIDADES hABILIDADES = await db.HABILIDADES.FindAsync(id, tipo, valor);
            db.HABILIDADES.Remove(hABILIDADES);
            await db.SaveChangesAsync();
            //ViewBag.habilidades = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor), "cedulaEmpleadoFK", "tipoPK", "valorPK");

            //return View(hABILIDADES);
            return RedirectToAction("Index", new { id = id });
        }

        public ActionResult to_empleados(string id)
        {
            return RedirectToAction("../EMPLEADO/Index", new { });
        }

        /*public async Task<ActionResult> DeleteConfirmed(string id, string valor, string tipo)
        {

            HABILIDADES habilidad = await db.HABILIDADES.FindAsync(id, valor, tipo);
            //HABILIDADES hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id);
            db.HABILIDADES.Remove(habilidad);
            await db.SaveChangesAsync();
            return RedirectToAction("../EMPLEADO/Index", new { id =id });
        }*/
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
