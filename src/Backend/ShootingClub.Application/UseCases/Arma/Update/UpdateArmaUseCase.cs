using AutoMapper;
using FluentValidation.Results;
using ShootingClub.Application.UseCases.Arma.Validators;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Enums;
using ShootingClub.Communication.Requests;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Arma.Update
{
    public class UpdateArmaUseCase : IUpdateArmaUseCase
    {
        private readonly IArmaUpdateOnlyRepository _updateOnlyRepository;
        private readonly IUsuarioReadOnlyRepository _usuarioRepository;
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateArmaUseCase(
            IArmaUpdateOnlyRepository updateOnlyRepository,
            IArmaReadOnlyRepository armaReadOnlyRepository,
            IUsuarioReadOnlyRepository usuarioRepository,
            ILoggedUsuario loggedUsuario,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _updateOnlyRepository = updateOnlyRepository;
            _loggedUsuario = loggedUsuario;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
        }

        public async Task Execute(int armaId, RequestArmaBaseJson request)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();

            await Validate(request, loggedUsuario.CPF, loggedUsuario.ClubeId, armaId);

            var arma = await _updateOnlyRepository.GetById(loggedUsuario, armaId);

            if (arma is null)
                throw new NotFoundException(ResourceMessagesException.ARMA_NAO_ENCONTRADA);

            if (arma.TipoPosse != (Domain.Enums.TipoPosseArma)request.TipoPosse)
                throw new ErrorOnValidationException([ResourceMessagesException.TIPO_POSSE_NAO_PODE_SER_ALTERADO]);

            _mapper.Map(request, arma);

            if (!string.IsNullOrWhiteSpace(request.Cpf_proprietario))
            {
                var cpfProprietario = CpfUtils.Format(request.Cpf_proprietario);
                var idProprietario = await _usuarioRepository.GetIdUsuarioByCPF(cpfProprietario);
                arma.UsuarioId = idProprietario;
                arma.ClubeId = 0;
            }
            else
            {
                arma.UsuarioId = 0;
                arma.ClubeId = loggedUsuario.ClubeId;
            }

            arma.AtualizadoEm = DateTime.UtcNow;
            arma.NumeroSerie = arma.NumeroSerie.ToUpperInvariant();

            _updateOnlyRepository.Update(arma);
            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestArmaBaseJson request, string cpf_loggedUsuario, int clubeId_loggedUsuario, int armaId)
        {
            ValidationResult? result = null;
            switch (request.TipoPosse)
            {
                case TipoPosseArma.Exercito:
                    var validatorExercito = new ArmaExercitoValidator();
                    result = await validatorExercito.ValidateAsync((RequestArmaExercitoJson)request);
                    break;

                case TipoPosseArma.PoliciaFederal:
                    var validatorPF = new ArmaPFValidator();
                    result = await validatorPF.ValidateAsync((RequestArmaPFJson)request);
                    break;

                case TipoPosseArma.PortePessoal:
                    var validatorPorte = new ArmaPorteValidator();
                    result = await validatorPorte.ValidateAsync((RequestArmaPorteJson)request);
                    break;

                default:
                    throw new ErrorOnValidationException([ResourceMessagesException.TIPO_POSSE_ARMA_INVALIDO]);
            }

            if (await _updateOnlyRepository.ExistActiveArmaWithNumeroSerieAndIgnoreId(request.NumeroSerie.ToUpperInvariant(), armaId))
            {
                result.Errors.Add(new ValidationFailure(nameof(request.NumeroSerie), ResourceMessagesException.NUMERO_SERIE_JA_REGISTRADO));
            }

            if (!string.IsNullOrWhiteSpace(request.Cpf_proprietario) && CpfUtils.ValidCPF(request.Cpf_proprietario))
            {
                var cpfProprietario = CpfUtils.Format(request.Cpf_proprietario);

                if (cpf_loggedUsuario.Equals(cpfProprietario))
                    result.Errors.Add(new ValidationFailure(nameof(request.Cpf_proprietario), ResourceMessagesException.ACAO_INVALIDA_CPF_INFORMADO));

                if (!await _usuarioRepository.ExistActiveUsuarioWithClubeAndCPF(clubeId_loggedUsuario, cpfProprietario))
                    result.Errors.Add(new ValidationFailure(nameof(request.Cpf_proprietario), ResourceMessagesException.CPF_USUARIO_INEXISTENTE));
            }

            if (result is not null && !result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}