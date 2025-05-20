using Domain.Users;
using Domain.Currencies;
using Domain.Deals;
using Domain.Orders;
using Domain.Wallets;
using Microsoft.EntityFrameworkCore;
using Application.Services;

namespace Infrastructure;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options), IUnitOfWork {
    public DbSet<Order> Orders { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<Wallet> Wallets { get; init; }
    public DbSet<Currency> Currencies { get; init; }
    public DbSet<Deal> Deals { get; init; }

    public Task<int> SaveChanges(CancellationToken token = default) => SaveChangesAsync(token);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
}
