using BervProject.WebApi.Boilerplate.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BervProject.WebApi.Boilerplate.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BoilerplateDbContext>
{
    public BoilerplateDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<BoilerplateDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("BoilerplateConnectionString"));

        return new BoilerplateDbContext(optionsBuilder.Options);
    }
}
