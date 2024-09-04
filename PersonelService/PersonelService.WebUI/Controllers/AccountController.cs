using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using PersonelService.Entities;
using PersonelService.Service.Abstract;
using PersonelService.WebUI.Models;
using System.Security.Claims;

namespace PersonelService.WebUI.Controllers
{

    public class AccountController : Controller
    {
        private readonly IUserService _service;
        private readonly IService<Rol> _serviceRol;
        private readonly ICarService _serviceArac;
        private readonly IMemoryCache _cache;

        public AccountController(IService<Rol> serviceRol, IUserService service, ICarService serviceArac, IMemoryCache cache)
        {
            _serviceRol = serviceRol;
            _service = service;
            _serviceArac = serviceArac;
            _cache = cache;
        }

        [Authorize(Policy ="CustomerPolicy")]
        public IActionResult Index()
        {

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var uguid = User.FindFirst(ClaimTypes.UserData)?.Value;

            if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(uguid)) 
            {
                var user = _service.Get(k=>k.Email == email && k.UserGuid.ToString() == uguid);

                if (user != null) 
                {
                    return View(user);
                }
            }

            return NotFound();
        }


        [HttpPost]
        public IActionResult UserUpdate(Kullanici kullanici)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var uguid = User.FindFirst(ClaimTypes.UserData)?.Value;

                if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(uguid))
                {
                    var user = _service.Get(k => k.Email == email && k.UserGuid.ToString() == uguid);

                    if (user != null)
                    {
                        user.Adi = kullanici.Adi;
                        user.AktifMi = kullanici.AktifMi;
                        user.Email = kullanici.Email;
                        user.UserGuid = kullanici.UserGuid;
                        user.Sifre = kullanici.Sifre;
                        user.EklenmeTarihi = kullanici.EklenmeTarihi;
                        user.Soyadi = kullanici.Soyadi;
                        user.Telefon = kullanici.Telefon;

                        _service.Update(user);
                        _service.Save();
                    }
                }
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Hata oluştu");
            }

           

            return RedirectToAction("Index");
        }        


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rol = await _serviceRol.GetAsync(r => r.Adi=="Customer");

                    if(rol == null)
                    {
                        ModelState.AddModelError("", "Kayıt Başarısız");
                        return View();
                    }

                    kullanici.RolId = rol.Id;
                    kullanici.AktifMi = true;
                    await _service.AddAsync(kullanici);
                    await _service.SaveAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata oluştu!");
                }
            }
            
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(CustomerLoginViewModel customerViewModel)
        {

                try
                {
                    var account =  _service.Get(k => k.Email == customerViewModel.Email && k.Sifre == customerViewModel.Sifre && k.AktifMi == true);

                    if (account == null)
                    {
                        ModelState.AddModelError("", "Giriş Başarısız");
                    }
                    else
                    {
                        var rol = _serviceRol.Get(r => r.Id == account.RolId);

                        var claims = new List<Claim>()
                        {
                        new Claim(ClaimTypes.Name,account.Adi),
                        new Claim(ClaimTypes.Email,account.Email),
                        new Claim(ClaimTypes.UserData,account.UserGuid.ToString()),
                        };

                        if (rol is not null)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, rol.Adi));
                        }


                        var userIdentity = new ClaimsIdentity(claims, "Login");

                        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                        await HttpContext.SignInAsync(principal);

                        if (rol.Adi == "Admin")
                        {
                            return Redirect("/Admin");
                        }

                        return Redirect("/Account");

                    }

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluştu");
                }

            return View();
            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult RedirectToAdminWebsite()
        {
            return RedirectToAction("Index", "Main", new { area = "Admin" });
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CacheData()
        {
            try
            {
                var sliders = await _service.GetAllAsync();
                var araclar = await _serviceArac.GetCustomCarList(a => a.Anasayfa);

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _cache.Set("Sliders", sliders, cacheEntryOptions);
                _cache.Set("Araclar", araclar, cacheEntryOptions);

                TempData["Message"] = "Veriler başarıyla önbelleğe alındı!";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Önbelleğe alma sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

    }
}
