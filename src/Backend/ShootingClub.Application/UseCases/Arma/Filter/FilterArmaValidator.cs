using FluentValidation;
using ShootingClub.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingClub.Application.UseCases.Arma.Filter
{
    public class FilterArmaValidator : AbstractValidator<RequestFilterArmaJson>
    {
        public FilterArmaValidator()
        {
            
        }
    }
}
