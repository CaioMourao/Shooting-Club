using AutoMapper;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Services.LoggedUsuario;

namespace ShootingClub.Application.UseCases.Usuario.Filter
{
    public class FilterUsuarioUseCase : IFilterUsuarioUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUsuarioReadOnlyRepository _usuarioReadOnlyRepository;
        public FilterUsuarioUseCase(IMapper mapper, ILoggedUsuario loggedUsuario, IUsuarioReadOnlyRepository usuarioReadOnlyRepository)
        {
            _mapper = mapper;
            _loggedUsuario = loggedUsuario;
            _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
        }
        public async Task<ResponseUsuariosJson> Execute(RequestFilterUsuarioJson request)
        {
            var loggedUser = await _loggedUsuario.Usuario();

            var filters = new Domain.Dtos.FilterUsuariosDto
            {
                Nome = request.Nome,
                Email = request.Email,
                CPF = request.CPF,
                ProximoExpiracao = request.ProximoExpiracao
            };
            var usuarios = await _usuarioReadOnlyRepository.Filter(loggedUser, filters);

            return new ResponseUsuariosJson { Usuarios = _mapper.Map<List<ResponseUsuarioShortJson>>(usuarios) };
        }
    }
}
