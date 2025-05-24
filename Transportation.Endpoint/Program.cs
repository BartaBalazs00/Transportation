
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportation.Data;
using Transportation.Endpoint.Helper;
using Transportation.Logic;
using Transportation.Logic.Dto;

namespace Transportation.Endpoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(opt => {
                opt.Filters.Add<ExceptionFilter>();
                opt.Filters.Add<ValidationFilter>();
            });
            builder.Services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient(typeof(Repository<>));
            builder.Services.AddTransient<DtoProvider>();
            builder.Services.AddTransient<VehicleLogic>();
            builder.Services.AddDbContext<TransportationContext>(opt =>
            {
                opt.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TransportationDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True")
                .UseLazyLoadingProxies();
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
