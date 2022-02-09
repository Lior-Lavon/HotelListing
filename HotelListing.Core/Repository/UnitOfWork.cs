using HotelListing.Data;
using System;
using System.Threading.Tasks;

namespace HotelListing.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        private IGenericRepository<Country> _countries;

        private IGenericRepository<Hotel> _hotels;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IGenericRepository<Country> Countries => _countries ??= new GenericRepository<Country>(_context);
        // if _countries == null return a new Country repository instance

        public IGenericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_context);
        // if _hotels == null return a new Hotel repository instance

        public void Dispose()
        {
            _context.Dispose(); // free the memory when i am done with context
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
