using Microsoft.EntityFrameworkCore.Storage;
using TeSystemBackend.Application.Repositories;
using TeSystemBackend.Infrastructure.Repositories;

namespace TeSystemBackend.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private ISoftwareRepository? _softwares;
    private ISoftwareVersionRepository? _softwareVersions;
    private ISoftwareFileRepository? _softwareFiles;
    private IComputerRepository? _computers;
    private IComputerSoftwareRepository? _computerSoftwares;
    private ITeamRepository? _teams;
    private IResourceTypeRepository? _resourceTypes;
    private IAclRepository? _aclEntries;
    private IInstallationHistoryRepository? _installationHistories;
    private IChangeLogRepository? _changeLogs;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ISoftwareRepository Softwares =>
        _softwares ??= new SoftwareRepository(_context);

    public ISoftwareVersionRepository SoftwareVersions =>
        _softwareVersions ??= new SoftwareVersionRepository(_context);

    public ISoftwareFileRepository SoftwareFiles =>
        _softwareFiles ??= new SoftwareFileRepository(_context);

    public IComputerRepository Computers =>
        _computers ??= new ComputerRepository(_context);

    public IComputerSoftwareRepository ComputerSoftwares =>
        _computerSoftwares ??= new ComputerSoftwareRepository(_context);

    public ITeamRepository Teams =>
        _teams ??= new TeamRepository(_context);

    public IResourceTypeRepository ResourceTypes =>
        _resourceTypes ??= new ResourceTypeRepository(_context);

    public IAclRepository AclEntries =>
        _aclEntries ??= new AclRepository(_context);

    public IInstallationHistoryRepository InstallationHistories =>
        _installationHistories ??= new InstallationHistoryRepository(_context);

    public IChangeLogRepository ChangeLogs =>
        _changeLogs ??= new ChangeLogRepository(_context);

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


