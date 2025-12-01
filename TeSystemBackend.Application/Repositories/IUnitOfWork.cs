namespace TeSystemBackend.Application.Repositories;

public interface IUnitOfWork : IDisposable
{
    IComputerRepository Computers { get; }
    IRefreshTokenRepository RefreshTokens { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}






