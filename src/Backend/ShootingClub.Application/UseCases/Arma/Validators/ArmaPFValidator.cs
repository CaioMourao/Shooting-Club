using FluentValidation;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;

namespace ShootingClub.Application.UseCases.Arma.Validators
{
    public class ArmaPFValidator : AbstractValidator<RequestArmaPFJson>
    {
        public ArmaPFValidator()
        {
            Include(new ArmaBaseValidator());

            RuleFor(arma => arma.NumeroSinarm)
                .NotEmpty().WithMessage(ResourceMessagesException.NUMERO_SINARM_VAZIO);

            RuleFor(arma => arma.NumeroRegistroPF)
                .NotEmpty().WithMessage(ResourceMessagesException.NUMERO_REGISTRO_PF_VAZIO);

            RuleFor(arma => arma.NumeroNotaFiscal)
                .NotEmpty().WithMessage(ResourceMessagesException.NUMERO_NF_VAZIO);

            RuleFor(arma => arma.DataValidadePF)
                .NotEmpty()
                .GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.VALIDADE_PF_INVALIDA);
        }
    }
}
