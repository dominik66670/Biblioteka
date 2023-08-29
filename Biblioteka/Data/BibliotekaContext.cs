using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Models;

namespace Biblioteka.Data;

public class BibliotekaContext : IdentityDbContext<IdentityUser>
{
    public BibliotekaContext(DbContextOptions<BibliotekaContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Biblioteka.Models.Autor>? Autor { get; set; }

    public DbSet<Biblioteka.Models.LibraryUser>? LibraryUser { get; set; }

    public DbSet<Biblioteka.Models.Status>? Status { get; set; }

    public DbSet<Biblioteka.Models.Zasob>? Zasob { get; set; }

    public DbSet<Biblioteka.Models.Zbior>? Zbior { get; set; }

    public DbSet<Biblioteka.Models.Wyporzyczenie>? Wyporzyczenie { get; set; }
}
