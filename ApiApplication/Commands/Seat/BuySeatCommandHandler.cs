using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Exceptions;
using ApiApplication.Models;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Commands.Seat
{
    public class BuySeatCommandHandler : IRequestHandler<BuySeatCommand, TicketVM>
    {
        private readonly ITicketsRepository ticketsRepository;
        private readonly IShowtimesRepository showtimesRepository;
        private readonly IAuditoriumsRepository auditoriumsRepository;

        public BuySeatCommandHandler(ITicketsRepository ticketsRepository, IShowtimesRepository showtimesRepository, IAuditoriumsRepository auditoriumsRepository)
        {
            this.ticketsRepository = ticketsRepository;
            this.showtimesRepository = showtimesRepository;
            this.auditoriumsRepository = auditoriumsRepository;
        }
        public async Task<TicketVM> Handle(BuySeatCommand request, CancellationToken cancellationToken)
        {

            var showTime = (await showtimesRepository.GetAllAsync(x => x.Id == request.ShowtimeId, cancellationToken)).FirstOrDefault();
            if (showTime == null)
            {
                throw new NotFoundException("Showtime is not exists");
            }
            var allTicketOfShowtime = await ticketsRepository.GetEnrichedAsync(request.ShowtimeId, cancellationToken);

            var reservedTickets = allTicketOfShowtime.Where(ticket =>
              ticket.Seats.Any(seat => request.SeatNumbers.Any(seatNr =>
              seatNr.Row == seat.Row && showTime.AuditoriumId == seat.AuditoriumId && seatNr.SeatNumber == seat.SeatNumber))).ToList();

            var isPaid = reservedTickets.Any(t => t.Paid);

            if (isPaid)
            {
                throw new System.Exception("Already paid tickets");
            }

            var notExpired = reservedTickets.Any(t => t.CreatedTime.AddMinutes(10) > DateTime.Now);
            if (notExpired)
            {
                throw new System.Exception("Tickets are already reserved");
            }


            var auditorium = await auditoriumsRepository.GetAsync(showTime.AuditoriumId, cancellationToken);

            var seats = auditorium.Seats.Where(seat => request.SeatNumbers.Any(seatNr => seatNr.Row == seat.Row && seatNr.SeatNumber == seat.SeatNumber));

            if (seats.Count() != request.SeatNumbers.Count())
            {
                throw new Exception("Seat numbers are not present");
            }

            var ticket = await ticketsRepository.CreateAsync(showTime, seats, cancellationToken);
            await ticketsRepository.ConfirmPaymentAsync(ticket, cancellationToken);
            return new TicketVM
            {
                Id = ticket.Id,
                NumberOfSeats = ticket.Seats.Select(seat => new SeatVM { Row = seat.Row, SeatNumber = seat.SeatNumber }),
                AuditoriumId = auditorium.Id,
                MovieTitle = showTime.Movie.Title
            };

        }
    }
}
