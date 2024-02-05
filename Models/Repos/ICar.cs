using MongoDB.Bson;

namespace SuperCarBookingSystem.Models.Repos
{
    public interface ICar
    {
        IEnumerable<Car> GetAllCars();
        Car? GetCarById(ObjectId id);

        void AddCar(Car car);

        void EditCar(Car car);

        void DeleteCar(Car car);
    }
}