using ApiApplication.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using System.Linq.Expressions;
using ApiApplication.Database.Repositories.Abstractions;

namespace ApiApplication.Database.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly CinemaContext _context;

        public MoviesRepository(CinemaContext context)
        {
            _context = context;
        }
       
        public async Task<IEnumerable<MovieEntity>> GetAllAsync(Expression<Func<MovieEntity, bool>> filter, CancellationToken cancel)
        {
            if (filter == null)
            {
                return await _context.Movies                
                .ToListAsync(cancel);
            }
            return await _context.Movies                
                .Where(filter)
                .ToListAsync(cancel);
        }
    }
}
