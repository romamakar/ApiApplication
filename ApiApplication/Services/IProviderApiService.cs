using ApiApplication.Services.DTO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public interface IProviderApiService
    {
        Task<MovieDTO> GetMovieAsync(string id);
        Task<IEnumerable<MovieDTO>> GetAllMoviesAsync();
    }
}
