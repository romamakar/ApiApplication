using ApiApplication.Database.Repositories.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Commands.Seat
{
    public class ConfirmSeatCommandHandler : IRequestHandler<ConfirmSeatCommand, bool>
    {
        private readonly ITicketsRepository ticketsRepository;

        public ConfirmSeatCommandHandler(ITicketsRepository ticketsRepository)
        {
            this.ticketsRepository = ticketsRepository;
        }
        public async Task<bool> Handle(ConfirmSeatCommand request, CancellationToken cancellationToken)
        {
            var ticket = await ticketsRepository.GetAsync(request.TickedId, cancellationToken);
            if (ticket == null || ticket.Paid)
            {
                throw new System.Exception("ticket not found or already paid");
            }
            await ticketsRepository.ConfirmPaymentAsync(ticket, cancellationToken);
            return true;
        }
    }
}
