using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shopping.Enums;

namespace Shopping.Models
{
    public class AddUserViewModel: EditUserViewModel
    {
        [Display(Name ="Email")]
        [EmailAddress(ErrorMessage ="Debes Ingresar un correo Válido")]
        [MaxLength(100, ErrorMessage ="El Campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage ="El Campo {0} es obligatorio")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage ="El Campo {0} es obligatorio")]
        [StringLength(20, MinimumLength = 6, ErrorMessage ="El campo {0} debe tener entre {2} y {1} caracteres")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "La contraseña y confrimacion no son Iguales")]
        [Display(Name = "Confirmacion de contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres")]
        public string PasswordConfirm { get; set; }

        [Display(Name ="Tipo de Usuario")]
        public UserType UserType { get; set; }

    }
}
