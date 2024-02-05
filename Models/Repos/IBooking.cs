

using MongoDB.Bson;

namespace SuperCarBookingSystem.Models.Repos
{
    public interface IBooking
    {
        IEnumerable<Booking> GetAllBookings();
        Booking? GetBookingById(ObjectId id);

        void AddBooking(Booking newBooking);

        void EditBooking(Booking updatedBooking);

        void DeleteBooking(Booking booking);
    }
}