using Microsoft.AspNetCore.Mvc;
using ShootingClub.Application.UseCases.Token.RefreshToken;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
    public class TokenController : ShootingClubBaseController
    {
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken(
            [FromServices] IUserRefreshTokenUseCase usecase,
            [FromBody] RequestNewTokenJson request)
        {
            var response = await usecase.Execute(request);
            return Ok(response);
        }
    }
}
