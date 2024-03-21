using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection;
using Task2.DataContext.Models;
using Task2.DataContext.Utilities;

namespace Task2.DataContext;

public partial class AppDbContext : DbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<District> Districts { get; set; }
    public virtual DbSet<Estate> Estates { get; set; }
    public virtual DbSet<Material> Materials { get; set; }
    public virtual DbSet<Realtor> Realtors { get; set; }
    public virtual DbSet<Sale> Sales { get; set; }
    public virtual DbSet<Score> Scores { get; set; }
    public virtual DbSet<ScoreCriteria> ScoreCriterias { get; set; }
    public virtual DbSet<EstateType> Types { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Districts

        builder.Entity<District>()
            .HasKey(e => e.ID);

        // Estates

        builder.Entity<Estate>()
            .HasKey(e => e.ID);
        builder.Entity<Estate>()
            .HasOne(e => e.District)
            .WithMany(e => e.EstatesOfDistrict);
        builder.Entity<Estate>()
            .HasOne(e => e.Type)
            .WithMany();
        builder.Entity<Estate>()
            .HasMany(e => e.Materials)
            .WithMany(e => e.Estates);
        builder.Entity<Estate>()
            .HasMany(e => e.Scores)
            .WithOne()
            .HasForeignKey(e => e.EstateID)
            .IsRequired();

        // Materials

        builder.Entity<Material>()
            .HasKey(e => e.ID);

        // Realtors

        builder.Entity<Realtor>()
            .HasKey(e => e.ID);
        builder.Entity<Realtor>()
            .HasMany(e => e.Sales)
            .WithOne(e => e.Realtor);

        // Sales

        builder.Entity<Sale>()
            .HasKey(e => e.ID);
        builder.Entity<Sale>()
            .HasOne(e => e.Estate)
            .WithOne(e => e.Sale)
            .HasForeignKey<Estate>(e => e.SaleID);

        // Scores

        builder.Entity<Score>()
            .HasKey(e => e.ID);
        builder.Entity<Score>()
            .HasOne(e => e.Criteria)
            .WithMany();

        // Criterias

        builder.Entity<ScoreCriteria>()
            .HasKey(e => e.ID);

        // Functions

        //var FCsCreatorMethodInfo = typeof(FCsCreator)
        //    .GetRuntimeMethod(
        //        nameof(FCsCreator.GetFCsJoined),
        //        new[] { typeof(Realtor) }
        //     );

        //builder.HasDbFunction(FCsCreatorMethodInfo!)
        //    .HasTranslation(args => 
        //        SqlFunctionExpression.
        //    )

    }

}