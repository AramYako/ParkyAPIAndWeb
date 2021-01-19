using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Helpers;
using ParkyWeb.Models;
using ParkyWeb.Models.Enums;
using ParkyWeb.Models.ModelObjects;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository.RepositoryClients.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class TrailsParksController : Controller
    {

        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;

        public TrailsParksController(INationalParkRepository npRepo, ITrailRepository _trailRepo)
        {
            this._npRepo = npRepo;
            this._trailRepo = _trailRepo;
   
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Trail> trails = await _trailRepo.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));
            return View(trails);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));

            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.NationalParkId.ToString()
                }),

                Trail = new Trail()
            };
        
            if (id is null)
            {
                return View(objVM);
            }
            else
            {
                objVM.Trail = await _trailRepo.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));

                if(objVM.Trail is null)
                {
                    return NotFound();
                }

                return View(objVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Trail.TrailId == 0)
                    await _trailRepo.CreateAsync(SD.TrailAPIPath, obj.Trail, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));
                else
                    await _trailRepo.UpdateAsync(SD.TrailAPIPath, obj.Trail, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()));

                TrailsVM objVM = new TrailsVM()
                {
                    NationalParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.NationalParkId.ToString()
                    }),

                    Trail = obj.Trail
                };
                return View(objVM);

            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString())) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if(await _trailRepo.DeleteAsync(SD.TrailAPIPath, id, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString())))
            {
                return Json(new { success = true, message = "Delete Successful" });
            }

            return Json(new { success = false, message = "Delete not Successful" });
        }


    }
}
