using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelService.Entities
{
    public class Servis : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Servise Geliş Tarihi")]
        public DateTime ServiseGelisTarihi { get; set; }
        [Display(Name = "Araç Sorunu"), Required(ErrorMessage = "{0} boş bırakılamaz")]
        public string AracSorunu { get; set; }
        [Display(Name = "Servis Ücreti")]
        public decimal ServisUcreti { get; set; }
        [Display(Name = "Servisten Çıkış Tarihi")]
        public DateTime ServistenCikisTarihi { get; set; }
        [Display(Name = "Yapulan İşlemler")]
        public string? YapilanIslemler { get; set; }
        [Display(Name = "Garanti Kapsamında mı?")]
        public bool GarantiKapsamindaMi { get; set; }
        [StringLength(15)]
        [Display(Name = "Araç Plaka"), Required(ErrorMessage = "{0} boş bırakılamaz")]
        public string AracPlaka { get; set; }
        [StringLength(50), Required(ErrorMessage = "{0} boş bırakılamaz")]

        public string Marka { get; set; }
        [StringLength(50)]

        public string? Model { get; set; }
        [StringLength(50)]
        [Display(Name = "Kasa Tipi")]
        public string? KasaTipi { get; set; }
        [StringLength(50)]
        [Display(Name = "Şase No")]
        public string? SaseNo { get; set; }
        [Required(ErrorMessage = "{0} boş bırakılamaz")]
        public string Notlar { get; set; }
    }
}
