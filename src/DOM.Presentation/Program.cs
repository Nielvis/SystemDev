using DOM.Presentation.Implementation.Interfaces;
using DOM.Presentation.Implementation.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);

builder.Services.AddRouting();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddControllersWithViews();

// Dependences
builder.Services.AddScoped<IConstantsService, ConstantsService>();
builder.Services.AddScoped<IDbService, DbService>();

var app = builder.Build();

app.UseStaticFiles();

app.UseCors("AllowAnyOrigin");

app.UseExceptionHandler("/Home/Error");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();