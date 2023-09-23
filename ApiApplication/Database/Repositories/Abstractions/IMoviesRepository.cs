using ApiApplication.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface IMoviesRepository
    {
        Task<IEnumerable<MovieEntity>> GetAllAsync(Expression<Func<MovieEntity, bool>> filter, CancellationToken cancel);
    }
}