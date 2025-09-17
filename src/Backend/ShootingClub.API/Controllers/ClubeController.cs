using Microsoft.AspNetCore.Mvc;
using ShootingClub.API.Attributes;
using ShootingClub.Application.UseCases.Clube.Profile;
using ShootingClub.Application.UseCases.Clube.Register;
using ShootingClub.Application.UseCases.Clube.Update;
using ShootingClub.Application.UseCases.Usuario.Profile;
using ShootingClub.Application.UseCases.Usuario.Update;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
    public class ClubeController : ShootingClubBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredClubeJson), StatusCodes.Status201Created)]
        [AuthenticatedAdmin]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterClubeUseCase useCase,
            [FromBody] RequestClubeJson request)
        {
            var result = await useCase.Execute(request);

            return Created(string.Empty, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseClubeProfileJson), StatusCodes.Status200OK)]
        [AuthenticatedUsuario]
        public async Task<IActionResult> GetClubeProfile([FromServices] IGetClubeProfileUseCase useCase)
        {
            var result = await useCase.Execute();
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedAdminWithClube]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateClubeUseCase useCase,
            [FromBody] RequestClubeJson request)
        {
            await useCase.Execute(request);
            return NoContent();
        }
    }
}
