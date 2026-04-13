using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Domain.Entities.ErrorDictionary;
using BimManagerPortal.Domain.Entities.PluginConfigurations;
using BimManagerPortal.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BimManagerPortal.Persistance.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<PluginBigData> BigDataPlugins { get; set; }
    public DbSet<PluginConfiguration> PluginConfigurations { get; set; }
    public DbSet<ErrorDictionaryEntry> ErrorDictionaryEntries { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Применяем все конфигурации из текущей сборки
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}