using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_LP3__Daylin_.Models;

namespace ProyectoFinal_LP3__Daylin_.Controllers
{
    public class LogInController : Controller
    {
        // Mostrar la vista del login
        public IActionResult Index()
        {
            return View();
        }

        // Procesar el inicio de sesión
        [HttpPost]
        public IActionResult Index(LogInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Usuario y contraseña de prueba
            if (model.Usuario == "admin" && model.Password == "1234")
            {
                HttpContext.Session.SetString("Usuario", model.Usuario);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View(model);
        }

        // Cerrar sesión
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}