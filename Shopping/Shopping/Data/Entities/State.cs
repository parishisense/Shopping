using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Display(Name = "Estado")]
        public string Name { get; set; }

        public Country Country  { get; set; }

        public ICollection<City> Cities { get; set; }

        [Display(Name ="Estados")]
        public int CitiesNumber => Cities== null ? 0 : Cities.Count;
    }
}
