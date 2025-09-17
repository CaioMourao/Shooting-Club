namespace ShootingClub.Application.UseCases.Arma.Delete
{
    public interface IDeleteArmaUseCase
    {
        public Task Execute(int armaId);
    }
}
