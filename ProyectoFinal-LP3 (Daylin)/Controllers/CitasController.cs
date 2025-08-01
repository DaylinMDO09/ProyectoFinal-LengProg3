using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_LP3__Daylin_.Models;
using System.Text;

namespace ProyectoFinal_LP3__Daylin_.Controllers
{
    public class CitasController : Controller
    {
        private readonly AppDbContext _context;

        public CitasController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Pacientes = _context.Pacientes
            .Select(p => new SelectListItem
            {
                Value = p.IdPaciente.ToString(),
                Text = p.nombrePaciente + " - " + p.cedulaPaciente
            }).ToList();

            ViewBag.Dentistas = _context.Dentistas
            .Select(d => new SelectListItem
            {
                Value = d.idDentista.ToString(),
                Text = d.nombreDentista
            }).ToList();

            ViewBag.Motivos = _context.Motivos
            .Select(m => new SelectListItem
            {
                Value = m.idMotivo.ToString(),
                Text = m.descripcionMotivo
            }).ToList();
            
            return View(new CitaViewModel());
        }

        [HttpPost]
        public IActionResult Registrar(CitaViewModel cita)
        {
            ViewBag.Pacientes = _context.Pacientes
                .Select(p => new SelectListItem
                {
                    Value = p.IdPaciente.ToString(),
                    Text = p.nombrePaciente + " - " + p.cedulaPaciente
                }).ToList();

            ViewBag.Dentistas = _context.Dentistas
            .Select(d => new SelectListItem
            {
                Value = d.idDentista.ToString(),
                Text = d.nombreDentista
            }).ToList();

            ViewBag.Motivos = _context.Motivos
            .Select(m => new SelectListItem
            {
                Value = m.idMotivo.ToString(),
                Text = m.descripcionMotivo
            }).ToList();

            if (cita.fechaCita.Date < DateTime.Now.Date)
            {
                TempData["Mensaje"] = "No es posible agendar esta cita porque la fecha indicada no es correcta. Por favor verifique que sea una fecha a futuro.";
                return View("Index", cita);
            }

            DateTime inicioCita = cita.fechaCita.Add(cita.horaCita);
                DateTime finCita = inicioCita.AddMinutes(cita.duracionCita);

                var citarepetida = _context.Citas.Where(c => c.idDentista == cita.idDentista
                && c.fechaCita == cita.fechaCita.Date).AsEnumerable().Any(c => inicioCita < c.fechaCita.Add
                (c.horaCita).AddMinutes((double)c.duracionCita) && finCita > c.fechaCita.Add(c.horaCita));

                if (citarepetida)
                {
                    TempData["Mensaje"] = "El dentista tiene una cita en ese horario. Por favor indicar otro.";
                }                
                else if (!ModelState.IsValid)
                {
                    _context.Citas.Add(cita);
                    _context.SaveChanges();
                    TempData["Mensaje"] = "Citas agregada correctamente.";
                    return RedirectToAction("Lista");
                }         
            return View("Index", cita);
        }
        

        public IActionResult Lista()
        {
            var cita = _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Dentista)
            .Include(c => c.Motivo)
             .ToList();

