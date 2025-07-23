using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_LP3__Daylin_.Models;
using System.Text;

namespace ProyectoFinal_LP3__Daylin_.Controllers
{
    public class PacientesController : Controller
    {
        private readonly AppDbContext _context;

        public PacientesController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(PacienteViewModel paciente)
        {
            var cedulaexistente = _context.Pacientes.Any(p => p.cedulaPaciente == paciente.cedulaPaciente);
            if (cedulaexistente)
            {
                TempData["Mensaje"] = "Esta cédula ya está registrada.";
            }
            else if (ModelState.IsValid)
            {
                _context.Pacientes.Add(paciente);
                _context.SaveChanges();
                TempData["Mensaje"] = "Paciente registrado exitosamente.";
                return RedirectToAction("Lista");
            }
            return View("Index", paciente);
        }

        public IActionResult Lista() 
        {
            var paciente = _context.Pacientes.ToList();
            return View(paciente);
        }

        [HttpGet]
        public IActionResult Editar(int idp)
        {
            var paciente = _context.Pacientes.FirstOrDefault(p => p.IdPaciente == idp);
            if (paciente == null)
            {
                TempData["Mensaje"] = "No existe el paciente indicado";
                return RedirectToAction("Lista");
            }
            return View(paciente);
        }

        [HttpPost]
        public IActionResult Editar(PacienteViewModel paciente)
        {
            var cedulaexistente = _context.Pacientes.Any(p => p.cedulaPaciente == paciente.cedulaPaciente);
            if (cedulaexistente)
            {
                TempData["Mensaje"] = "Esta cedula es de otro paciente. Por favor revisar detenidamente los dígitos de la misma.";
            }
            else if (ModelState.IsValid)
            {
                var pacienteActual = _context.Pacientes.FirstOrDefault(p => p.IdPaciente == paciente.IdPaciente);

                if (pacienteActual == null) 
                {
                    TempData["Mensaje"] = "No existe el paciente indicado.";
                    return RedirectToAction("Lista");
                }

                pacienteActual.nombrePaciente = paciente.nombrePaciente;
                pacienteActual.cedulaPaciente = paciente.cedulaPaciente.ToString();
                pacienteActual.telefonoPaciente = paciente.telefonoPaciente;

                _context.Update(pacienteActual);
                _context.SaveChanges();

                TempData["Mensaje"] = "Los datos del paciente han sido modificados exitosamente.";
                return RedirectToAction("Lista");
            }
            return View(paciente);
        }

        public IActionResult Eliminar(int idp)
        {
            var paciente = _context.Pacientes.FirstOrDefault(p => p.IdPaciente == idp);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                _context.SaveChanges();
                TempData["Mensaje"] = "El paciente ha sido eliminado exitosamente.";
                return RedirectToAction("Lista");
            }
            return View(paciente); 
        }
        public ActionResult ExportarCSV()
        {
            var pacientes = _context.Pacientes.ToList();

            var csv = new StringBuilder();
            csv.AppendLine("\"Id\",\"Nombre del paciente\",\"Cédula del paciente\",\"N° telefónico del paciente\"");

            foreach (var p in pacientes)
            {
                csv.AppendLine($"\"{p.IdPaciente}\",\"{p.nombrePaciente}\",\"{p.cedulaPaciente}\",\"{p.telefonoPaciente}\"");
            }

            var bom = Encoding.UTF8.GetPreamble();
            var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
            var finalBytes = bom.Concat(csvBytes).ToArray();

            return File(finalBytes, "text/csv", "Pacientes.csv");
        }
    }
}
