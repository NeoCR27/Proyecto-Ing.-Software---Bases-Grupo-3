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

    public class HABILIDADESController : Controller
    {

        private EMPLEADOController emp_controller = new EMPLEADOController();
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        private static string valor_viejo = null;
        private static string tipo_viejo = null;
        private static string id_viejo = null;

        // GET: HABILIDADES
        public ActionResult Index(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            ViewBag.habilidades = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id), "", "valorPK", "tipoPK");

            SelectList nombre = emp_controller.get_nombres(id);
            ViewBag.nombre = nombre;

            return View();
        }

        // GET: HABILIDADES/Create
        public ActionResult Create()
        {
            //var hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == "1");
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
            HABILIDADES duplicate =  db.HABILIDADES.Find(hABILIDADES.valorPK, hABILIDADES.tipoPK, hABILIDADES.cedulaEmpleadoFK);
            //HABILIDADES habilidad_viejas = await db.HABILIDADES.FindAsync(valor_viejo, tipo_viejo, id_viejo);
            if (duplicate == null)
            {
                if (ModelState.IsValid)
                {
                    db.HABILIDADES.Add(hABILIDADES);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = hABILIDADES.cedulaEmpleadoFK });
                }
            }
            //else ERROR
            

            //ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", hABILIDADES.cedulaEmpleadoFK);
            return RedirectToAction("Index", new { id =  hABILIDADES.cedulaEmpleadoFK});
        }

        // GET: HABILIDADES/Edit/5
        public ActionResult Edit(string id, string valor, string tipo)
        {
            string[] values = new[] { "Tecnica", "Blanda" };
            ViewBag.tipoPK = new SelectList(values);

            id_viejo = string.Copy(id);
            valor_viejo = string.Copy(valor);
            tipo_viejo = string.Copy(tipo);

            if (id == null && valor == null && tipo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.habilidades = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor), "cedulaEmpleadoFK", "tipoPK", "valorPK");
            ViewBag.ced = id;

            var hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor);

            return View(hABILIDADES);
        }

        // POST: HABILIDADES/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598. [Bind(Include = "valorPK,tipoPK,cedulaEmpleadoFK")] HABILIDADES hABILIDADES 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "valorPK,tipoPK,cedulaEmpleadoFK")] HABILIDADES hABILIDADES)
        {
            HABILIDADES habilidad_viejas = await db.HABILIDADES.FindAsync(valor_viejo, tipo_viejo, id_viejo);

            var query = from var_old in db.HABILIDADES
                        where var_old.cedulaEmpleadoFK == id_viejo
                        && var_old.valorPK == valor_viejo
                        && var_old.tipoPK == tipo_viejo
                        select var_old;
            foreach (var index in query)
            {
                db.HABILIDADES.Remove(index);
            }

            //db.HABILIDADES.Remove(habilidad_viejas);
            await db.SaveChangesAsync();

            //HABILIDADES habilidade = db.HABILIDADES.Find(valor, tipo, id);
            db.HABILIDADES.Add(hABILIDADES);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { id = hABILIDADES.cedulaEmpleadoFK });
        }

        // GET: HABILIDADES/Delete/5
        public ActionResult Delete(string id, string valor, string tipo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.habilidades = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor), "cedulaEmpleadoFK", "tipoPK", "valorPK");
            ViewBag.ced = id;
            var hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor);

            return View(hABILIDADES);
        }

        // POST: HABILIDADES/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string valor, string tipo)
        {

            HABILIDADES habilidade = db.HABILIDADES.Find(valor, tipo, id);
            db.HABILIDADES.Remove(habilidade);
            db.SaveChanges();

            return RedirectToAction("Index", new { id = id });
        }

        public ActionResult to_empleados()
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
