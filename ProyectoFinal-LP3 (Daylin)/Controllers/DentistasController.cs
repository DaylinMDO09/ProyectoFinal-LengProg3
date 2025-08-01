using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_LP3__Daylin_.Models;
using System.Text;

namespace ProyectoFinal_LP3__Daylin_.Controllers
{
    public class DentistasController : Controller
    {
        private readonly AppDbContext _context;

        public DentistasController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(DentistaViewModel dentista)
        {
            var correodentista = _context.Dentistas.Any(d => d.correoDentista == dentista.correoDentista);
            if (correodentista)
            {
                TempData["Mensaje"] = "El correo escrito ya está asignado a un dentista. Intente con otro correo.";
            }
            else if(ModelState.IsValid) 
            {
                _context.Dentistas.Add(dentista);
                _context.SaveChanges();
                TempData["Mensaje"] = "Dentista registrado correctamente.";
                return RedirectToAction("Lista");
            }
            return View("Index", dentista);
        }
        
        public IActionResult Lista()
        {
            var dentista = _context.Dentistas.ToList();
            return View(dentista);
        }

        [HttpGet]
        public IActionResult Editar(int idd)
        {
            var dentista = _context.Dentistas.FirstOrDefault(d => d.idDentista == idd);
            if (dentista == null)
            {
                TempData["Mensaje"] = "No existe el doctor indicado";
                return RedirectToAction("Lista");
            }
            return View(dentista);
        }

        [HttpPost]
        public IActionResult Editar(DentistaViewModel dentista)
        {
            var correodentista = _context.Dentistas.Any(d => d.correoDentista == dentista.correoDentista && d.idDentista != dentista.idDentista);
            if (correodentista)
            {
                TempData["Mensaje"] = "El correo escrito ya está asignado a un dentista. Intente con otro correo.";
            }
            else if(ModelState.IsValid)
            {
                var dentistaActual = _context.Dentistas.FirstOrDefault(d => d.idDentista == dentista.idDentista);
                if(dentistaActual == null)
                {
                    TempData["Mensaje"] = "Los datos del dentista seleccionado no se han podido modificar.";
                    return RedirectToAction("Lista");
                }

                dentistaActual.nombreDentista = dentista.nombreDentista;
                dentistaActual.telefonoDentista = dentista.telefonoDentista;
                dentistaActual.correoDentista = dentista.correoDentista;

                _context.Update(dentistaActual);
                _context.SaveChanges();

                TempData["Mensaje"] = "Los datos del dentista han sido editados exitosamente.";
                return RedirectToAction("Lista");
            }
            return View(dentista);
        }

        public IActionResult Eliminar(int idd)
        {
            var dentista = _context.Dentistas.FirstOrDefault(d => d.idDentista == idd);
            if (dentista != null)
            {
                _context.Dentistas.Remove(dentista);
                _context.SaveChanges();
                TempData["Mensaje"] = "El dentista ha sido eliminado correctamente";
                return RedirectToAction("Lista");
            }
            return View(dentista);
        }
        public ActionResult ExportarCSV()
        {
            var dentistas = _context.Dentistas.ToList();

            var csv = new StringBuilder();
            csv.AppendLine("\"Id\",\"Nombre del dentista\",\"Correo electrónico del dentista\",\"N° telefónico del dentista\"");

            foreach (var d in dentistas)
            {
                csv.AppendLine($"\"{d.idDentista}\",\"{d.nombreDentista}\",\"{d.correoDentista}\",\"{d.telefonoDentista}\"");
            }

            var bom = Encoding.UTF8.GetPreamble();
            var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
            var finalBytes = bom.Concat(csvBytes).ToArray();

            return File(finalBytes, "text/csv", "Dentistas.csv");
        }

    }
}
