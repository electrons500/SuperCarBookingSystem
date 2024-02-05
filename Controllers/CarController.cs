using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using SuperCarBookingSystem.Models;
using SuperCarBookingSystem.Models.Repos;

namespace SuperCarBookingSystem.Controllers
{
    public class CarController : Controller
    {
        private readonly ILogger<CarController> _logger;
        private readonly ICar _car;
        public CarController(ILogger<CarController> logger, ICar car)
        {
            _logger = logger;
            _car = car;
        }

        public IActionResult Index()
        {
            var cars = _car.GetAllCars();

            return View(cars);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Car car)
        {
            if (ModelState.IsValid)
            {
                _car.AddCar(car);
                TempData["Message"] = "New car successfully Added!";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selectedCar = _car.GetCarById(new ObjectId(id));
            return View(selectedCar);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Car car)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _car.EditCar(car);
                    TempData["Message"] = $"{car.Model} successfully Update!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating the car failed, please try again! Error: {ex.Message}");
            }


            return View(car);
        }
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selectedCar = _car.GetCarById(new ObjectId(id));
            return View(selectedCar);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Car car)
        {
            try
            {
                if (car.Id != null)
                {
                    _car.DeleteCar(car);
                    TempData["Message"] = "Car successfully deleted!";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating the car failed, please try again! Error: {ex.Message}");
            }


            return View(car);
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}