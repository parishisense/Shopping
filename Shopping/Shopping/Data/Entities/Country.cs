using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }

        //Datanotation
        [Display(Name = "País")]
        [MaxLength(50, ErrorMessage ="El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage ="El Campo {0} es obligatorio")]
        public string Name { get; set; }


        //Relacion entrte entidades Country y states
        public ICollection<State> States{ get; set; }

        [Display(Name = "Estados o Alcaldia")]
        public int StatesNumber => States== null ? 0 : States.Count;

        [Display(Name = "Ciudades")]
        public int CitiesNumber => States == null ? 0 : States.Sum(s => s.CitiesNumber);

    }
}
