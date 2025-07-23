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
        [Column("CEDULA")]
        [Display(Name = "Cédula del paciente")]
        public string cedulaPaciente { get; set;}

        [Phone(ErrorMessage = "El número telefónico del paciente debe ser ingresado")]
        [MinLength(10)]
        [Column("TELEFONOPACIENTE")]
        [Display(Name = "Numero de telefono")]
        public string telefonoPaciente { get; set;}
    }
}
