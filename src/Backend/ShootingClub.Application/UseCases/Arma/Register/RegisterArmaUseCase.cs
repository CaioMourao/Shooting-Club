using AutoMapper;
using ShootingClub.Application.UseCases.Arma.Validators;
using ShootingClub.Communication.Enums;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;
using FluentValidation.Results;
using ShootingClub.Application.Utils;
using ShootingClub.Domain.Repositories.Usuario;

namespace ShootingClub.Application.UseCases.Arma.Register
{
    public class RegisterArmaUseCase : IRegisterArmaUseCase
    {
        private readonly IArmaWriteOnlyRepository _armaWriteOnlyRepository;
        private readonly IArmaReadOnlyRepository _armaReadOnlyRepository;
        private readonly IUsuarioReadOnlyRepository _usuarioRepository;
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterArmaUseCase(
            IArmaWriteOnlyRepository armaWriteOnlyRepository,
            IArmaReadOnlyRepository armaReadOnlyRepository,
            IUsuarioReadOnlyRepository usuarioRepository,
            ILoggedUsuario loggedUsuario,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _armaWriteOnlyRepository = armaWriteOnlyRepository;
            _armaReadOnlyRepository = armaReadOnlyRepository;
            _loggedUsuario = loggedUsuario;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ResponseRegisteredArmaJson> Execute(RequestArmaBaseJson request)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();

            await Validate(request, loggedUsuario.CPF, loggedUsuario.ClubeId);

            var arma = _mapper.Map<ArmaBase>(request);

            if (!string.IsNullOrWhiteSpace(request.Cpf_proprietario))
            {
                var cpfProprietario = CpfUtils.Format(request.Cpf_proprietario);
                var idProprietario = await _usuarioRepository.GetIdUsuarioByCPF(cpfProprietario);
                arma.UsuarioId = idProprietario;
            }else
                arma.ClubeId = loggedUsuario.ClubeId;

            arma.NumeroSerie = arma.NumeroSerie.ToUpperInvariant();

            await _armaWriteOnlyRepository.Add(arma);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredArmaJson>(arma);
        }

        private async Task Validate(RequestArmaBaseJson request, string cpf_loggedUsuario, int clubeId_loggedUsuario)

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

            bool existNumeroSerie = await _armaReadOnlyRepository.ExistActiveArmaWithNumeroSerie(request.NumeroSerie.ToUpperInvariant());
            if (existNumeroSerie)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.NUMERO_SERIE_JA_REGISTRADO));
            }
            if (!string.IsNullOrEmpty(request.Cpf_proprietario) && CpfUtils.ValidCPF(request.Cpf_proprietario))
            {
                var cpf_proprietario = CpfUtils.Format(request.Cpf_proprietario);
                bool existClubeAndCPF = await _usuarioRepository.ExistActiveUsuarioWithClubeAndCPF(clubeId_loggedUsuario, cpf_proprietario);

                if (!existClubeAndCPF)
                    result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.CPF_USUARIO_INEXISTENTE));

                if (cpf_loggedUsuario.Equals(request.Cpf_proprietario))
                    result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.ACAO_INVALIDA_CPF_INFORMADO));
            }

            if (result is not null && !result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}