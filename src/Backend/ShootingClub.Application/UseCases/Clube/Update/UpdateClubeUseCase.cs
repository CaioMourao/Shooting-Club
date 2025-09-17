using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Clube;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Clube.Update
{
    public class UpdateClubeUseCase : IUpdateClubeUseCase
    {
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IClubeUpdateOnlyRepository _repository;
        private readonly IClubeReadOnlyRepository _clubeReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateClubeUseCase(ILoggedUsuario loggedUsuario, IClubeUpdateOnlyRepository repository, IClubeReadOnlyRepository clubeReadRepository, IUnitOfWork unitOfWork)
        {
            _loggedUsuario = loggedUsuario;
            _repository = repository;
            _clubeReadRepository = clubeReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestClubeJson request)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();

            var clube = await _repository.GetById(loggedUsuario.ClubeId);

            await Validate(request, clube);


            clube.AtualizadoEm = DateTime.UtcNow;
            clube.Nome = request.Nome;
            clube.CNPJ = request.CNPJ;
            clube.CertificadoRegistro = request.CertificadoRegistro;
            clube.EnderecoPais = request.EnderecoPais;
            clube.EnderecoEstado = request.EnderecoEstado;
            clube.EnderecoCidade = request.EnderecoCidade;
            clube.EnderecoBairro = request.EnderecoBairro;
            clube.EnderecoRua = request.EnderecoRua;
            clube.EnderecoNumero = request.EnderecoNumero;

            _repository.Update(clube);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestClubeJson request, Domain.Entities.Clube clube)
        {
            var validator = new ClubeValidator();

            var result = validator.Validate(request);

            if (!CnpjUtils.Format(request.CNPJ).Equals(clube.CNPJ))
            {
                var cnpjExist = await _clubeReadRepository.ExistActiveClubeWithCNPJ(CnpjUtils.Format(request.CNPJ));
                if (cnpjExist)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("cnpj", ResourceMessagesException.CNPJ_JA_CADASTRADO));
            }

            if (!request.CertificadoRegistro.Equals(clube.CertificadoRegistro))
            {
                var certificadoRegistroExist = await _clubeReadRepository.ExistActiveClubeWithCertificadoRegistro(request.CertificadoRegistro);
                if (certificadoRegistroExist)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("cnpj", ResourceMessagesException.CNPJ_JA_CADASTRADO));
            }

            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }

    }
}
