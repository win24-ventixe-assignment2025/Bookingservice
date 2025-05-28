using Presentation.Data.Entities;
using Presentation.Models;

namespace Presentation.Services
{
    public interface IBookingService
    {
        Task<BookingResult> CreateBookingAsync(CreateBookingRequest request);
        Task<BookingResult<IEnumerable<BookingEntity>>> GetAllBookingsAsync();

    }
}