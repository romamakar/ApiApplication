using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Exceptions;
using ApiApplication.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Commands.Showtime
{
    public class GetSeatByIdRequestHandler : IRequestHandler<GetSeatByIdRequest, TicketVM>
    {
        private readonly ITicketsRepository tikcetsRepository;

        public GetSeatByIdRequestHandler(ITicketsRepository tikcetsRepository)
        {
            this.tikcetsRepository = tikcetsRepository;
        }
        public async Task<TicketVM> Handle(GetSeatByIdRequest request, CancellationToken cancellationToken)
        {
            var ticket = await tikcetsRepository.GetAsync(request.Id, cancellationToken);
            if (ticket == null)
            {
                throw new NotFoundException($"Ticket with id={request.Id} not found");
            }
            return new TicketVM
            {
                Id = ticket.Id,
                NumberOfSeats = ticket.Seats.Select(seat => new SeatVM { Row = seat.Row, SeatNumber = seat.SeatNumber }),
                AuditoriumId =  ticket.Showtime.AuditoriumId,
                MovieTitle = ticket.Showtime.Movie.Title
            };
        }
    }
}
