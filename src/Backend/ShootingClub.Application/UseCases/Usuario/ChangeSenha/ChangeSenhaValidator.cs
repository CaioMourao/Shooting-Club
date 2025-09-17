using FluentValidation;
using ShootingClub.Application.SharedValidators;
using ShootingClub.Communication.Requests;

namespace ShootingClub.Application.UseCases.Usuario.ChangeSenha
{
    public class ChangeSenhaValidator : AbstractValidator<RequestChangeSenhaJson>
    {
        public ChangeSenhaValidator()
        {
            RuleFor(x => x.NovaSenha).SetValidator(new SenhaValidator<RequestChangeSenhaJson>());
        }
    }
}
