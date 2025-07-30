using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_LP3__Daylin_.Models
{
    public class CitaViewModel
    {
        [Key]
        [Column("IDCITA")]
        public int idCita { get; set; }

        [Required(ErrorMessage ="Debes seleccionar un paciente para la cita.")]
        [Column("IDPACIENTE")]
        [Display(Name ="Nombre del paciente")]
        public int idPaciente { get; set; }

        [ForeignKey("idPaciente")]
        public PacienteViewModel Paciente { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Column("FECHA")]
        [Display(Name = "Fecha para la cita.")]
        public DateTime fechaCita { get; set; }

        [Required]
        [Column("HORA")]
        [Display(Name ="Hora de la cita.")]
        public TimeSpan horaCita { get; set; }

        [Required]
        [Column("DURACION")]
        [Display(Name ="Duración de la cita (en minutos).")]
        public int duracionCita { get; set; }

        [Required(ErrorMessage ="Debes seleccionar el nombre del dentista que antenderá al paciente.")]
        [Column("IDDENTISTA")]
        [Display(Name ="Nombre del dentista")]
        public int idDentista { get; set; }

        [ForeignKey("idDentista")]
        public DentistaViewModel Dentista { get; set; }

       [Required(ErrorMessage = "Debes seleccionar el motivo del paciente para agendar la cita.")]
        [Column("IDMOTIVO")]
        [Display(Name = "Motivo de la cita")]
        public int? idMotivo { get; set; }

        [ForeignKey("idMotivo")]
        public MotivoViewModel Motivo { get; set; }

        public string Estado => CalculoEstado();
        public TimeSpan TiempoRestante => fechaCita.Add(horaCita).Subtract(DateTime.Now);

        private string CalculoEstado()
        {
            var cita = fechaCita.Add(horaCita);
            if (DateTime.Now < cita) return "Cita vigente";
            if (DateTime.Now >= cita && DateTime.Now <= cita.AddMinutes(duracionCita)) return "Cita en proceso";
            return "Cita realizada con exito";
        }
    }
}
