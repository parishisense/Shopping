using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class City
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Display(Name = "Ciudad")]
        public string Name { get; set; }

        public State State { get; set; }
    }
}
