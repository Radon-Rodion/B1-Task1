using React.AspNet;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Task1.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Task1.Configs;
using Task1.Services;

//Configuring Serilog logging different info into different files
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File("Logs\\Info.log"))
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File("Logs\\Warning.log"))
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File("Logs\\Error.log"))
            .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File("Logs\\Fatal.log"))
            .CreateLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    //Adding services
    var services = builder.Services;
    services.Configure<LineRandomiserConfigs>(builder.Configuration.GetSection("RandomiserOptions"));

    services.AddMemoryCache();
    //AddSingletone to prevent disposing after sending response (because many actions are performed in daemon threads)
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddSingleton<IRandomiser, LineRandomiser>();
    services.AddSingleton<IFileService, LinesFileService>();
    services.AddSingleton<LinesDbService>();
    services.AddReact(); //Frontend mixes react JS and RazorPages
    services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();

    string connection = builder.Configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connection), ServiceLifetime.Singleton);
    services.AddControllersWithViews();

    var app = builder.Build();

    app.UseDeveloperExceptionPage();
    app.UseReact(config => { });
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Files}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}