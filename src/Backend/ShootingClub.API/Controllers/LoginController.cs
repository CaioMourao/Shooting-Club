using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ShootingClub.Application.UseCases.Login.DoLogin;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
    public class LoginController : ShootingClubBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseLoggedInUsuarioJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromServices] IDoLoginUseCase usecase, [FromBody] RequestLoginJson request)
        {
            var response = await usecase.Execute(request);

            return Ok(response);
        }
       
    }
}
