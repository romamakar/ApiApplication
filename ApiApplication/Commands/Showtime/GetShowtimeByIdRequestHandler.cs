using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Exceptions;
using ApiApplication.Models;
using ApiApplication.Services;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Commands.Showtime
{
    public class GetShowtimeByIdRequestHandler : IRequestHandler<GetShowtimeByIdRequest, ShowtimeVM>
    {
        private readonly IShowtimesRepository showtimesRepository;

        public GetShowtimeByIdRequestHandler(IShowtimesRepository showtimesRepository)
        {
            this.showtimesRepository = showtimesRepository;
        }
        public async Task<ShowtimeVM> Handle(GetShowtimeByIdRequest request, CancellationToken cancellationToken)
        {
            var showtimeEntity = (await showtimesRepository.GetAllAsync(x => x.Id == request.ShowtimeId, cancellationToken)).FirstOrDefault();
            if (showtimeEntity ==null)
            {
                throw new NotFoundException($"ShowtimeEntity with id={request.ShowtimeId} not found");
            }
            return new ShowtimeVM { ShowTimeId = showtimeEntity.Id };
        }
    }
}