            return View(cita);
            //var cita = _context.Citas.ToList();
            //return View(cita);
        }

        [HttpGet]
        public IActionResult Editar(int idc)
        {
            ViewBag.Pacientes = _context.Pacientes
            .Select(p => new SelectListItem
            {
                Value = p.IdPaciente.ToString(),
                Text = p.nombrePaciente + " - " + p.cedulaPaciente
            }).ToList();

            ViewBag.Dentistas = _context.Dentistas
            .Select(d => new SelectListItem
            {
                Value = d.idDentista.ToString(),
                Text = d.nombreDentista
            }).ToList();

            ViewBag.Motivos = _context.Motivos
            .Select(m => new SelectListItem
            {
                Value = m.idMotivo.ToString(),
                Text = m.descripcionMotivo
            }).ToList();
            var cita = _context.Citas.FirstOrDefault(c => c.idCita == idc);
            if (cita == null)
            {
                TempData["Mensaje"] = "No existe la cita indicada.";
                return RedirectToAction("Lista");
            }
            return View(cita); 
        }

        [HttpPost]
        public IActionResult Editar(CitaViewModel cita)
        {
            ViewBag.Pacientes = _context.Pacientes
            .Select(p => new SelectListItem
            {
                Value = p.IdPaciente.ToString(),
                Text = p.nombrePaciente + " - " + p.cedulaPaciente
            }).ToList();

            ViewBag.Dentistas = _context.Dentistas
            .Select(d => new SelectListItem
            {
                Value = d.idDentista.ToString(),
                Text = d.nombreDentista
            }).ToList();

            ViewBag.Motivos = _context.Motivos
            .Select(m => new SelectListItem
            {
                Value = m.idMotivo.ToString(),
                Text = m.descripcionMotivo
            }).ToList();

            if (cita.fechaCita.Date < DateTime.Now.Date)
            {
                TempData["Mensaje"] = "No es posible editar la fecha de esta agenda porque la fecha no es correcta. Por favor verifique que sea una fecha a futuro.";
                return View("Editar", cita);
            }

            DateTime inicioCita = cita.fechaCita.Add(cita.horaCita);
            DateTime finCita = inicioCita.AddMinutes(cita.duracionCita);

            var citarepetida = _context.Citas.Where(c => c.idDentista == cita.idDentista
            && c.fechaCita == cita.fechaCita.Date).AsEnumerable().Any(c => inicioCita < c.fechaCita.Add
            (c.horaCita).AddMinutes((double)c.duracionCita) && finCita > c.fechaCita.Add(c.horaCita) && c.idCita == cita.idCita);


            if (citarepetida)
            {
                TempData["Mensaje"] = "El dentista tiene una cita en ese horario. Por favor indicar otro.";
            }
            else if (!ModelState.IsValid)
            {
                var citaActual = _context.Citas.FirstOrDefault(c => c.idCita == cita.idCita);
                if (citaActual == null)
                {
                    TempData["Mensaje"] = "La cita seleccionada no  ha podido ser modificada.";
                    return RedirectToAction("Lista");
                }

                citaActual.fechaCita = cita.fechaCita;
                citaActual.horaCita = cita.horaCita;
                citaActual.duracionCita = cita.duracionCita;
                citaActual.Motivo = cita.Motivo;

                _context.Citas.Update(citaActual);
                _context.SaveChanges();
                TempData["Mensaje"] = "Cita modificada correctamente.";
                return RedirectToAction("Lista");
            }
            return View(cita);
        }

        public IActionResult Eliminar(int idc)
        {
            var cita = _context.Citas.FirstOrDefault(c => c.idCita == idc);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
                _context.SaveChanges();
                TempData["Mensaje"] = "La cita ha sido cancelada y eliminada con exito.";
                return RedirectToAction("Lista");
            }
            return View(cita);
        }

        public ActionResult ExportarCSV()
        {
            var citas = _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Dentista)
            .Include(c => c.Motivo)
             .ToList();

            var csv = new StringBuilder();
            csv.AppendLine("\"Id\",\"Datos del paciente\",\"Fecha de la cita\",\"Hora de la cita\",\"Duración (en minutos)\",\"Nombre del dentista\",\"Motivo de la visita\",\"Estado de la cita\"");

            foreach (var c in citas)
            {
                csv.AppendLine($"\"{c.idCita}\",\"{c.Paciente.nombrePaciente + " - " + c.Paciente.cedulaPaciente}\",\"{c.fechaCita.ToShortDateString()}\",\"{c.horaCita}\",\"{c.duracionCita}\",\"{c.Dentista.nombreDentista}\",\"{c.Motivo.descripcionMotivo}\",\"{c.Estado}\"");
            }

            var bom = Encoding.UTF8.GetPreamble();
            var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
            var finalBytes = bom.Concat(csvBytes).ToArray();

            return File(finalBytes, "text/csv", "Citas Agendadas.csv");
        }
    }
}
