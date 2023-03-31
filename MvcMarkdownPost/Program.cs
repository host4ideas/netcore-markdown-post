using Microsoft.EntityFrameworkCore;
using MvcMarkdownPost.Data;
using MvcMarkdownPost.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Obtenemos la connectionString de appsettings.json
string connectionString = builder.Configuration.GetConnectionString("SqlPost");

// Activamos el contexto para la inyeccion
builder.Services.AddDbContext<PostsContext>(options => options.UseSqlServer(connectionString));
// Activamos el repositorio que hemos creado para la inyeccion
builder.Services.AddTransient<RepositoryPosts>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Modificamos la ruta por defecto para que sea la ruta a los posts
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}");

app.Run();
