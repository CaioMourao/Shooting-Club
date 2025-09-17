using FluentValidation;
using FluentValidation.Validators;
using ShootingClub.Exceptions;
using System.Text.RegularExpressions;

namespace ShootingClub.Application.SharedValidators
{
    public class SenhaValidator<T> : PropertyValidator<T, string>
    {

        public override bool IsValid(ValidationContext<T> context, string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.SENHA_VAZIA);
                return false;
            }
            if (!Regex.IsMatch(senha, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"))
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.SENHA_INVALIDA);
                return false;
            }

            return true;
        }

        public override string Name => "SenhaValidator";

        protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";

    }
}
