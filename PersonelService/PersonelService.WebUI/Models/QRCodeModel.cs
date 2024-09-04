using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class QRCodeModel
{
    [Display(Name = "QR Code için isim giriniz."), Required(ErrorMessage = "{0} boş bırakılamaz")]
    public string QRCodeText{ get; set; }

    [Display(Name = "QR Code oluşturacağınız arabayı seçiniz."), Required(ErrorMessage = "{0} boş bırakılamaz")]
    public string SelectedAracId { get; set; } 
    public List<SelectListItem> AracOptions { get; set; } 
}
