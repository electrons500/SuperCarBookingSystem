using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using SuperCarBookingSystem.Models.Repos;

namespace SuperCarBookingSystem.Models.Services
{
    public class CarService : ICar
    {
        private readonly ILogger<CarService> _logger;
        private readonly CarBookingDbContext _carDbContext;

        public CarService(ILogger<CarService> logger, CarBookingDbContext context)
        {
            _logger = logger;
            _carDbContext = context;
        }


        public void AddCar(Car car)
        {
            _carDbContext.Cars.Add(car);

            _carDbContext.ChangeTracker.DetectChanges();
            _logger.LogInformation(_carDbContext.ChangeTracker.DebugView.LongView);

            _carDbContext.SaveChanges();
        }

        public void DeleteCar(Car car)
        {
            var carToDelete = _carDbContext.Cars.Where(c => c.Id == car.Id).FirstOrDefault();

            if(carToDelete != null) {
            _carDbContext.Cars.Remove(carToDelete);
            _carDbContext.ChangeTracker.DetectChanges();
            _logger.LogInformation(_carDbContext.ChangeTracker.DebugView.LongView);
            _carDbContext.SaveChanges();
            }
            else {
                throw new ArgumentException("The car to delete cannot be found.");
            }
        }

        public void EditCar(Car car)
        {
             var carToUpdate = _carDbContext.Cars.FirstOrDefault(c => c.Id == car.Id);

            if(carToUpdate != null)
            {                
                carToUpdate.Model = car.Model;
                carToUpdate.NumberPlate = car.NumberPlate;
                carToUpdate.Location = car.Location;
                carToUpdate.IsBooked = car.IsBooked;

                _carDbContext.Cars.Update(carToUpdate);

                _carDbContext.ChangeTracker.DetectChanges();
                _logger.LogInformation(_carDbContext.ChangeTracker.DebugView.LongView);

                _carDbContext.SaveChanges();
            }
        }
        public IEnumerable<Car> GetAllCars()
        {
            return _carDbContext.Cars.OrderBy(c => c.Id).AsNoTracking().AsEnumerable<Car>();
        }

        public Car GetCarById(ObjectId id)
        {
            return _carDbContext.Cars.FirstOrDefault(c  => c.Id == id);
        }
    }
}