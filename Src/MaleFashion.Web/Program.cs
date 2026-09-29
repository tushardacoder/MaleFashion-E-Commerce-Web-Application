using Cortex.Mediator.DependencyInjection;
using Demo.Infrastructure.Identity;
using MaleFashion.Application.Features.ContactMessages.Command;
using MaleFashion.Domain.Utilities;
using MaleFashion.Infrastructure;
using MaleFashion.Infrastructure.Extensions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;




Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/bootstrap-.log", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();


try { 
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = typeof(ApplicationDbContext).Assembly;

    #region Dependency Injection

    //builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //   options.UseSqlServer(connectionString));

    builder.Services.AddInfrastructureDependency(
     builder.Configuration);

    #endregion

    #region SerilogSetup

    builder.Host.UseSerilog((context, lc) =>
        lc.ReadFrom.Configuration(context.Configuration));

    #endregion

    #region Cortex Mediator Configuration


    builder.Services.AddCortexMediator(
        new[] { typeof(Program), typeof(ContactUsAddCommand) },
        options => options.AddDefaultBehaviors()
    );

    #endregion


    #region Mapster Configuration

    // Custom Configuration
    //var config = TypeAdapterConfig.GlobalSettings;
    //config.Scan(typeof(MapsterConfiguration).Assembly);
    //builder.Services.AddSingleton(config);
    //builder.Services.AddScoped<IMapper, ServiceMapper>();

    // Default Configuration
    builder.Services.AddMapster();

    #endregion


    #region DbContext Configuration
    //builder.Services.AddInfrastructureDependency(connectionString, migrationAssembly);
    builder.Services.AddDbContext(connectionString, migrationAssembly);
    #endregion

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    #region Identity Configuration
    builder.Services.AddIdentitySetup();

    #endregion


    #region AppSettings Configuration
    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
    #endregion

    builder.Services.AddRazorPages();
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    //    .AddEntityFrameworkStores<ApplicationDbContext>();


    builder.Services.AddControllersWithViews();

    Log.Information("Before app.Run()");
    Console.WriteLine("BEFORE APP.RUN");


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    //app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
 
    app.MapStaticAssets();
    app.UseStaticFiles();

app.MapControllerRoute(
 name: "areas",
 pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index1}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
       .WithStaticAssets();

    Log.Information("Starting application");

 
  

    app.Run();

    Console.WriteLine("After APP.RUN");
}
catch (Exception e)
{
    Log.Fatal(e, "Application crashed");
    Console.WriteLine("====================================");
    Console.WriteLine("APPLICATION CRASHED:");
    Console.WriteLine(e.ToString());
    Console.WriteLine("====================================");

    throw;
}
finally
{
    Log.CloseAndFlush();
}
