using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_LP3__Daylin_.Models
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "Digite el usuario")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Digite la contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}