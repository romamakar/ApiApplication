using ApiApplication.Models;
using MediatR;

namespace ApiApplication.Commands.Showtime
{
    public class GetShowtimeByIdRequest : IRequest<ShowtimeVM>
    {
        public int ShowtimeId { get; set; }
    }
}
