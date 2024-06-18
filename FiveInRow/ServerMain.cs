using FiveInRow.Client.Pages;
using FiveInRow.Components;
using FiveInRow.Hubs;

namespace FiveInRow
{
    public class ServerMain
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddAntiforgery(options =>
            {     // Set Cookie properties using CookieBuilder properties†.

                options.Cookie.Expiration = TimeSpan.Zero;

            });
            // SignalR
            builder.Services.AddSingleton(new GStorage());
            builder.Services.AddSignalR();



            

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.MapRazorComponents<App>()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(GameMp).Assembly)
                .DisableAntiforgery()
                ;

            app.MapHub<GameHub>("/gamehub");

            app.Run();
        }
    }
}
