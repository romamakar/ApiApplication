using FluentValidation;
using System;

namespace ApiApplication.Commands.Seat.Validators
{
    public class ConfirmSeatValidator: AbstractValidator<ConfirmSeatCommand>
    {
        public ConfirmSeatValidator()
        {
            RuleFor(x => x.TickedId).NotEmpty();
        }
    }
}
