using ApiApplication.Models;
using MediatR;
using System.Collections.Generic;

namespace ApiApplication.Commands.Seat
{
    public class BuySeatCommand : IRequest<TicketVM>
    {
        public int ShowtimeId { get; set; }
  
        public IEnumerable<SeatVM> SeatNumbers  { get; set; }
    }
}
