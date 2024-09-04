using System.ComponentModel.DataAnnotations;

namespace PersonelService.WebUI.Models
{
    public class CustomerLoginViewModel
    {
        [StringLength(50), Required(ErrorMessage = "{0} boş bırakılamaz")]
        public string Email { get; set; }

        [Display(Name = "Şifre"), StringLength(50), Required(ErrorMessage = "{0} boş bırakılamaz")]
        public string Sifre { get; set; }
    }
}
