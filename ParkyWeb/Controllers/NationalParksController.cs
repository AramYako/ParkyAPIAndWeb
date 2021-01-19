using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Helpers;
using ParkyWeb.Models;
using ParkyWeb.Models.Enums;
using ParkyWeb.Models.ModelObjects;
using ParkyWeb.Repository.IRepository.RepositoryClients.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {

        private readonly INationalParkRepository _npRepo;

        public NationalParksController(INationalParkRepository npRepo)
        {
            this._npRepo = npRepo;
        }

        public async Task<IActionResult> Index()
        {

            IEnumerable<NationalPark> parks =  await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));

            return View(parks);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark obj = new NationalPark(); 

            if (id is null)
            {
                return View(obj);
            }
            else
            {
                
                   obj = await _npRepo.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));

                if(obj is null)
                {
                    return NotFound();
                }

                return View(obj);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                if (files.Count() > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        files[0].CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        obj.Picture = fileBytes;
                    }
                }
                else
                {
                    var objFromDb = await _npRepo.GetAsync(SD.NationalParkAPIPath, obj.NationalParkId, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));

                    obj.Picture = objFromDb.Picture;
                }

                if (obj.NationalParkId == 0)
                    await _npRepo.CreateAsync(SD.NationalParkAPIPath, obj, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));
                else
                    await _npRepo.UpdateAsync(SD.NationalParkAPIPath, obj, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));
            }
            else
                return View(obj);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString())) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if(await _npRepo.DeleteAsync(SD.NationalParkAPIPath, id, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString())))
            {
                return Json(new { success = true, message = "Delete Successful" });
            }

            return Json(new { success = false, message = "Delete not Successful" });
        }


    }
}
