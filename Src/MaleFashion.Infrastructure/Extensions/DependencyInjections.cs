using MaleFashion.Application;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Contracts.Services;
using MaleFashion.Application.Services;
using MaleFashion.Infrastructure.Data.Repositories;
using MaleFashion.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MaleFashion.Infrastructure.Extensions
{
    public static class DependencyInjections
    {


        public static IServiceCollection AddInfrastructureDependency(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();

            services.AddSingleton<IServerTime, ServerTime>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddSingleton<IFileStorageService, FileStorageService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            services.AddScoped<IOrderRepository,OrderRepository>();

            services.AddScoped<ITransactionManager,EfTransactionManager>();


            services.Configure<GoogleReCaptchaOptions>(
           configuration.GetSection(
               GoogleReCaptchaOptions.SectionName));

            services.AddHttpClient<
                IGoogleReCaptchaService,
                GoogleReCaptchaService>();


            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");

            var migrationAssembly =
                typeof(ApplicationDbContext).Assembly;

           

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sql => sql.MigrationsAssembly(
                        migrationAssembly.GetName().Name)));


         

            return services;
        }

    }
}
