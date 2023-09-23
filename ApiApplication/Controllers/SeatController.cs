using ApiApplication.Commands.Showtime;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Threading;
using ApiApplication.Commands.Seat;

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly IMediator mediator;
        public SeatController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("buy")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuySeat([FromBody] BuySeatCommand request, CancellationToken cancellationToken)
        {
            var ticket = await mediator.Send(request, cancellationToken);
            if (ticket == null)
            {
                return NotFound();
            }
            var location = Url.Action(nameof(GetById), new { id = ticket.Id }) ?? $"/{ticket.Id}";
            return Created(location, ticket);
        }


        [HttpPost("confirm")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmSeat([FromBody] ConfirmSeatCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
            return Ok();
        }


        [HttpPost("reserve")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReserveSeat([FromBody] ReserveSeatCommand request, CancellationToken cancellationToken)
        {
            var ticket = await mediator.Send(request, cancellationToken);
            if (ticket == null)
            {
                return NotFound();
            }
            var location = Url.Action(nameof(GetById), new { id = ticket.Id }) ?? $"/{ticket.Id}";
            return Created(location, ticket);
        }

        [HttpGet("{seatId}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int showTimeId, CancellationToken cancellationToken)
        {
            var cmd = new GetShowtimeByIdRequest { ShowtimeId = showTimeId };
            var showtime = await mediator.Send(cmd, cancellationToken);
            return Ok(showtime);
        }

    }
}
