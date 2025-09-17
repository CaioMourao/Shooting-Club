using ShootingClub.Communication.Requests;

namespace ShootingClub.Application.UseCases.Arma.Update
{
    public interface IUpdateArmaUseCase
    {
        Task Execute(int ArmaId, RequestArmaBaseJson request);

    }
}
