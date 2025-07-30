using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_LP3__Daylin_.Models;

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
            if (cita.fechaCita.Date < DateTime.Now.Date)
            {
                TempData["Mensaje"] = "No es posible agendar esta cita porque la fecha indicada no es correcta. Por favor verifique que sea una fecha a futuro.";
            }

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

            DateTime inicioCita = cita.fechaCita.Add(cita.horaCita);
            DateTime finCita = inicioCita.AddMinutes(cita.duracionCita);

            var citarepetida = _context.Citas.Where(c => c.idDentista == cita.idDentista
            && c.fechaCita == cita.fechaCita.Date).AsEnumerable().Any(c => inicioCita < c.fechaCita.Add
            (c.horaCita).AddMinutes((double)c.duracionCita) && finCita > c.fechaCita.Add(c.horaCita));

            if (citarepetida)
            {
                TempData["Mensaje"] = "El dentista tiene una cita en ese horario. Por favor indicar otro.";
            }
            else if (ModelState.IsValid)
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
    }
}
