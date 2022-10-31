using Core.DataAccess.Entities;
using Core.DataAccess.Repositories;
using Core.Domain.Services;
using Infrastructure.DataAccess.Data;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.DataAccess.Repositories.Base;
using Infrastructure.Domain.Mappers;
using Infrastructure.Domain.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Application;

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
                a.AddDbContext<BusinessContext>(o => o.UseSqlite(cnn));
                a.AddSingleton(new LoggerFactory().CreateLogger("Business"));
                a.AddAutoMapper(typeof(CustomerMapper));
                a.AddScoped(typeof(ICustomerRepository), typeof(CustomerRepository));
                a.AddScoped(typeof(ICustomerService), typeof(CustomerService));
            })
            .Build();


        host.Run();
    }
}