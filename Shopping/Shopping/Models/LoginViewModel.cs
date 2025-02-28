using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class LoginViewModel
    {
        [Display(Name ="Email")]
        [Required(ErrorMessage ="El Capo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage ="Debes Ingresar un Correo Válido")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Contraseña")]
        [Required(ErrorMessage ="El Campo {0} es obligatorio.")]
        [MinLength(6, ErrorMessage ="El Campo {0} debe tener al menos {1} caracteres")]
        public string Password { get; set; }

        [Display(Name ="Recordarme en este Navegador")]
        public bool RememberMe { get; set; }
    }
}
