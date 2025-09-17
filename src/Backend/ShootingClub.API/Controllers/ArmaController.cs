using Microsoft.AspNetCore.Mvc;
using ShootingClub.API.Attributes;
using ShootingClub.Application.UseCases.Arma.Delete;
using ShootingClub.Application.UseCases.Arma.Filter;
using ShootingClub.Application.UseCases.Arma.GetById;
using ShootingClub.Application.UseCases.Arma.Register;
using ShootingClub.Application.UseCases.Arma.Update;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
    public class ArmaController : ShootingClubBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredArmaJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedAdminWithClube]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterArmaUseCase useCase,
            [FromBody] RequestArmaBaseJson request)
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(ResponseArmasJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthenticatedUsuario]
        public async Task<IActionResult> Filter(
            [FromServices] IFilterArmaUseCase useCase,
            [FromBody] RequestFilterArmaJson request)
        {
            var response = await useCase.Execute(request);

            if (response.Armas.Any())
                return Ok(response);

            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseArmaBaseJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [AuthenticatedUsuario]
        public async Task<IActionResult> GetById(
            [FromServices] IGetArmaByIdUseCase useCase,
            [FromRoute] int id)
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [AuthenticatedAdmin]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteArmaUseCase useCase,
            [FromRoute] int id)
        {
            await useCase.Execute(id);

            return NoContent(); 
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [AuthenticatedAdminWithClube]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateArmaUseCase useCase,
            [FromRoute] int id,
            [FromBody] RequestArmaBaseJson request
            )
        {
            await useCase.Execute(id, request);

            return NoContent();
        }


    }
}
