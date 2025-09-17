using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.UseCases.Clube.Profile
{
    public interface IGetClubeProfileUseCase
    {
        public Task<ResponseClubeProfileJson> Execute();
    }
}
