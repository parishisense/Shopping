using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class StateViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Display(Name = "Estado")]
        public string Name { get; set; }

        public int CountryId { get; set; }
    }
}
