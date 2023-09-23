using MediatR;
using System;

namespace ApiApplication.Commands.Seat
{
    public class ConfirmSeatCommand : IRequest<bool>
    {
        public Guid TickedId { get; set; }
    }
}
