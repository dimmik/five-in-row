using FiveInRow.Client.Pages;
using FiveInRow.Components;
using FiveInRow.Hubs;
using FiveInRow.Storage;

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
            var storageType = builder.Configuration.GetValue<string>("Storage");
            if (string.Equals(storageType, "mongo", StringComparison.OrdinalIgnoreCase))
            {
                var srv = builder.Configuration.GetValue<string>("MongoSrv") ?? "_none";
                var login = builder.Configuration.GetValue<string>("MongoLogin") ?? "_none";
                var pwd = builder.Configuration.GetValue<string>("MongoPwd") ?? "_none";
                var sp = new MongoGStorage(srv, login, pwd);
                builder.Services.AddSingleton<IGStorage>(sp);
            }
            else // Default to in-memory storage
            {
                builder.Services.AddSingleton<IGStorage>(new InMemoryGStorage());
            }
            // SignalR
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
