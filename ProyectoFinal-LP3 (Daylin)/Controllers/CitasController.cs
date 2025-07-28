using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index()
        {
            var model = new CitaViewModel()
            {
                Pacientes = _context.Pacientes
                .Select(p => new SelectListItem
                {
                    Value = p.IdPaciente.ToString(),
                    Text = p.nombrePaciente + "-" + p.cedulaPaciente
                }).ToList(),

                Dentistas = _context.Dentistas
                .Select(d => new SelectListItem
                {
                    Value = d.idDentista.ToString(),
                    Text = d.nombreDentista
                }).ToList(),

                Motivos = _context.Motivos
                .Select(m => new SelectListItem
                {
                    Value = m.idMotivo.ToString(),
                    Text = m.descripcionMotivo
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Registrar(CitaViewModel cita)
        {
            if (ModelState.IsValid)
            {
                cita.Pacientes = _context.Pacientes
                .Select(p => new SelectListItem
                {
                    Value = p.IdPaciente.ToString(),
                    Text = p.nombrePaciente + "-" + p.cedulaPaciente
                }).ToList();

                cita.Dentistas = _context.Dentistas
                .Select(d => new SelectListItem
                {
                    Value = d.idDentista.ToString(),
                    Text = d.nombreDentista
                }).ToList();

                cita.Motivos = _context.Motivos
                .Select(m => new SelectListItem
                {
                    Value = m.idMotivo.ToString(),
                    Text = m.descripcionMotivo
                }).ToList();
                
                return View(cita);
            }

            DateTime inicioCita = cita.fechaCita.Add(cita.horaCita);
            DateTime finCita = inicioCita.AddMinutes(cita.duracionCita);

            bool citarepetida = _context.Citas.Any(c => c.idDentista == cita.idDentista &&
            c.fechaCita == cita.fechaCita.Date && (inicioCita < c.fechaCita.Add(c.horaCita).AddMinutes(c.duracionCita) &&
            finCita > c.fechaCita.Add(c.horaCita)));

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
    }
}
