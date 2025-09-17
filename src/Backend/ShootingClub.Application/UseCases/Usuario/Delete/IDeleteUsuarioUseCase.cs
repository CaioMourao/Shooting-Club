namespace ShootingClub.Application.UseCases.Usuario.Delete
{
    public interface IDeleteUsuarioUseCase
    {
        public Task Execute(int usuarioId);
    }
}
