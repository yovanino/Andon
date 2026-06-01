using Andon.Web.Data;
using Andon.Web.Services.ActionItems;
using Andon.Web.Services.Andon;
using Andon.Web.Services.Events;
using Andon.Web.Services.Gantt;
using Andon.Web.Services.Rca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IActionItemService, ActionItemService>();
builder.Services.AddScoped<IAndonAlertService, AndonAlertService>();
builder.Services.AddScoped<IOperationalEventService, OperationalEventService>();
builder.Services.AddScoped<IGanttIntegrationService, GanttIntegrationService>();
builder.Services.AddScoped<IRcaIntegrationService, RcaIntegrationService>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

    options
        .UseMySQL(connectionString)
        .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
