using Core.Entities;
using Core.Helpers;
using Core.RepositoriesContracts;
using Core.Services.Contracts;
using Core.Services.Implementations;
using Infrastructure.DB;
using Infrastructure.Repositories.Implemenations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            builder.Services.AddScoped<ITokenRepository, TokenRepository>();


            // Regiseter services
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBorrowingRecordService, BorrowingRecordService>();
            builder.Services.AddScoped<IJwtServices, JwtServices>();
            builder.Services.AddScoped<ServicesHelpers>();
            builder.Services.AddScoped<IEmailService, EmailService>();


            // Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<LibraryDbContext>()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, LibraryDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, LibraryDbContext, Guid>>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("LIBRARYMANAGER_DEV_JWT_ISSUER"),
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("LIBRARYMANAGER_DEV_JWT_SECRET")!))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = async context =>
                    {
                        var token = context.HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1];
                        if (token != null)
                        {
                            var tokenService = context.HttpContext.RequestServices.GetRequiredService<IJwtServices>();
                            if (tokenService.IsExpiredToken(token))
                            {
                                context.Fail("Expired Token");
                            }
                        }
                    }
                };
            });

/*            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated successfully");
                    return Task.CompletedTask;
                }
            };*/

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("NoAuthenticated", p =>
                {
                    p.RequireAssertion(context =>
                    {
                        return !context.User.Identity!.IsAuthenticated;
                    });
                });
            });


            return builder;
        }

    }
}
