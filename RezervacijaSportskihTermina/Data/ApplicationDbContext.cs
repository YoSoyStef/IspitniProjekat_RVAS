using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Sport> Sportovi { get; set; }
    public DbSet<SportskiCentar> SportskiCentri { get; set; }
    public DbSet<Termin> Termini { get; set; }
    public DbSet<KorisnikTermin> KorisnikTermini { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Termin>()
            .Property(t => t.Cena)
            .HasColumnType("decimal(18,2)");

        builder.Entity<KorisnikTermin>()
            .HasKey(kt => new { kt.ApplicationUserId, kt.TerminId });

        builder.Entity<KorisnikTermin>()
            .HasOne(kt => kt.ApplicationUser)
            .WithMany(k => k.Termini)
            .HasForeignKey(kt => kt.ApplicationUserId);

        builder.Entity<KorisnikTermin>()
            .HasOne(kt => kt.Termin)
            .WithMany(t => t.Korisnici)
            .HasForeignKey(kt => kt.TerminId);
    


}
}
