using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using SuperCarBookingSystem.Models;
using SuperCarBookingSystem.Models.Repos;

namespace SuperCarBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBooking _booking;
        private readonly ICar _car;
        public BookingController(ILogger<BookingController> logger, IBooking booking, ICar car)
        {
            _logger = logger;
            _booking = booking;
            _car = car;
        }

        public IActionResult Index()
        {
            var model = _booking.GetAllBookings();
            return View(model);
        }

        public IActionResult Add(string carId)
        {
            var selectedCar = _car.GetCarById(new ObjectId(carId));
            Booking booking = new()
            {
                CarId = selectedCar.Id,
                CarModel = selectedCar.Model,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Booking booking)
        {
            _booking.AddBooking(booking);
            TempData["Message"] = "Car successfully booked!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var selectedBooking = _booking.GetBookingById(new ObjectId(Id));
            return View(selectedBooking);
        }

        [HttpPost]
        public IActionResult Edit(Booking booking)
        {
            try
            {
                var existingBooking = _booking.GetBookingById(booking.Id);
                if (existingBooking != null)
                {
                    _booking.EditBooking(existingBooking);
                    TempData["Message"] = "Booking successfully updated!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", $"Booking with ID {booking.Id} does not exist!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating the booking failed, please try again! Error: {ex.Message}");
            }

            return View(booking);
        }

        public IActionResult Delete(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var selectedBooking = _booking.GetBookingById(new ObjectId(Id));
            return View(selectedBooking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Booking booking)
        {
            if (booking.Id == null)
            {
                ViewData["ErrorMessage"] = "Deleting the booking failed, invalid ID!";
                return View();
            }

            try
            {
                _booking.DeleteBooking(booking);
                TempData["Message"] = "Booking deleted successfully";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting the booking failed, please try again! Error: {ex.Message}";
            }

            var selectedCar = _booking.GetBookingById(booking.Id);
            return View(selectedCar);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}