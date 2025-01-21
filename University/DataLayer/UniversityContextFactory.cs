using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace University.DataLayer
{
    internal class UniversityContextFactory : IDesignTimeDbContextFactory<UniversityContext>
    {
        public UniversityContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<UniversityContext>();

            var config = new ConfigurationBuilder()
                .AddUserSecrets<UniversityContextFactory>()
                .Build();

            return new UniversityContext(builder.UseSqlServer(config["DbConnectionString"]).Options);
        }
    }
}
