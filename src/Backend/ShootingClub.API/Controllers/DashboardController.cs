using Microsoft.AspNetCore.Mvc;
using ShootingClub.API.Attributes;
using ShootingClub.Application.UseCases.Dashboard.accountants;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
    public class DashboardController : ShootingClubBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseAccountantsDashboard), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [AuthenticatedAdmin]
        public async Task<IActionResult> Get([FromServices] IGetDashboardUseCase useCase)
        {
            var response = await useCase.Execute();
            return Ok(response);
        }

    }
}
