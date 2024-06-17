using FiveInRow.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

internal class ClientMain
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        //builder.Services.AddAuthorizationCore();
        //builder.Services.AddCascadingAuthenticationState();
        //builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
        await builder.Build().RunAsync();
    }
}