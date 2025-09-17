using AutoMapper;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Arma.GetById
{
    internal class GetArmaByIdUseCase : IGetArmaByIdUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IArmaReadOnlyRepository _repository;

        public GetArmaByIdUseCase(IMapper mapper, ILoggedUsuario loggedUsuario, IArmaReadOnlyRepository repository)
        {
            _mapper = mapper;
            _loggedUsuario = loggedUsuario;
            _repository = repository;
        }
        public async Task<ResponseArmaBaseJson> Execute(int armaId)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();
            var arma = await _repository.GetById(loggedUsuario, armaId);
            if (arma is null)
            {
                throw new NotFoundException(ResourceMessagesException.ARMA_NAO_ENCONTRADA);
            }

            return _mapper.Map<ResponseArmaBaseJson>(arma);
        }
    }
}
