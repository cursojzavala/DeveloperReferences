using FormComplete.Models;
using FormComplete.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormComplete.Controllers
{
    public class AlumnoController : Controller
    {

        private cnxBase db = new cnxBase();


        // GET: Alumno
        public ActionResult Index()
        {

            var alumnos = db.Alumno.ToList();
                       
            return View(alumnos);
        }

        public ActionResult Registrar(int IdAlumno=0)
        {
            var modelo = new FormComplete.Models.Alumno();

            ViewBag.listaCursos = ListarCursos();

            return View(modelo);
        }

        [HttpPost]
        public ActionResult Registrar(FormComplete.Models.Alumno body)
        {

            if (!ModelState.IsValid)//Invalid
            {
                var listaCursos = ListarCursos();

                if (!(body.cursos == null))
                    foreach (var curso in listaCursos)
                    {
                        for (var i = 0; i < body.cursos.Count; i++)
                        {
                            if (body.cursos[i] == curso.IdCurso.ToString())
                            {
                                curso.Select = true;
                            }
                        }
                    }

                ViewBag.listaCursos = listaCursos;

                return View(body);
            }

            //Save

            var regAlumno = new ORM.Alumno()
            {
                Nombre = body.Nombre,
                Apellidos = body.Apellidos,
                Direccion = body.direccion,
                sexo = body.sexo
            };

            db.Alumno.Add(regAlumno);
            db.SaveChanges();

            var idAlumno = regAlumno.IdAlumno;

            var alumnoCurso = new AlumnoCurso();
            alumnoCurso.IdAlumno = idAlumno;

            foreach (var idCurso in body.cursos)
            {
                alumnoCurso.IdCurso = Convert.ToInt32(idCurso);
                db.AlumnoCurso.Add(alumnoCurso);
                db.SaveChanges();
            }

            return View("Index");
        }

        public List<Curso> ListarCursos()
        {
            var response = new List<Curso>();

            response.Add(new Curso() { IdCurso = 1, Nombre = "lenguage" });
            response.Add(new Curso() { IdCurso = 2, Nombre = "matematica" });
            response.Add(new Curso() { IdCurso = 3, Nombre = "historia" });
            response.Add(new Curso() { IdCurso = 4, Nombre = "filosofia" });
            response.Add(new Curso() { IdCurso = 5, Nombre = "computacion" });
            response.Add(new Curso() { IdCurso = 6, Nombre = "ingles" });

            return response;
        }



    }
}