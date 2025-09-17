using FluentValidation;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;

namespace ShootingClub.Application.UseCases.Usuario.Update
{
    public class UpdateUsuarioValidator : AbstractValidator<RequestUpdateUsuarioJson>
    {
        public UpdateUsuarioValidator()
        {
            RuleFor(usuario => usuario.Nome)
                .NotEmpty()
                .Must(nome => nome.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length >= 2)
                .WithMessage(ResourceMessagesException.NOME_INVALIDO);

            RuleFor(usuario => usuario.DataNascimento)
                .NotEmpty()
                .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Today))
                .WithMessage(ResourceMessagesException.DATA_NASCIMENTO_INVALIDA);

            RuleFor(usuario => usuario.EnderecoPais)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_PAIS_INVALIDO);

            RuleFor(usuario => usuario.EnderecoEstado)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_ESTADO_INVALIDO);

            RuleFor(usuario => usuario.EnderecoCidade)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_CIDADE_INVALIDO);

            RuleFor(usuario => usuario.EnderecoBairro)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_BAIRRO_INVALIDO);

            RuleFor(usuario => usuario.EnderecoRua)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_RUA_INVALIDA);

            RuleFor(usuario => usuario.EnderecoNumero)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_NUMERO_INVALIDO);

        }
    }
}
