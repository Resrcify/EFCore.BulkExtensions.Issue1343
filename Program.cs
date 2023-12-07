using EFCore.BulkExtensions;
using EFCore.BulkExtensions.Issue1343.Entities.Mods;
using EFCore.BulkExtensions.Issue1343.Entities.Mods.Enums;
using EFCore.BulkExtensions.Issue1343.Entities.Mods.ValueObjects;
using Microsoft.EntityFrameworkCore;

var host = "testdb";
var database = "TestDb";
var user = "test-user";
var password = "test-password";

var dbConnectionString = $"host={host};database={database};username={user};password={password};";
using var context = new MyDbContext(new DbContextOptionsBuilder().UseNpgsql(dbConnectionString).Options);

context.Database.EnsureCreated();

var playerId = PlayerId.Create("--wvkCLQSFiG9xpBbTTXBg");
var modStat1 = ModStat.Create("000NeMAdR1Cx6FVye4UV6Q", playerId.Value, StatType.Unitstatmaxhealth, 1, 2, 2, false);
var modStat2 = ModStat.Create("000NeMAdR1Cx6FVye4UV6Q", playerId.Value, StatType.Unitstatmaxhealth, 2, 2, 2, false);
var modStatLis = new List<ModStat>
{
    modStat1.Value,
    modStat2.Value
};
var mod = Mod.Create("000NeMAdR1Cx6FVye4UV6Q", playerId.Value, "163", ModType.POTENCY, ModRarity.MODRARITY3, ModSlot.ARROW, 1, ModTier.A, 2, modStatLis);


await context.Mods.AddAsync(mod.Value);
await context.BulkSaveChangesAsync();

public class MyDbContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<Mod> Mods { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
    }
}