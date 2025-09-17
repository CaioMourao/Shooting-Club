using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.UseCases.Arma.Filter
{
    public interface IFilterArmaUseCase
    {
        public Task<ResponseArmasJson> Execute(RequestFilterArmaJson request);
    }
}
