using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.UseCases.Clube.Register
{
    public interface IRegisterClubeUseCase
    {
        public Task<ResponseRegisteredClubeJson> Execute(RequestClubeJson request);
    }
}
