using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.UseCases.Dashboard.accountants
{
    public interface IGetDashboardUseCase
    {
        public Task<ResponseAccountantsDashboard> Execute();
    }
}
