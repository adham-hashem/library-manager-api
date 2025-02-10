using Core.RepositoriesContracts;
using Core.Services.Contracts;
using Core.Services.Implementations;
using Infrastructure.DB;
using Infrastructure.Repositories.Implemenations;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagerAPI.Extensions
{
    public static class ServicesResgisterationExtension
    {
        public static WebApplicationBuilder RegisterAndConfigureServices(this WebApplicationBuilder builder, IWebHostEnvironment env)
        {
            builder.Services.AddDbContext<LibraryDbContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("LIBRARYMANAGER_DEV_DATABASE"));

                if (env.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }
            });


            // Register repositories
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IBorrowingRecordRepository, BorrowingRecordRepository>();


            // Regiseter services
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBorrowingRecordService, BorrowingRecordService>();

            return builder;
        }

    }
}
