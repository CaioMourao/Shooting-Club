using AutoMapper;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Clube;
using ShootingClub.Domain.Services.LoggedUsuario;

namespace ShootingClub.Application.UseCases.Clube.Profile
{
    public class GetClubeProfileUseCase : IGetClubeProfileUseCase
    {
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IClubeReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        public GetClubeProfileUseCase(ILoggedUsuario loggedUsuario, IMapper mapper, IClubeReadOnlyRepository repository)
        {
            _loggedUsuario = loggedUsuario;
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<ResponseClubeProfileJson> Execute()
        {
            var usuario = await _loggedUsuario.Usuario();
            var clube = await _repository.GetById(usuario.ClubeId);
            return _mapper.Map<ResponseClubeProfileJson>(clube);
        }
    }
}
