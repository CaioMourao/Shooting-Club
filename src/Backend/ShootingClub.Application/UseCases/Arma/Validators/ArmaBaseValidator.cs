using FluentValidation;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;

namespace ShootingClub.Application.UseCases.Arma.Validators
{
    public class ArmaBaseValidator : AbstractValidator<RequestArmaBaseJson>
    {
        public ArmaBaseValidator()
        {
            When(usuario => !string.IsNullOrWhiteSpace(usuario.Cpf_proprietario), () =>
            {
                RuleFor(Usuario => Usuario.Cpf_proprietario).Must(CpfUtils.ValidCPF!).WithMessage(ResourceMessagesException.CPF_INVALIDO);
            });
            RuleFor(arma => arma.Tipo).NotEmpty().WithMessage(ResourceMessagesException.TIPO_ARMA_VAZIO);
            RuleFor(arma => arma.Marca).NotEmpty().WithMessage(ResourceMessagesException.MARCA_ARMA_VAZIO);
            RuleFor(arma => arma.NumeroSerie).NotEmpty().WithMessage(ResourceMessagesException.NUMERO_SERIE_ARMA_VAZIO);
            RuleFor(arma => arma.TipoPosse).IsInEnum().WithMessage(ResourceMessagesException.TIPO_POSSE_ARMA_INVALIDO);

            RuleFor(arma => arma.Calibre)
                .NotEmpty()
                .Must(calibre => calibre.Any(char.IsDigit))
                    .WithMessage(ResourceMessagesException.CALIBRE_ARMA_INVALIDO)
                    .When(arma => !string.IsNullOrWhiteSpace(arma.Calibre));
        }
    }
}
