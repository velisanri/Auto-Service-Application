using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelService.Entities
{
    public class Kullanici : IEntity
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Display(Name = "Ad"),Required(ErrorMessage = "{0} boş bırakılamaz")]
        public string Adi { get; set; }
        [StringLength(50)]
        [Display(Name = "Soyad"), Required(ErrorMessage = "{0} boş bırakılamaz")]
        public string Soyadi { get; set; }
        [StringLength(50), Required(ErrorMessage = "{0} boş bırakılamaz")]
        public string Email { get; set; }
        [StringLength(20)]
        public string? Telefon { get; set; }
        [StringLength(50)]
        public string? KullaniciAdi { get; set; }
        [Display(Name = "Şifre"),StringLength(50), Required(ErrorMessage = "{0} boş bırakılamaz")]
        public string Sifre { get; set; }
        public bool AktifMi { get; set; }
        [Display(Name = "Eklenme Tarihi"),ScaffoldColumn(false)]
        public DateTime? EklenmeTarihi { get; set; } = DateTime.Now;
        [Display(Name = "Kullanıcı Rolü"), Required(ErrorMessage = "{0} boş bırakılamaz")]
        public int RolId { get; set; }
        [Display(Name = "Kullanıcı Rolü")]
        public virtual Rol? Rol { get; set; }

        public Guid? UserGuid { get; set; } = Guid.NewGuid();
    }
}
