using FluentValidation;
using System;

namespace ApiApplication.Commands.Seat.Validators
{
    public class BuySeatValidator: AbstractValidator<BuySeatCommand>
    {
        public BuySeatValidator()
        {
            RuleFor(x => x.ShowtimeId).GreaterThan(1);
            RuleForEach(x => x.SeatNumbers).ChildRules(x => { 
                x.RuleFor(y => (int)y.SeatNumber).GreaterThan(0);
                x.RuleFor(y => (int)y.Row).GreaterThan(0);
            });
        }
    }
}
