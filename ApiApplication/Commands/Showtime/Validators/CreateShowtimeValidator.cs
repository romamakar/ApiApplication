using FluentValidation;

namespace ApiApplication.Commands.Showtime.Validators
{
    public class CreateShowtimeValidator:AbstractValidator<CreateShowtimeCommand>
    {
        public CreateShowtimeValidator()
        {
            RuleFor(x => x.MovieId).NotEmpty();
            RuleFor(x => x.AuditoriumId).GreaterThan(0);
            RuleFor(x => x.SessionDate).NotEmpty();           
        }
    }
}
