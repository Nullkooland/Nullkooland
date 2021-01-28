using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Nullkooland.Client.Services.Markdown;
using Nullkooland.Client.Services.Post;
using Nullkooland.Client.ViewModels;

namespace Nullkooland.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(
                sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

            builder.Services.AddScoped<IBlogPostService, LocalBlogPostService>();
            builder.Services.AddScoped<MarkdownRenderService>();

            builder.Services.AddMudBlazorDialog();
            builder.Services.AddMudBlazorSnackbar();
            builder.Services.AddMudBlazorResizeListener();

            AddViewModels(builder);

            await builder.Build().RunAsync();
        }

        private static void AddViewModels(WebAssemblyHostBuilder builder)
        {
            // Main layout
            builder.Services.AddScoped<MainLayoutViewModel>();
            builder.Services.AddScoped<NavMenuViewModel>();
            // Pages
            builder.Services.AddScoped<HomePageViewModel>();
            builder.Services.AddScoped<ArchivesPageViewModel>();
            builder.Services.AddScoped<TagsPageViewModel>();
            builder.Services.AddScoped<PostPageViewModel>();
            builder.Services.AddScoped<SearchPageViewModel>();
            builder.Services.AddScoped<AboutPageViewModel>();
            // Controls
            builder.Services.AddScoped<ArchivesViewerViewModel>();
        }
    }
}