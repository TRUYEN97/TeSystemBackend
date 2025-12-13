using Microsoft.EntityFrameworkCore.Storage;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Infrastructure.Repositories;

namespace TeSystemBackend.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IComputerRepository Computers => new ComputerRepository(_context);
    public IRefreshTokenRepository RefreshTokens => new RefreshTokenRepository(_context);
    public IDepartmentRepository Departments => new DepartmentRepository(_context);
    public ITeamRepository Teams => new TeamRepository(_context);
    public IUserTeamRepository UserTeams => new UserTeamRepository(_context);
    public ITeamRoleLocationRepository TeamRoleLocations => new TeamRoleLocationRepository(_context);
    public IRoleRepository Roles => new RoleRepository(_context);
    public IPermissionRepository Permissions => new PermissionRepository(_context);
    public IPerRoleRepository PerRoles => new PerRoleRepository(_context);
    public ILocationRepository Locations => new LocationRepository(_context);
    public IReportRepository Reports => new ReportRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

