using Microgroove.Core.Entities;
using Microgroove.Infrastructure.Data;
using Microgroove.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Microgroove.Functions;

public class Program
{

    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration(config => config
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json")
                    .AddEnvironmentVariables())
            .ConfigureLogging(a => a.AddConsole())
            .ConfigureServices(a =>
            {
                var cnn = new SqliteConnection("Filename=:memory:");
                cnn.Open();
                a.AddDbContext<MicrogrooveContext>(o => o.UseSqlite(cnn));
                a.AddScoped(typeof(CustomerRepository), typeof(MicrogrooveRepository<Customer>));
            })
            .Build();
            
        
       host.Run();
    }
}