using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Escuela.Models;

namespace Escuela.Controllers
{
    public class NotasController : Controller
    {
        private BD_EscuelaEntities db = new BD_EscuelaEntities();

        // GET: Notas
        public ActionResult Index()
        {
            var notas = db.Notas.Include(n => n.Alumno);
            return View(notas.ToList());
        }

        // GET: Notas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Notas.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }
            return View(nota);
        }

        // GET: Notas/Create
        public ActionResult Create()
        {
            ViewBag.ID_Alumno = new SelectList(db.Alumnos, "ID_Alumnos", "Nombre");
            return View();
        }

        // POST: Notas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Nota,Nota1,Nota2,Nota3,Materia,ID_Alumno")] Nota nota)
        {
            double n1 = nota.Nota1, n2 = nota.Nota2, n3=nota.Nota3;
            double prom=(n1+n2+n3)/3;
            nota.Promedio = prom;
            if (prom>=6 && prom <= 10)
            {
                nota.Estado = "APROBADO";
            }else if(prom<6 && prom>0)
            {
                nota.Estado = "REPROBADO";
            }
            else
            {
                nota.Estado = "Nota no valida";
            }
            
            
            if (ModelState.IsValid)
            {
                db.Notas.Add(nota);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Alumno = new SelectList(db.Alumnos, "ID_Alumnos", "Nombre", nota.ID_Alumno);
            return View(nota);
        }

        // GET: Notas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Notas.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Alumno = new SelectList(db.Alumnos, "ID_Alumnos", "Nombre", nota.ID_Alumno);
            return View(nota);
        }

        // POST: Notas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Nota,Nota1,Nota2,Nota3,Promedio,Materia,Estado,ID_Alumno")] Nota nota)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Alumno = new SelectList(db.Alumnos, "ID_Alumnos", "Nombre", nota.ID_Alumno);
            return View(nota);
        }

        // GET: Notas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Notas.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }
            return View(nota);
        }

        // POST: Notas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nota nota = db.Notas.Find(id);
            db.Notas.Remove(nota);
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
