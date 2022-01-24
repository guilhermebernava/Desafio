using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using qodeless.desafio.domain.Entities;

namespace qodeless.desafio.Infra.CrossCutting.identity.Data
{
    public interface IAppDbContext { }

    public  class ApplicationDbContext : IdentityDbContext<IdentityUser>, IAppDbContext
    {
        public DbSet<Ubuntu> Ubuntus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new Ubuntu().Configure(modelBuilder.Entity<Ubuntu>().ToTable("Ubuntus"));

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
    

}
