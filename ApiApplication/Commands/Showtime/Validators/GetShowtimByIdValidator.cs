using FluentValidation;

namespace ApiApplication.Commands.Showtime.Validators
{
    public class GetShowtimByIdValidator : AbstractValidator<GetShowtimeByIdRequest>
    {
        public GetShowtimByIdValidator()
        {
            RuleFor(x => x.ShowtimeId).GreaterThan(0);          
        }
    }
}
