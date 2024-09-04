using Microsoft.AspNetCore.Mvc;
using PersonelService.Entities;
using PersonelService.Service.Abstract;
using PersonelService.WebUI.Models;
using PersonelService.WebUI.Utils;
using System.Security.Claims;

namespace PersonelService.WebUI.Controllers
{
    public class AracController : Controller
    {
        private readonly ICarService _serviceArac;
        private readonly IService<Musteri> _serviceMusteri;
        private readonly IUserService _service;

        public AracController(ICarService serviceArac, IService<Musteri> serviceMusteri, IUserService service)
        {
            _serviceArac = serviceArac;
            _serviceMusteri = serviceMusteri;
            _service = service;
        }

        public async Task<IActionResult> IndexAsync(int? id)
        {
            var arac = await _serviceArac.GetCustomCar(id.Value);

            if (id == null && arac == null)
                return NotFound();

            var model = new CarDetailViewModel();
            
            model.Arac = arac;

            if (User.Identity.IsAuthenticated)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var uguid = User.FindFirst(ClaimTypes.UserData)?.Value;

                if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(uguid))
                {
                    var user = _service.Get(k => k.Email == email && k.UserGuid.ToString() == uguid);

                    if (user != null)
                    {
                        model.Musteri = new Musteri
                        {
                            Adi= user.Adi,
                            Soyadi = user.Soyadi,
                            Email = user.Email,
                            Telefon = user.Telefon

                        };
                    }
                }
            }

            return View(model);
        }

        [Route("tum-araclarimiz")]
        public async Task<IActionResult> List()
        {
            var model = await _serviceArac.GetCustomCarList(c=>c.SatistaMi);
            return View(model);
        }

        public async Task<IActionResult> Ara(string q )
        {
            var model = await _serviceArac.GetCustomCarList(c => c.SatistaMi && c.Marka.Adi.Contains(q) || c.KasaTipi.Contains(q) || c.Modeli.Contains(q));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MusteriKayit(Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceMusteri.AddAsync(musteri);
                    await _serviceMusteri.SaveAsync();

                    TempData["Message"] = "<div class='alert alert-success' >Talebiniz Tamamlanmıştır. </div>";

                    return Redirect("/Arac/Index/" + musteri.AracId);
                }
                catch
                {
                    TempData["Message"] = "<div class='alert alert-danger' >Talebiniz Tamamlanamadı lütfen tekrar deneyiniz </div>";
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }


            return View();
        }

    }
}
