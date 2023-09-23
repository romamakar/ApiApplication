﻿using FluentValidation;
using System;

namespace ApiApplication.Commands.Seat.Validators
{
    public class ReserveSeatValidator : AbstractValidator<ReserveSeatCommand>
    {
        public ReserveSeatValidator()
        {
            RuleFor(x => x.ShowtimeId).GreaterThan(0);
            RuleFor(x => x.SeatNumbers).NotEmpty();
            RuleForEach(x => x.SeatNumbers).ChildRules(x => {
                x.RuleFor(y => (int)y.SeatNumber).GreaterThan(0);
                x.RuleFor(y => (int)y.Row).GreaterThan(0);
            });
        }
    }
}
