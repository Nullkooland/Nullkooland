using System;
using System.Collections.Immutable;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using MudBlazor;
using Nullkooland.Client.Models.Theme;

namespace Nullkooland.Client.Services.Theme
{
    public class LocalThemeService : IThemeService
    {
        private readonly HttpClient _client;
        private readonly IJSRuntime _jsRuntime;

        private ImmutableDictionary<ThemeType, OolandTheme> _themes;

        public LocalThemeService(HttpClient client, IJSRuntime jsRuntime)
        {
            _client = client;
            _jsRuntime = jsRuntime;
        }

        public async ValueTask InitAsync()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new MudColorJsonConverter() }
            };

            var themes = await _client.GetFromJsonAsync<ImmutableDictionary<ThemeType, OolandTheme>>("themes.json", jsonOptions);
            _themes = themes!;

            bool isDarkMode = await _jsRuntime.InvokeAsync<bool>("darkModeHelper.isDarkMode");
            Type = isDarkMode ? ThemeType.Yunshan : ThemeType.Nullko;

            var thisRef = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("darkModeHelper.registerColorSchemeChangedCallback", thisRef);
        }

        public ThemeType Type { get; private set; }

        public event EventHandler<ThemeType>? ThemeChanged;

        [JSInvokable]
        public void OnColorSchemeChanged(bool isDarkMode)
        {
            Type = isDarkMode ? ThemeType.Yunshan : ThemeType.Nullko;
            ThemeChanged?.Invoke(this, Type);
        }

        public bool IsDark => Type switch
        {
            ThemeType.Nullko => false,
            ThemeType.Yunshan => true,
            _ => false,
        };

        public string SiteTitle => _themes[Type].SiteTitle!;

        public string AvatarImage => _themes[Type].AvatarImage!;

        public string BackgroundPattern => _themes[Type].BackgroundPattern!;

        public string GreetingsTitle => _themes[Type].GreetingsTitle!;

        public string GreetingsContent => _themes[Type].GreetingsContent!;

        public Palette Colors => _themes[Type].Colors!;

        public MudTheme MudTheme => new MudTheme
        {
            Palette = _themes[Type].Colors,
            LayoutProperties = new LayoutProperties
            {
                DefaultBorderRadius = _themes[Type].BorderRadius
            }
        };
    }
}
