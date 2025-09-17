using Microsoft.AspNetCore.Mvc;
using ShootingClub.API.Attributes;
using ShootingClub.Application.UseCases.Arma.Delete;
using ShootingClub.Application.UseCases.Arma.GetById;
using ShootingClub.Application.UseCases.Usuario.ChangeSenha;
using ShootingClub.Application.UseCases.Usuario.Delete;
using ShootingClub.Application.UseCases.Usuario.Filter;
using ShootingClub.Application.UseCases.Usuario.GetById;
using ShootingClub.Application.UseCases.Usuario.Profile;
using ShootingClub.Application.UseCases.Usuario.Register;
using ShootingClub.Application.UseCases.Usuario.Update;
using ShootingClub.Application.UseCases.Usuario.UpdateByAdmin;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
    public class UsuarioController : ShootingClubBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUsuarioJson), StatusCodes.Status201Created)]
        [AuthenticatedAdminWithClube]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUsuarioUseCase useCase,
            [FromBody] RequestUsuarioJson request)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(ResponseUsuarioProfileJson), StatusCodes.Status200OK)]
        [AuthenticatedUsuario]
        public async Task<IActionResult> GetUsuarioProfile([FromServices] IGetUsuarioProfileUseCase useCase)
        {
            var result = await useCase.Execute();
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUsuario]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateUsuarioUseCase useCase,
            [FromBody] RequestUpdateUsuarioJson request)
        {
            await useCase.Execute(request);
            return NoContent();
        }

        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUsuario]
        public async Task<IActionResult> changeSenha(
            [FromServices] IChangeSenhaUseCase usecase,
            [FromBody] RequestChangeSenhaJson request)
        {
            await usecase.Execute(request);
            return NoContent();
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(ResponseUsuariosJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthenticatedAdminWithClube]
        public async Task<IActionResult> Filter(
            [FromServices] IFilterUsuarioUseCase useCase,
            [FromBody] RequestFilterUsuarioJson request)
        {
            var response = await useCase.Execute(request);

            if (response.Usuarios.Any())
                return Ok(response);

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [AuthenticatedAdminWithClube]
        public async Task<IActionResult> Update(
            [FromServices] IUsuarioUpdateByAdminUseCase useCase,
            [FromRoute] int id,
            [FromBody] RequestUsuarioJson request
            )
        {
            await useCase.Execute(id, request);

            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseUsuarioJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [AuthenticatedAdminWithClube]
        public async Task<IActionResult> GetById(
            [FromServices] IGetUsuarioByIdUseCase useCase,
            [FromRoute] int id)
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [AuthenticatedAdminWithClube]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteUsuarioUseCase useCase,
            [FromRoute] int id)
        {
            await useCase.Execute(id);

            return NoContent();
        }

    }
}
