using FluentValidation;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;

namespace ShootingClub.Application.UseCases.Arma.Validators
{
    public class ArmaExercitoValidator : AbstractValidator<RequestArmaExercitoJson>
    {
        public ArmaExercitoValidator()
        {
            Include(new ArmaBaseValidator());

            RuleFor(arma => arma.NumeroSigma)
                .NotEmpty()
                .Matches("^[0-9]+$").WithMessage(ResourceMessagesException.NUMERO_SIGMA_INVALIDO);

            RuleFor(arma => arma.LocalRegistro)
                .NotEmpty().WithMessage(ResourceMessagesException.LOCAL_REGISTRO_VAZIO);

            RuleFor(arma => arma.DataExpedicaoCRAF)
                .NotEmpty()
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.DATA_EXPEDICAO_CRAF_INVALIDA);

            RuleFor(arma => arma.ValidadeCRAF)
                .NotEmpty()
                .GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.VALIDADE_CRAF_INVALIDA);

            When(arma => !string.IsNullOrWhiteSpace(arma.NumeroGTE), () =>
            {
                RuleFor(arma => arma.ValidadeGTE)
                    .NotEmpty()
                    .GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.VALIDADE_GTE_INVALIDA);
            });
        }
    }
}
