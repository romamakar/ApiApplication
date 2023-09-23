using ApiApplication.Models;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ApiApplication.Commands.Seat
{
    public class ReserveSeatCommand : IRequest<TicketVM>
    {
        public int ShowtimeId { get; set; }
  
        public IEnumerable<SeatVM> SeatNumbers  { get; set; }
    }
}
