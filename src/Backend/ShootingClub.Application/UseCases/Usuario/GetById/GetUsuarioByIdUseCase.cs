using AutoMapper;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Usuario.GetById
{
    public class GetUsuarioByIdUseCase : IGetUsuarioByIdUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUsuarioReadOnlyRepository _repository;

        public GetUsuarioByIdUseCase(IMapper mapper, ILoggedUsuario loggedUsuario, IUsuarioReadOnlyRepository repository)
        {
            _mapper = mapper;
            _loggedUsuario = loggedUsuario;
            _repository = repository;
        }
        public async Task<ResponseUsuarioJson> Execute(int usuarioId)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();
            var usuario = await _repository.GetById(usuarioId);

            if (usuario is null || usuario.ClubeId != loggedUsuario.ClubeId)
            {
                throw new NotFoundException(ResourceMessagesException.USUARIO_NAO_ENCONTRADO);
            }
            return _mapper.Map<ResponseUsuarioJson>(usuario);
        }
    }
}
