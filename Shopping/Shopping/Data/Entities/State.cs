using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.Data.Entities
{
    public class State
    {
            
        public int Id { get; set; }

        //Datanotation
        [Display(Name = "Estado o Alcaldia")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        public string Name { get; set; }


        //Con esta linea relacionas 
        //Jsonignore sirve para deserializar
        [JsonIgnore]
        public Country Country{ get; set; }

        public ICollection<City> Cities { get; set; }

        [Display(Name ="Ciudades")]
         public int CitiesNumber => Cities == null ? 0 : Cities.Count;
    }
}

