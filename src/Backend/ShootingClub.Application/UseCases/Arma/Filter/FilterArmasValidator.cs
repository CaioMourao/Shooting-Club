using FluentValidation;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;

namespace ShootingClub.Application.UseCases.Arma.Filter
{
    public class FilterArmasValidator : AbstractValidator<RequestFilterArmaJson>
    {
        public FilterArmasValidator()
        {
            RuleFor(a => a.TipoPosse).IsInEnum().WithMessage(ResourceMessagesException.TIPO_POSSE_ARMA_INVALIDO);
        }
    }
}
