using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_LP3__Daylin_.Models
{
    public class PacienteViewModel
    {
        [Key]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "El nombre del paciente debe ser ingresado")]
        [Column("NOMBREPACIENTE")]
        [Display(Name = "Nombre completo del paciente")]
        public string nombrePaciente { get; set; }

        [Required(ErrorMessage ="La cedula del paciente debe ser ingresada")]
        [MinLength(11, ErrorMessage = "La cédula debe contener 11 dígitos.")]
        [StringLength(11)]
        [Column("CEDULA")]
        [Display(Name = "Cédula del paciente")]
        public string cedulaPaciente { get; set;}

        [Phone]
        [Required(ErrorMessage = "El número telefónico del paciente debe ser ingresado")]
        [MinLength(10, ErrorMessage = "El número telefónico debe tener 10 dígitos")]
        [StringLength(10)]
        [Column("TELEFONOPACIENTE")]
        [Display(Name = "Numero de telefono")]
        public string telefonoPaciente { get; set;}
    }
}
