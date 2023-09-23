using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Exceptions;
using ApiApplication.Models;
using ApiApplication.Services;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Commands.Showtime
{
    public class CreateShowtimeCommandHandler : IRequestHandler<CreateShowtimeCommand, ShowtimeVM>
    {
        private readonly IShowtimesRepository showtimesRepository;
        private readonly IMoviesRepository moviesRepository;
        private readonly IProviderApiService providerApiService;

        public CreateShowtimeCommandHandler(IShowtimesRepository showtimesRepository, IMoviesRepository moviesRepository, IProviderApiService providerApiService)
        {
            this.showtimesRepository = showtimesRepository;
            this.moviesRepository = moviesRepository;
            this.providerApiService = providerApiService;
        }
        public async Task<ShowtimeVM> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var movie = await providerApiService.GetMovieAsync(request.MovieId);

            if (movie == null)
            {
                throw new NotFoundException("Not found movie");
            }

            var movieEntity = (await moviesRepository.GetAllAsync(x => x.Title == movie.Title, cancellationToken)).FirstOrDefault();

            if (movieEntity == null)
            {
                movieEntity = new MovieEntity
                {
                    ImdbId = Guid.NewGuid().ToString(),
                    ReleaseDate = new DateTime(Convert.ToInt32(movie.Year), 01, 01),
                    Title = movie.Title,
                    Stars = movie.Rank
                };
            }

            var showTimeEntity = new ShowtimeEntity
            {
                SessionDate = request.SessionDate,
                AuditoriumId = request.AuditoriumId,
                Movie = movieEntity
            };


            await showtimesRepository.CreateShowtime(showTimeEntity, cancellationToken);

            return new ShowtimeVM
            {
                ShowTimeId = showTimeEntity.Id
            };
        }
    }
}
