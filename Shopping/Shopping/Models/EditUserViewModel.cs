using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shopping.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name ="Documento")]
        [MaxLength(20, ErrorMessage = "El Campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage ="El campo {0} es Obligatorio")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El Campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        public string FirstName { get; set; }
        
        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El Campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        public string LastName { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El Campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        public string Address {  get; set; }

        [Display(Name = "Telefono")]
        [MaxLength(20, ErrorMessage = "El Campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        public string PhoneNumber { get; set; }

        [Display(Name ="Foto")]
        public Guid ImageId { get; set; }
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7148/images/NoImageBRAME.jpg"
            : $"https://shoppingparis.blob.core.windows.net/users/{ImageId}";

        [Display(Name ="Image")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name ="Pais")]
        [Range(1, int.MaxValue, ErrorMessage ="Debes Seleccionar un País")]
        [Required(ErrorMessage ="El Campo {0} es obligatorio")]
        public int CountryId { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

        [Display(Name ="Departamento/Estado")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes Seleccionar un Estado")]
        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        public int StateId { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }

        [Display(Name ="Ciudad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes Seleccionar una Ciudad")]
        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }


    }
}
