using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.Enums;
using ParkyWeb.Models.ModelObjects;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository.RepositoryClients.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailsRepo;
        private readonly IAccountRepository _accountRepo;


        public HomeController(ITrailRepository trailsRepo, INationalParkRepository npRepo, IAccountRepository accountRepo)
        {
            //branchyv2
            this._trailsRepo = trailsRepo;
            this._npRepo = npRepo;
            this._accountRepo = accountRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM ListOfParksAndTrails = new IndexVM()
            {
                Trails = await _trailsRepo.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString())),
                NationalParks = await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString(HttpContextKeys.JWTToken.ToString()))
        };

            return View(ListOfParksAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User userObj)
        {
            User Obj = await this._accountRepo.LoginAsync(SD.AccountAPIPath+ "authenticate/", userObj);

            if (string.IsNullOrEmpty(Obj.Token))
                return View();


            HttpContext.Session.SetString(HttpContextKeys.JWTToken.ToString(), Obj.Token);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User userObj)
        {
           bool IsRegistred = await this._accountRepo.RegisterAsync(SD.AccountAPIPath + "register/", userObj);

            if (!IsRegistred)
                return View();

            HttpContext.Session.SetString("JWTtoken", userObj.Token);

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {

            HttpContext.Session.SetString(HttpContextKeys.JWTToken.ToString(), "");

            return RedirectToAction("Index");
        }
    }
}
