using ApiApplication.Database.Entities;
using ApiApplication.Models;
using MediatR;
using System;

namespace ApiApplication.Commands.Showtime
{
    public class CreateShowtimeCommand:IRequest<ShowtimeVM>
    {
        public string MovieId { get; set; }
        public DateTime SessionDate { get; set; }
        public int AuditoriumId { get; set; }
    }
}
