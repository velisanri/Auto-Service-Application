using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonelService.Entities;
using PersonelService.Service.Abstract;
using PersonelService.WebUI.Utils;

namespace PersonelService.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminPolicy")]
    public class SlidersController : Controller
    {
        private readonly IService<Slider> _service;

        public SlidersController(IService<Slider> service)
        {
            _service = service;
        }



        // GET: SlidersController
        public async Task<ActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: SlidersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SlidersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SlidersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Slider slider, IFormFile? Resim)
        {
            try
            {
                slider.Resim = await FileHelper.FileLoaderAsync(Resim, "/Img/Slider/");
                await _service.AddAsync(slider);
                await _service.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SlidersController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var data = await _service.FindAsync(id);

            return View(data);
        }

        // POST: SlidersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Slider slide,IFormFile? Resim)
        {
            try
            {
                if(Resim is not null)
                    slide.Resim = await FileHelper.FileLoaderAsync(Resim, "/Img/Slider/");


                _service.Update(slide);
                await _service.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SlidersController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var data = await _service.FindAsync(id);

            return View(data);
        }

        // POST: SlidersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Slider slide)
        {
            try
            {
                _service.Delete(slide);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
