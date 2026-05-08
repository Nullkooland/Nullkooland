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

        private Dictionary<OolandThemeType, OolandTheme> _themes;

        public LocalThemeService(HttpClient client)
        {
            _client = client;
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

            var themes =
                await _client.GetFromJsonAsync<Dictionary<OolandThemeType, OolandTheme>>("themes.json", jsonOptions);
            _themes = themes!;
        }

        public bool IsDarkMode { get; set; }

        public OolandThemeType ThemeType => IsDarkMode ? OolandThemeType.Yunshan : OolandThemeType.Nullko;

        public string SiteTitle => _themes[ThemeType].SiteTitle!;

        public string AvatarImage => _themes[ThemeType].AvatarImage!;

        public string BackgroundPattern => _themes[ThemeType].BackgroundPattern!;

        public string GreetingsTitle => _themes[ThemeType].GreetingsTitle!;

        public string GreetingsContent => _themes[ThemeType].GreetingsContent!;

        public MudTheme MudTheme => new()
        {
            PaletteDark = _themes[OolandThemeType.Yunshan].DarkColors!,
            PaletteLight = _themes[OolandThemeType.Nullko].LightColors!,
            LayoutProperties = new LayoutProperties
            {
                DefaultBorderRadius = _themes[ThemeType].BorderRadius!
            },
            Typography = _themes[ThemeType].Typography!
        };
    }
}
