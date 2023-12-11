using Blazored.LocalStorage;
using BlazorStrap;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Grades.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<IGuidService, GuidService>();
            builder.Services.AddScoped<IGradeService, GradeService>();
            builder.Services.AddScoped<IStorageService, StorageService>();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazorStrap();

            await builder.Build().RunAsync();
        }
    }
}
