using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_LP3__Daylin_.Models;
using System.Text;

namespace ProyectoFinal_LP3__Daylin_.Controllers
{
    public class MotivosController : Controller
    {
        private readonly AppDbContext _context;

        public MotivosController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registrar(MotivoViewModel motivo)
        {
            if(ModelState.IsValid)
            {
                _context.Motivos.Add(motivo);
                _context.SaveChanges();
                TempData["Mensaje"] = "El motivo ha sido registrado exitosamente.";
                return RedirectToAction("Lista");
            }
            return View("Index", motivo);
        }

        public IActionResult Lista()
        {
            var motivo = _context.Motivos.ToList();
            return View(motivo);
        }
        [HttpGet]
        public IActionResult Editar(int idm)
        {
            var motivo = _context.Motivos.FirstOrDefault(m => m.idMotivo == idm);
            if (motivo == null)
            {
                TempData["Mensaje"] = "No existe el motivo indicado";
                return RedirectToAction("Lista");
            }
            return View(motivo);
        }

        [HttpPost]
        public IActionResult Editar(MotivoViewModel motivo)
        {
            if (ModelState.IsValid)
            {
                var motivoActual = _context.Motivos.FirstOrDefault(m => m.idMotivo == motivo.idMotivo);

                if (motivoActual == null)
                {
                    TempData["Mensaje"] = "No existe el motivo seleccionado anteriormente";
                    return RedirectToAction("Lista");
                }

                motivoActual.descripcionMotivo = motivo.descripcionMotivo;

                _context.Update(motivoActual);
                _context.SaveChanges();

                TempData["Mensaje"] = "El texto ha sido editado con exito.";
                return RedirectToAction("Lista");
            }
            return View(motivo); 
        }

        public IActionResult Eliminar(int idm)
        {
            var motivo = _context.Motivos.FirstOrDefault(m => m.idMotivo == idm);
            if(motivo != null)
            {
                _context.Motivos.Remove(motivo);
                _context.SaveChanges();
                TempData["Mensaje"] = "El motivo ha sido eliminado con exito.";
                return RedirectToAction("Lista");
            }
            return View(motivo);
        }

        public ActionResult ExportarCSV()
        {
            var motivos = _context.Motivos.ToList();

            var csv = new StringBuilder();
            csv.AppendLine("\"Id\",\"Descripción del motivo\"");

            foreach (var m in motivos)
            {
                csv.AppendLine($"\"{m.idMotivo}\",\"{m.descripcionMotivo}\"");
            }

            var bom = Encoding.UTF8.GetPreamble();
            var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
            var finalBytes = bom.Concat(csvBytes).ToArray();

            return File(finalBytes, "text/csv", "Motivos.csv");
        }
    }
}
