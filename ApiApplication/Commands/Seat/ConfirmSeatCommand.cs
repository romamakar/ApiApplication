using MediatR;
using System;

namespace ApiApplication.Commands.Seat
{
    public class ConfirmSeatCommand : IRequest
    {
        public Guid TickedId { get; set; }
    }
}
