using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Nullkooland.Client.Services.Markdown;
using Nullkooland.Client.Services.Post;
using Nullkooland.Client.Services.Theme;
using Nullkooland.Client.ViewModels.Components;
using Nullkooland.Client.ViewModels.Pages;
using Nullkooland.Client.ViewModels.Shared;

namespace Nullkooland.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            builder.Services.AddScoped(sp => client);

            builder.Services.AddMudServices();

            builder.Services.AddScoped<IThemeService, LocalThemeService>();
            builder.Services.AddScoped<IBlogPostService, LocalBlogPostService>();
            builder.Services.AddScoped<MarkdownRenderService>();

            AddViewModels(builder);

            var host = builder.Build();
            await LoadTheme(host);

            await host.RunAsync();
        }

        private static void AddViewModels(WebAssemblyHostBuilder builder)
        {
            // Main layout
            builder.Services.AddScoped<MainLayoutViewModel>();
            // Pages
            builder.Services.AddScoped<HomePageViewModel>();
            builder.Services.AddScoped<ArchivesPageViewModel>();
            builder.Services.AddScoped<TagsPageViewModel>();
            builder.Services.AddScoped<PostPageViewModel>();
            builder.Services.AddScoped<SearchPageViewModel>();
            builder.Services.AddScoped<AboutPageViewModel>();
            // Components
            builder.Services.AddScoped<ArchivesViewerViewModel>();
            builder.Services.AddScoped<ImageDialogViewModel>();
        }

        private static ValueTask LoadTheme(WebAssemblyHost host)
        {
            var themeService = host.Services.GetRequiredService<IThemeService>();
            return themeService.InitAsync();
        }
    }
}