using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testColegio.Models;
using testColegio.ORM;

namespace testColegio.Controllers
{
    public class SolicitudController : Controller
    {

        private testColegioEntities db = new testColegioEntities();

        // GET: Solicitud
        public ActionResult Index()
        {

            return View();

        }
        public ActionResult Listado(string txtPeriodo = "",
            string txtFecha = "",
            string txtAlumno = "",
            string txtCurso = ""
            )
        {
            var listaResultado = (
                from solicitud in db.Solicitud
                join detalle in db.DetalleSolicitud on solicitud.IdSolicitud equals detalle.IdSolicitud
                join alumno in db.Alumno on solicitud.IdAlumno equals alumno.IdAlumno
                join curso in db.Cursos on detalle.IdCurso equals curso.IdCurso
                where (solicitud.Periodo == txtPeriodo || txtPeriodo == "") &&
                ((alumno.IdAlumno.ToString() == txtAlumno || alumno.Nombres == txtAlumno) || txtAlumno == "") &&
                (solicitud.Periodo == txtPeriodo || txtPeriodo == "") &&
                (curso.Nombre == txtCurso || txtCurso == "")

                select new
                {
                    alumno.Nombres,
                    solicitud.FechaSolicitud,
                    solicitud.CodRegistrante,
                    solicitud.Carrera,
                    solicitud.Periodo,
                    detalle.Sede,
                    cursoDescripcion = curso.Nombre,
                    detalle.Profesor
                }

               ).ToList();

            List<FiltroSolicitud> lstResult = new List<FiltroSolicitud>();

            foreach (var ele in listaResultado)
            {
                FiltroSolicitud data = new FiltroSolicitud();

                data.Nombres = ele.Nombres;
                data.FechaSolicitud = Convert.ToDateTime(ele.FechaSolicitud);
                data.CodRegistrante = ele.CodRegistrante;
                data.Carrera = ele.Carrera;
                data.Periodo = ele.Periodo;
                data.Sede = ele.Sede;
                data.cursoDescripcion = ele.cursoDescripcion;
                data.Profesor = ele.Profesor;

                lstResult.Add(data);
            }

            return View(lstResult);


        }
        public ActionResult Registro(int id = 0)
        {

            SolicitudView modelo = new SolicitudView();
            ViewBag.listaAlumnos = db.Alumno.ToList();
            ViewBag.listaCursos = db.Cursos.ToList().FindAll(x => x.Activo == true);


            return View(modelo);
        }

        [HttpPost]
        public ActionResult Registro(SolicitudView solicitud)
        {

            if (ModelState.IsValid)
            {
                var soli = new Solicitud();

                soli.IdAlumno = solicitud.IdAlumno;
                soli.FechaSolicitud = solicitud.FechaSolicitud;
                soli.CodRegistrante = solicitud.CodRegistrante;
                soli.Carrera = solicitud.Carrera;
                soli.Periodo = solicitud.Periodo;

                var solDeta = new DetalleSolicitud();


                db.Solicitud.Add(soli);
                db.SaveChanges();

                var id = soli.IdSolicitud;

                return RedirectToAction("Listado");
            }

            solicitud.alumnos = new testColegioEntities().Alumno.ToList();

            return View(solicitud);

        }


        [HttpPost]
        public JsonResult GrabarRegistro(SolicitudView solicitudd)
        {

            try
            {

                var listaCursos = db.Cursos.ToList();
                var listaSolitudes = db.Solicitud.ToList();


                if (listaSolitudes.Exists(x => x.IdAlumno == solicitudd.IdAlumno && x.Periodo == solicitudd.Periodo))
                {
                    return Json(new { respuesta = "2", msg = "No puede realizar mas de una solicitud por periodo" });
                }

                int cntCreditos = 0;


                foreach (var det in solicitudd.detalle)
                {
                    var nroCreditos = listaCursos.Find(x => x.IdCurso == det.Idcurso).NroCreditos;

                    cntCreditos = cntCreditos + Convert.ToInt32(nroCreditos);
                }

                if (cntCreditos > 20)
                {
                    return Json(new { respuesta = "2", msg = "No puede tener mas de 20 creditos por periodo" });
                }


                //Registro =============================================

                var soli = new Solicitud();

                soli.IdAlumno = solicitudd.IdAlumno;
                soli.FechaSolicitud = solicitudd.FechaSolicitud;
                soli.CodRegistrante = solicitudd.CodRegistrante;
                soli.Carrera = solicitudd.Carrera;
                soli.Periodo = solicitudd.Periodo;

                db.Solicitud.Add(soli);
                db.SaveChanges();

                var idSolicitud = soli.IdSolicitud;


                foreach (var det in solicitudd.detalle)
                {
                    var detalle = new DetalleSolicitud();

                    detalle.IdSolicitud = idSolicitud;
                    detalle.IdCurso = det.Idcurso;
                    detalle.Profesor = det.profesor;
                    detalle.Aula = det.aula;
                    detalle.Sede = det.sede;
                    detalle.Observacion = det.observacion;
                    db.DetalleSolicitud.Add(detalle);
                    db.SaveChanges();
                }


                return Json(new { respuesta = "1", msg = "OK" });

            }
            catch (Exception ex)
            {
                return Json(new { respuesta = "2", msg = ex.Message });
            }

        }



    }
}