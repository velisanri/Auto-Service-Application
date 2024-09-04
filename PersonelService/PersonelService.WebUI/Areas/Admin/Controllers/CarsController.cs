﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonelService.Entities;
using PersonelService.Service.Abstract;
using PersonelService.WebUI.Utils;

namespace PersonelService.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminPolicy")]
    public class CarsController : Controller 
    {
        private readonly ICarService _service;
        private readonly IService<Marka> _serviceMarka;

        public CarsController(ICarService service, IService<Marka> serviceMarka)
        {
            _service = service;
            _serviceMarka = serviceMarka;
        }


        // GET: CarsController
        public async Task<IActionResult> IndexAsync()
        {
            var model = await _service.GetCustomCarList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
               return Json(model);
            } 

            return View(model);
        }

        // GET: CarsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CarsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.MarkaId = new SelectList(await _serviceMarka.GetAllAsync(),"Id","Adi");
            return View();
        }

        // POST: CarsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Arac arac , IFormFile? Resim1, IFormFile? Resim2, IFormFile? Resim3)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    arac.Resim1 = await FileHelper.FileLoaderAsync(Resim1,"/Img/Cars/");
                    arac.Resim2 = await FileHelper.FileLoaderAsync(Resim2, filePath : "/Img/Cars/");
                    arac.Resim3 = await FileHelper.FileLoaderAsync(Resim3 , "/Img/Cars/" );

                    await _service.AddAsync(arac);
                    await _service.SaveAsync();
                    

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        // Başarı durumunda JSON döndür
                        return Json(new { success = true, redirectUrl = "/admin/cars", message = "Araç başarıyla eklendi." });
                    }

                    // Normal POST isteği için view döndür
                    return RedirectToAction(nameof(Index));

                }
                catch
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Hata oluştu!" });
                    }

                }
            }

            ViewBag.MarkaId = new SelectList(await _serviceMarka.GetAllAsync(), "Id", "Adi");
            return View(arac);
        }

        // GET: CarsController/Edit/5
        public async Task<IActionResult> EditAsync(int id)
        {
            ViewBag.MarkaId = new SelectList(await _serviceMarka.GetAllAsync(), "Id", "Adi");
            var model = await _service.FindAsync(id);
            return View(model);
        }

        // POST: CarsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Arac arac, IFormFile? Resim1, IFormFile? Resim2, IFormFile? Resim3)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Resim1 is not null)
                    {
                        arac.Resim1 = await FileHelper.FileLoaderAsync(Resim1, "/Img/Cars/");
                    }
                    if (Resim2 is not null)
                    {
                        arac.Resim2 = await FileHelper.FileLoaderAsync(Resim2, "/Img/Cars/");
                    }
                    if (Resim3 is not null)
                    {
                        arac.Resim3 = await FileHelper.FileLoaderAsync(Resim3, "/Img/Cars/");
                    }

                    _service.Update(arac);
                    await _service.SaveAsync();

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        // Başarı durumunda JSON döndür
                        return Json(new { success = true, redirectUrl = "/admin/cars", message = "Araç başarıyla eklendi." });
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Hata oluştu!" });
                    }
                }
            }

            ViewBag.MarkaId = new SelectList(await _serviceMarka.GetAllAsync(), "Id", "Adi");
            return View(arac);
        }

        // GET: CarsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _service.FindAsync(id);
            return View(model);
        }

        // POST: CarsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Arac arac)
        {
            try
            {
                _service.Delete(arac);
                await _service.SaveAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // Başarı durumunda JSON döndür
                    return Json(new { success = true, redirectUrl = "/admin/cars", message = "Araç başarıyla eklendi." });
                }

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Hata oluştu!" });
                }

                return View();
            }
        }
    }
}
