using HotelListing.Data;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // Initiate in constractor with Depedency Ingection
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _db;

        // Depedency Ingection
        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _db = _context.Set<T>(); // get a context to the datataype we are dealing with (Hotel or Country)
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);
            _db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _db; // _db is a context t Country or Hotel based on what passed in
            
            //add additional properties 
            if(includes != null)
            {
                foreach(var property in includes)
                {
                    query = query.Include(property);
                }
            }

            // return a value from the db and detach any tracking , just make a copy from db and store in memory
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = _db; // all records of _db context t Country or Hotel based on what passed in
            
            // fillter the query 
            if(expression != null) 
            {
                // if expression is != null apply fillter expression
                query = query.Where(expression);
            }

            //add additional properties like depencencies
            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }

            // check for orderBy
            if(orderBy != null)
            {
                query = orderBy(query);
            }

            // return a value from the db and detach any tracking , just make a copy from db and store in memory
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            // pay attention to the entity , it might be only in memory, might be different then the db value
            _db.Attach(entity);
            // Update the context with the modified value
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
