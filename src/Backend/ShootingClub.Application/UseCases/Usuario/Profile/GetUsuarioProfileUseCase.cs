using AutoMapper;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Services.LoggedUsuario;

namespace ShootingClub.Application.UseCases.Usuario.Profile
{
    public class GetUsuarioProfileUseCase : IGetUsuarioProfileUseCase
    {
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IMapper _mapper;
        public GetUsuarioProfileUseCase(ILoggedUsuario loggedUsuario, IMapper mapper)
        {
            _loggedUsuario = loggedUsuario;
            _mapper = mapper;
        }
        public async Task<ResponseUsuarioProfileJson> Execute()
        {
            var usuario = await _loggedUsuario.Usuario();
            return _mapper.Map<ResponseUsuarioProfileJson>(usuario);
        }
    }
}
