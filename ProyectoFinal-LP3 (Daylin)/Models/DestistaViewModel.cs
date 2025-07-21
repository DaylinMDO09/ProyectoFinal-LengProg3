using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_LP3__Daylin_.Models
{
    public class DestistaViewModel
    {
        [Key]
        [Column("IDDENTISTA")]
        public int idDentista { get; set; }

        [Required(ErrorMessage ="Debes introducir un nombre.")]
        [Column("NOMBREDENTISTA")]
        [Display(Name ="Nombre del dentista")]
        public string nombreDentista { get; set; }

        [Phone]
        [StringLength(15)]
        [Column("TELEFONODENTISTA")]
        [Display(Name = "Número telefónico del dentista")]
        public string telefonoDentista { get; set; }
    }
}
