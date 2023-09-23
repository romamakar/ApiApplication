using System;
using System.Collections.Generic;

namespace ApiApplication.Models
{
    public class TicketVM
    {
        public Guid Id { get; set; }
        public IEnumerable<SeatVM> NumberOfSeats { get; set; }
        public int AuditoriumId { get; set; }
        public string MovieTitle { get; set; }
    }
}
