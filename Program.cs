using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

var host = "testdb";
var database = "TestDb";
var user = "test-user";
var password = "test-password";

var dbConnectionString = $"host={host};database={database};username={user};password={password};";
using var context = new MyDbContext(new DbContextOptionsBuilder().UseNpgsql(dbConnectionString).Options);
context.Database.EnsureCreated();

var playerId = PlayerId.Create("--wvkCLQSFiG9xpBbTTXBg");
var modStat1 = ModStat.Create("000NeMAdR1Cx6FVye4UV6Q", playerId);
var modStatLis = new List<ModStat>
{
    modStat1
};
var mod = Mod.Create("000NeMAdR1Cx6FVye4UV6Q", playerId, modStatLis);

await context.Mods.AddAsync(mod);
await context.BulkSaveChangesAsync();

public sealed class Mod
{
    public ModId Id { get; private set; }
    public PlayerId PlayerId { get; private set; }
    public IReadOnlyCollection<ModStat> Stats => _stats;
    private readonly List<ModStat> _stats = [];

    private Mod(ModId id, PlayerId playerId)
    {
        Id = id;
        PlayerId = playerId;
    }

    public static Mod Create(string id, PlayerId playerId, IEnumerable<ModStat> modStat)
    {
        var modId = ModId.Create(id);
        var mod = new Mod(modId, playerId);

        mod.AddStats(modStat);
        return mod;
    }
    private void AddStats(IEnumerable<ModStat> modStats)
    {
        _stats.AddRange(modStats);
    }
}

public sealed class ModStat
{
    public ModId ModId { get; init; }
    public PlayerId PlayerId { get; init; }

    private ModStat(ModId modId, PlayerId playerId)
    {
        ModId = modId;
        PlayerId = playerId;
    }

    public static ModStat Create(string modId, PlayerId playerId)
    {
        var mId = ModId.Create(modId);
        return new ModStat(mId, playerId);
    }
}
public sealed class PlayerId
{
    public string Value;
    private PlayerId(string value)
        => Value = value;

    public static PlayerId Create(string value)
        => new PlayerId(value);
}

public sealed class ModId
{
    public string Value;
    private ModId(string value)
        => Value = value;

    public static ModId Create(string value)
        => new ModId(value);
}

public class MyDbContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<Mod> Mods { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
        modelBuilder.Entity<Mod>()
            .ToTable("Mods");

        modelBuilder.Entity<Mod>()
           .HasKey(x => new { x.Id, x.PlayerId });

        modelBuilder.Entity<Mod>()
           .Property(x => x.Id)
           .HasConversion(x => x.Value, v => ModId.Create(v))
           .ValueGeneratedNever()
           .IsRequired();

        modelBuilder.Entity<Mod>()
           .Property(x => x.PlayerId)
           .HasConversion(x => x.Value, v => PlayerId.Create(v))
           .ValueGeneratedNever()
           .IsRequired();

        modelBuilder.Entity<Mod>()
           .HasMany(x => x.Stats)
           .WithOne();

        modelBuilder.Entity<Mod>()
           .Metadata
           .FindNavigation(nameof(Mod.Stats))!
               .SetPropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<ModStat>()
            .ToTable("ModStat");

        modelBuilder.Entity<ModStat>()
            .HasKey(x => new { x.ModId, x.PlayerId });

        modelBuilder.Entity<ModStat>()
            .Property(x => x.ModId)
            .HasConversion(x => x.Value, v => ModId.Create(v))
            .ValueGeneratedNever()
            .IsRequired();

        modelBuilder.Entity<ModStat>()
            .Property(x => x.PlayerId)
            .HasConversion(x => x.Value, v => PlayerId.Create(v))
            .ValueGeneratedNever()
            .IsRequired();
    }
}
