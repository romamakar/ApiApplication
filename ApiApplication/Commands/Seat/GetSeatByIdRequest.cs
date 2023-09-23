using ApiApplication.Models;
using MediatR;
using System;

namespace ApiApplication.Commands.Showtime
{
    public class GetSeatByIdRequest : IRequest<TicketVM>
    {
        public Guid Id { get; set; }
    }
}
