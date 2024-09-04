using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.Web.CodeGeneration;
using PersonelService.Entities;
using PersonelService.Service.Abstract;
using PersonelService.WebUI.Models;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Security.Claims;
using IronBarCode;



namespace PersonelService.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<Slider> _service;
        private readonly ICarService _serviceArac;
        private readonly IService<Musteri> _serviceMusteri;
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _environment;

        public HomeController(IService<Slider> service, ICarService serviceArac, IService<Musteri> serviceMusteri, IUserService userService, IMemoryCache cache, IWebHostEnvironment environment)
        {
            _service = service;
            _serviceArac = serviceArac;
            _serviceMusteri = serviceMusteri;
            _userService = userService;
            _cache = cache;
            _environment = environment;
        }

        public async Task<IActionResult> IndexAsync(int? id)
        {
            var model = new HomePageViewModel()
            {
                Sliders = await _service.GetAllAsync(),
                Araclar = await _serviceArac.GetCustomCarList(a => a.Anasayfa)

            };
            if (id.HasValue)
            {
                var arac = await _serviceArac.GetCustomCar(id.Value);

                if (arac != null)
                {
                    // ID'ye sahip aracı modele ekle
                    model.Arac = arac;
                }
            }

            // Kullanıcı kimlik doğrulamasını kontrol et
            if (User.Identity.IsAuthenticated)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var uguid = User.FindFirst(ClaimTypes.UserData)?.Value;

                if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(uguid))
                {
                    var user = _userService.Get(k => k.Email == email && k.UserGuid.ToString() == uguid);

                    if (user != null)
                    {
                        model.Musteri = new Musteri
                        {
                            Adi = user.Adi,
                            Soyadi = user.Soyadi,
                            Email = user.Email,
                            Telefon = user.Telefon
                        };
                    }
                }
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public async Task<IActionResult> Contact()
        {
            ViewBag.AracId = new SelectList(await _serviceArac.GetAllAsync(), "Id", "Modeli");

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ContactKayit(Musteri musteri)
        {
            if (musteri == null)
            {
                ModelState.AddModelError("", "Müşteri bilgisi alınamadı.");
                return View("Contact", musteri);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (musteri.AracId.HasValue)
                    {
                        var arac = await _serviceArac.GetByIdAsync(musteri.AracId.Value);
                        if (arac == null)
                        {
                            ModelState.AddModelError("", "Araç bulunamadı.");
                            return View("Contact", musteri);
                        }

                        musteri.Arac = arac;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Araç ID geçersiz.");
                        return View("Contact", musteri);
                    }

                    await _serviceMusteri.AddAsync(musteri);
                    await _serviceMusteri.SaveAsync();


                    return RedirectToAction("Index", new { id = musteri.AracId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Hata oluştu: " + ex.Message);
                }
            }
            ViewBag.AracId = new SelectList(await _serviceArac.GetAllAsync(), "Id", "Modeli");
            return View("Contact", musteri);
        }


        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<List<SelectListItem>> GetAracOptionsAsync()
        {
            var aracList = await _serviceArac.GetAllAsync();
            return aracList.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Modeli,
            }).ToList();
        }

        public async Task<IActionResult> CreateQRCode()
        {
            var model = new QRCodeModel
            {
                AracOptions = await GetAracOptionsAsync()
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateQRCode(QRCodeModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.SelectedAracId))
                {
                    var selectedCarUrl = $"https://Sanriotoservis.com/Arabam/{model.SelectedAracId}";

                    // QR kodunu oluştur
                    GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(selectedCarUrl);
                    string path = Path.Combine(_environment.WebRootPath, "GeneratedQRCode");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filePath = Path.Combine(_environment.WebRootPath, "GeneratedQRCode/qrcode.png");
                    barcode.SaveAsPng(filePath);
                    string fileName = Path.GetFileName(filePath);
                    string imageUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/GeneratedQRCode/" + fileName;
                    ViewBag.QrCodeUri = imageUrl;
                }
                else
                {
                    ModelState.AddModelError("", "Lütfen bir araç seçin.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Hata oluştu: " + ex.Message);
            }

            // Yeniden araba seçeneklerini sağla
            model.AracOptions = await GetAracOptionsAsync();
            return View(model);
        }
    }
}
