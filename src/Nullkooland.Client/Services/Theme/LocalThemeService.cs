using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        private Dictionary<OolandThemeType, OolandTheme> _themes;

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
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                    new MudColorJsonConverter() 
                }
            };

            var themes = await _client.GetFromJsonAsync<Dictionary<OolandThemeType, OolandTheme>>("themes.json", jsonOptions);
            _themes = themes!;

            bool isDarkMode = await _jsRuntime.InvokeAsync<bool>("darkModeHelper.isDarkMode");
            Type = isDarkMode ? OolandThemeType.Yunshan : OolandThemeType.Nullko;

            var thisRef = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("darkModeHelper.registerColorSchemeChangedCallback", thisRef);
        }

        public OolandThemeType Type { get; private set; }

        public event EventHandler<OolandThemeType>? ThemeChanged;

        [JSInvokable]
        public void OnColorSchemeChanged(bool isDarkMode)
        {
            Type = isDarkMode ? OolandThemeType.Yunshan : OolandThemeType.Nullko;
            ThemeChanged?.Invoke(this, Type);
        }

        public bool IsDark => Type switch
        {
            OolandThemeType.Nullko => false,
            OolandThemeType.Yunshan => true,
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
