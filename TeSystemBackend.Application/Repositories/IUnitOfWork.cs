namespace TeSystemBackend.Application.Repositories;

public interface IUnitOfWork : IDisposable
{
    IComputerRepository Computers { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IDepartmentRepository Departments { get; }
    ITeamRepository Teams { get; }
    IUserTeamRepository UserTeams { get; }
    ITeamRoleLocationRepository TeamRoleLocations { get; }
    IRoleRepository Roles { get; }
    IPermissionRepository Permissions { get; }
    IPerRoleRepository PerRoles { get; }
    ILocationRepository Locations { get; }
    IReportRepository Reports { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}






