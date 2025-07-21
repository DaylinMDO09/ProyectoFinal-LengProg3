using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_LP3__Daylin_.Models
{
    public class MotivoViewModel
    {
        [Key]
        [Column("IDMOTIVO")]
        public int idMotivo { get; set; }

        [Required(ErrorMessage ="Debes escribir un motivo.")]
        [StringLength(300)]
        [Column("DESCRIPCIONMOTIVO")]
        [Display(Name ="Descripción del motivo:")]
        public string descripcionMotivo { get; set; }
    }
}
