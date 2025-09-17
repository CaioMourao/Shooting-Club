using ShootingClub.Communication.Requests;

namespace ShootingClub.Application.UseCases.Clube.Update
{
    public interface IUpdateClubeUseCase
    {
        public Task Execute(RequestClubeJson request);
    }
}
