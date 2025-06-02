using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Presentation.Data.Entities;
using Presentation.Models;

namespace Presentation.Data.Repositories;

public class BookingRepository(DataContext context) : BaseRepository<BookingEntity>(context), IBookingRepository
{
    public override async Task<RepositoryResult<IEnumerable<BookingEntity>>> GetAllAsync()
    {
        try
        {
            var entities = await _table
                .Include(x => x.BookingOwner)
                .ThenInclude(x => x!.Address)
                .ToListAsync();
            return new RepositoryResult<IEnumerable<BookingEntity>> { Success = true, Result = entities };

        }
        catch (Exception ex)
        {

            return new RepositoryResult<IEnumerable<BookingEntity>>
            {
                Success = false,
                Error = ex.Message,
            };

        }
    }

    public override async Task<RepositoryResult<BookingEntity?>> GetAsync(Expression<Func<BookingEntity, bool>> expression)
    {
        try
        {
            var entity = await _table
                .Include(x => x.BookingOwner)
                .ThenInclude(x => x!.Address)
                .FirstOrDefaultAsync(expression) ?? throw new Exception("Not Found");
            return new RepositoryResult<BookingEntity?> { Success = true, Result = entity };

        }
        catch (Exception ex)
        {

            return new RepositoryResult<BookingEntity?>
            {
                Success = false,
                Error = ex.Message,
            };

        }
    }

    public async Task<BookingResult> DeleteAsync(string id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return new BookingResult 
            { 
                Success = false, 
                Error = "Booking not found" 
            };

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        return new BookingResult { Success = true };
    }

}
