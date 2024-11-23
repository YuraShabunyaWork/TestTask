using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TestTask.Data;
using TestTask.Services.Implementations;
using TestTask.Services.Interfaces;

namespace TestTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //If Your connection is not exist I use my. I realised method for check access for database. Thanks for migrations:)
                /*Task was easy, but I liked. Work with Json and Linq, perfect. 
                 * Now I am looking for new job and unfortunately 
                 * I don't have enough experience. So I am getting a lot of refusals( 
                 * But I hope take to internship*/
            var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
            var fallbackConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestTask;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            var selectedConnection = IsDatabaseAvailable(defaultConnection) ? defaultConnection : fallbackConnection;

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(selectedConnection));


            var app = builder.Build();

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

        private static bool IsDatabaseAvailable(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open(); 
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
