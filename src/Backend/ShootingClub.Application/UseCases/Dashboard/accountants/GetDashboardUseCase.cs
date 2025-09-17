
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Services.LoggedUsuario;

namespace ShootingClub.Application.UseCases.Dashboard.accountants
{
    public class GetDashboardUseCase : IGetDashboardUseCase
    {
        private readonly IArmaReadOnlyRepository _armaRepository;
        private readonly IUsuarioReadOnlyRepository _usuarioRepository;
        private readonly ILoggedUsuario _loggedUsuario;

        public GetDashboardUseCase(
            IArmaReadOnlyRepository armaRepository,
            IUsuarioReadOnlyRepository usuarioRepository,
            ILoggedUsuario loggedUsuario)
        {
            _armaRepository = armaRepository;
            _usuarioRepository = usuarioRepository;
            _loggedUsuario = loggedUsuario;
        }

        public async Task<ResponseAccountantsDashboard> Execute()
        {
            var loggedUsuario = await _loggedUsuario.Usuario();

            var armasAtrasadas = await _armaRepository.CountExpiredByClub(loggedUsuario.ClubeId);
            var usuariosNoClube = await _usuarioRepository.CountTotalByClube(loggedUsuario.ClubeId);
            var ultimosUsuariosCadastrados = await _usuarioRepository.CountUsuariosRegisteredInTheLastYear(loggedUsuario);
            var totalArmas = await _armaRepository.CountTotalByClub(loggedUsuario.ClubeId);
            var ultimasArmasCadastradas = await _armaRepository.CountArmasRegisteredInTheLastYear(loggedUsuario.ClubeId);

            return new ResponseAccountantsDashboard
            {
                QuantArmasAtrasadas = armasAtrasadas,
                QuantUsuariosNoClube = usuariosNoClube,
                QuantTotalArmas = totalArmas,
                QuantidadeArmasCadastradasUltimoAno = ultimasArmasCadastradas,
                QuantidadeUsuariosCadastradosUltimoAno = ultimosUsuariosCadastrados
            };
        }
    }
}
