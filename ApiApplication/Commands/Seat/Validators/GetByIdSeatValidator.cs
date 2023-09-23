using ApiApplication.Commands.Showtime;
using FluentValidation;
using System;

namespace ApiApplication.Commands.Seat.Validators
{
    public class GetByIdSeatValidator : AbstractValidator<GetSeatByIdRequest>
    {
        public GetByIdSeatValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
