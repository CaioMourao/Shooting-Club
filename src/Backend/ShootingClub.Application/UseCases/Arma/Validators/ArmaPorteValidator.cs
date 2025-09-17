using FluentValidation;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;

namespace ShootingClub.Application.UseCases.Arma.Validators
{
    public class ArmaPorteValidator : AbstractValidator<RequestArmaPorteJson>
    {
        public ArmaPorteValidator()
        {
            Include(new ArmaBaseValidator());

            RuleFor(arma => arma.NumeroCertificado)
                .NotEmpty().WithMessage(ResourceMessagesException.NUMERO_CERTIFICADO_VAZIO);

            RuleFor(arma => arma.ValidadeCertificacao)
                .NotEmpty()
                .GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.VALIDADE_CERTIFICADO_INVALIDA);

            RuleFor(arma => arma.DataCertificacao)
                .NotEmpty()
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.DATA_CERTIFICADO_INVALIDA);


        }
    }
}
