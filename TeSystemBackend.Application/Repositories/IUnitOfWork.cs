namespace TeSystemBackend.Application.Repositories;

public interface IUnitOfWork : IDisposable
{
    IComputerRepository Computers { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IDepartmentRepository Departments { get; }
    ITeamRepository Teams { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}






