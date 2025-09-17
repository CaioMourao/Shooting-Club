using FluentValidation;
using ShootingClub.Application.SharedValidators;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;

namespace ShootingClub.Application.UseCases.Usuario
{
    public class UsuarioValidator : AbstractValidator<RequestUsuarioJson>
    {
        private const string TextOnlyRegex = @"^[a-zA-Z\u00C0-\u017F\s'-]+$";
        private const string TextAndNumbersRegex = @"^[a-zA-Z0-9\u00C0-\u017F\s'-]+$";
        private const string PositiveIntegerRegex = @"^[1-9][0-9]*$";


        public UsuarioValidator()
        {
            RuleFor(usuario => usuario.Nome)
                .NotEmpty()
                .Must(nome => nome.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length >= 2)
                    .WithMessage(ResourceMessagesException.NOME_INVALIDO)
                .Matches(TextOnlyRegex)
                    .WithMessage(ResourceMessagesException.NOME_COM_CARACTERES_ESPECIAIS);

            RuleFor(usuario => usuario.Email).NotEmpty()
                .Must(EmailUtils.IsValidEmail)
                .WithMessage(ResourceMessagesException.EMAIL_INVALIDO);

            RuleFor(usuario => usuario.Senha).SetValidator(new SenhaValidator<RequestUsuarioJson>());

            RuleFor(Usuario => Usuario.CPF)
                .NotEmpty()
                .Must(CpfUtils.ValidCPF)
                .WithMessage(ResourceMessagesException.CPF_INVALIDO);

            RuleFor(usuario => usuario.DataNascimento)
                .NotEmpty()
                .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Today))
                .WithMessage(ResourceMessagesException.DATA_NASCIMENTO_INVALIDA);

            RuleFor(usuario => usuario.EnderecoPais)
                .NotEmpty()
                    .WithMessage(ResourceMessagesException.ENDERECO_PAIS_INVALIDO)
                .Matches(TextAndNumbersRegex)
                    .WithMessage(ResourceMessagesException.ENDERECO_COM_CARACTERES_ESPECIAIS);

            RuleFor(usuario => usuario.EnderecoEstado)
                .NotEmpty()
                    .WithMessage(ResourceMessagesException.ENDERECO_ESTADO_INVALIDO)
                .Matches(TextAndNumbersRegex)
                    .WithMessage(ResourceMessagesException.ENDERECO_COM_CARACTERES_ESPECIAIS);

            RuleFor(usuario => usuario.EnderecoCidade)
                .NotEmpty()
                    .WithMessage(ResourceMessagesException.ENDERECO_CIDADE_INVALIDO)
                .Matches(TextAndNumbersRegex)
                    .WithMessage(ResourceMessagesException.ENDERECO_COM_CARACTERES_ESPECIAIS);

            RuleFor(usuario => usuario.EnderecoBairro)
                .NotEmpty()
                    .WithMessage(ResourceMessagesException.ENDERECO_BAIRRO_INVALIDO)
                .Matches(TextAndNumbersRegex)
                    .WithMessage(ResourceMessagesException.ENDERECO_COM_CARACTERES_ESPECIAIS);

            RuleFor(usuario => usuario.EnderecoRua)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_RUA_INVALIDA);

            RuleFor(usuario => usuario.EnderecoNumero)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_NUMERO_INVALIDO);

            RuleFor(usuario => usuario.NumeroFiliacao)
                .NotEmpty()
                .Matches(PositiveIntegerRegex)
                .WithMessage(ResourceMessagesException.NUMERO_FILIACAO_INVALIDO);

            RuleFor(usuario => usuario.DataFiliacao)
                .NotEmpty()
                .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Today))
                .WithMessage(ResourceMessagesException.DATA_FILIACAO_INVALIDA);

            RuleFor(usuario => usuario.DataRenovacaoFiliacao)
                .NotEmpty()
                .Must((usuario, dataRenovacao) => dataRenovacao >= usuario.DataFiliacao)
                .WithMessage(ResourceMessagesException.DATA_RENOVACAO_FILIACAO_INVALIDA);

            When(usuario => !string.IsNullOrWhiteSpace(usuario.CR), () =>
            {
                RuleFor(usuario => usuario.CR)
                    .Matches(PositiveIntegerRegex)
                    .WithMessage(ResourceMessagesException.CR_INVALIDO);

                RuleFor(usuario => usuario.DataVencimentoCR)
                    .NotEmpty().WithMessage(ResourceMessagesException.DATA_VENCIMENTO_CR_REQUERIDA)
                    .GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.DATA_VENCIMENTO_CR_INVALIDA);

                RuleFor(usuario => usuario.SFPCVinculacao)
                    .NotEmpty().WithMessage(ResourceMessagesException.SFPC_VINCULACAO_REQUERIDO)
                    .Matches(TextAndNumbersRegex)
                    .WithMessage(ResourceMessagesException.SFPC_VINCULACAO_INVALIDA);
            })
            .Otherwise(() =>
            {
                RuleFor(usuario => usuario.DataVencimentoCR)
                    .Empty()
                    .WithMessage(ResourceMessagesException.DATA_VENCIMENTO_CR_SEM_CR);

                RuleFor(usuario => usuario.SFPCVinculacao)
                    .Empty()
                    .WithMessage(ResourceMessagesException.SFPC_VINCULACAO_SEM_CR);
            });
        }
    }
}
