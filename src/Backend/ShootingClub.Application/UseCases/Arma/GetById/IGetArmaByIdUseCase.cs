using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.UseCases.Arma.GetById
{
    public interface IGetArmaByIdUseCase
    {
        public Task<ResponseArmaBaseJson> Execute(int armaId);
    }
}
