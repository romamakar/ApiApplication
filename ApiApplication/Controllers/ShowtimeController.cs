using ApiApplication.Commands.Showtime;
using ApiApplication.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimeController : ControllerBase
    {
        private readonly IMediator mediator;

        public ShowtimeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("create")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ShowtimeVM), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateShowtime([FromBody] CreateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var showtime = await mediator.Send(request, cancellationToken);
            if (showtime == null)
            {
                return NotFound();
            }
            var location = Url.Action(nameof(GetById), new { id = showtime.ShowTimeId }) ?? $"/{showtime.ShowTimeId}";
            return Created(location, showtime);
        }

        [HttpGet("{showTimeId}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ShowtimeVM), StatusCodes.Status200OK)]
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
