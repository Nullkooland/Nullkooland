using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MudBlazor;
using Nullkooland.Client.Models.Theme;

namespace Nullkooland.Client.Services.Theme
{
    public class LocalThemeService : IThemeService
    {
        private readonly HttpClient _client;

        private Dictionary<OolandThemeType, OolandTheme>? _oolandThemes;

        private readonly MudTheme _nullkoTheme;

        private readonly MudTheme _yunshanTheme;

        public LocalThemeService(HttpClient client)
        {
            _client = client;

            _nullkoTheme = new MudTheme()
            {
                PaletteLight = new PaletteLight()
                {
                    Primary = "#60306C",
                    PrimaryLighten = "#E1C8F0",
                    PrimaryDarken = "#482451",
                    PrimaryContrastText = "#FFFFFF",
                    TextPrimary = "#333333",
                    Secondary = "#8C48A0",
                    Tertiary = "#E1C8F0",
                    AppbarText = "#FFFFFFFF",
                    AppbarBackground = "#60306CEE",
                    DrawerBackground = "#F8F8F8",
                },
                LayoutProperties = new LayoutProperties()
                {
                    DefaultBorderRadius = "6px"
                },
                Typography = new Typography()
                {
                    Default = new DefaultTypography()
                    {
                        FontFamily =
                        [
                            "Newsreader",
                            "STDongGuanTi",
                            "Constantia",
                            "serif"
                        ],
                        FontSize = "1.0rem",
                        FontWeight = "400",
                        LineHeight = "1.5",
                    },
                    Body1 = new Body1Typography()
                    {
                        FontSize = "1.0rem",
                        FontWeight = "400",
                        LineHeight = "1.75",
                    },
                    Body2 = new Body2Typography()
                    {
                        FontSize = "0.75rem",
                        FontWeight = "400",
                        LineHeight = "1.25",
                    },
                    Caption = new CaptionTypography()
                    {
                        FontSize = "0.875rem",
                        FontWeight = "500",
                        LineHeight = "1.25",
                    },
                    H1 = new H1Typography()
                    {
                        FontSize = "3.5rem",
                        FontWeight = "600",
                        LineHeight = "1.0",
                    },
                    H2 = new H2Typography()
                    {
                        FontSize = "2.5rem",
                        FontWeight = "700",
                        LineHeight = "1.5",
                    },
                    H3 = new H3Typography()
                    {
                        FontSize = "2.0rem",
                        FontWeight = "500",
                        LineHeight = "2.0",
                    },
                    H4 = new H4Typography()
                    {
                        FontSize = "1.5rem",
                        FontWeight = "600",
                        LineHeight = "2.0",
                    },
                    H5 = new H5Typography()
                    {
                        FontSize = "1.25rem",
                        FontWeight = "500",
                        LineHeight = "1.5",
                    },
                    H6 = new H6Typography()
                    {
                        FontSize = "1.0rem",
                        FontWeight = "600",
                        LineHeight = "1.5",
                    },
                    Subtitle1 = new Subtitle1Typography()
                    {
                        FontSize = "1.5rem",
                        FontWeight = "400",
                        LineHeight = "1.25",
                    },
                    Subtitle2 = new Subtitle2Typography()
                    {
                        FontSize = "1.25rem",
                        FontWeight = "500",
                        LineHeight = "1.0",
                    },
                    Button = new ButtonTypography()
                    {
                        FontSize = "1.0rem",
                        FontWeight = "500",
                        LineHeight = "1.0",
                    },
                    Overline = new OverlineTypography()
                    {
                        FontSize = "0.875rem",
                        FontWeight = "400",
                        LineHeight = "2.5",
                    }
                }
            };

            _yunshanTheme = new MudTheme()
            {
                PaletteDark = new PaletteLight()
                {
                    Primary = "#FF9800",
                    PrimaryLighten = "#ECCC68",
                    PrimaryDarken = "#D88100",
                    PrimaryContrastText = "#101010",
                    Secondary = "#FF6348",
                    Tertiary = "#ECCC68",
                    AppbarText = "#000000AA",
                    AppbarBackground = "#FFAB00BD",
                    Black = "#101010",
                    Background = "#181818",
                    BackgroundGray = "#242424",
                    Surface = "#282828",
                    DrawerBackground = "#323232",
                    DrawerText = "rgba(255,255,255, 0.50)",
                    DrawerIcon = "rgba(255,255,255, 0.50)",
                    TextPrimary = "rgba(255,255,255, 0.70)",
                    TextSecondary = "rgba(255,255,255, 0.50)",
                    ActionDefault = "#adadad",
                    ActionDisabled = "rgba(255,255,255, 0.25)",
                    ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                    Divider = "rgba(255,255,255, 0.12)",
                    DividerLight = "rgba(255,255,255, 0.06)",
                    TableLines = "rgba(255,255,255, 0.12)",
                    TableStriped = "#FFFFFF05",
                    TableHover = "#FFFFFF0A",
                    LinesDefault = "rgba(255,255,255, 0.12)",
                    LinesInputs = "rgba(255,255,255, 0.3)",
                    TextDisabled = "rgba(255,255,255, 0.2)",
                },
                LayoutProperties = new LayoutProperties()
                {
                    DefaultBorderRadius = "0px"
                },
                Typography = new Typography()
                {
                    Default = new DefaultTypography()
                    {
                        FontFamily =
                        [
                            "Roboto Slab",
                            "-apple-system",
                            "Segoe UI",
                            "sans-serif"
                        ],
                        FontSize = "1.0rem",
                        FontWeight = "400",
                        LineHeight = "1.5",
                    },
                    Body1 = new Body1Typography()
                    {
                        FontFamily =
                        [
                            "Saira",
                            "sans-serif"
                        ],
                        FontSize = "1.0rem",
                        FontWeight = "400",
                        LineHeight = "1.5",
                    },
                    Body2 = new Body2Typography()
                    {
                        FontFamily =
                        [
                            "Saira",
                            "sans-serif"
                        ],
                        FontSize = "0.75rem",
                        FontWeight = "400",
                        LineHeight = "1.25",
                    },
                    Caption = new CaptionTypography()
                    {
                        FontFamily =
                        [
                            "Saira",
                            "sans-serif"
                        ],
                        FontSize = "0.8rem",
                        FontWeight = "400",
                        LineHeight = "1.0",
                    },
                    H1 = new H1Typography()
                    {
                        FontSize = "3.5rem",
                        FontWeight = "600",
                        LineHeight = "1.0",
                    },
                    H2 = new H2Typography()
                    {
                        FontSize = "2.5rem",
                        FontWeight = "700",
                        LineHeight = "1.5",
                    },
                    H3 = new H3Typography()
                    {
                        FontSize = "2.0rem",
                        FontWeight = "500",
                        LineHeight = "2.0",
                    },
                    H4 = new H4Typography()
                    {
                        FontSize = "1.5rem",
                        FontWeight = "600",
                        LineHeight = "2.0",
                    },
                    H5 = new H5Typography()
                    {
                        FontSize = "1.25rem",
                        FontWeight = "700",
                        LineHeight = "1.5",
                    },
                    H6 = new H6Typography()
                    {
                        FontSize = "1.0rem",
                        FontWeight = "500",
                        LineHeight = "1.5",
                    },
                    Subtitle1 = new Subtitle1Typography()
                    {
                        FontSize = "1.4rem",
                        FontWeight = "400",
                        LineHeight = "1.25",
                    },
                    Subtitle2 = new Subtitle2Typography()
                    {
                        FontSize = "1.2rem",
                        FontWeight = "500",
                        LineHeight = "1.0",
                    },
                    Button = new ButtonTypography()
                    {
                        FontSize = "0.8rem",
                        FontWeight = "500",
                        LineHeight = "1.75",
                    },
                    Overline = new OverlineTypography()
                    {
                        FontSize = "0.8rem",
                        FontWeight = "400",
                        LineHeight = "2.5",
                    }
                }
            };
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
            _oolandThemes = themes;
        }

        public bool IsDarkMode { get; set; }

        public OolandThemeType ThemeType => IsDarkMode ? OolandThemeType.Yunshan : OolandThemeType.Nullko;

        public string SiteTitle => _oolandThemes![ThemeType].SiteTitle!;

        public string AvatarImage => _oolandThemes![ThemeType].AvatarImage!;
        
        public string AppBarIcon => _oolandThemes![ThemeType].AppBarIcon!;

        public string BackgroundPattern => _oolandThemes![ThemeType].BackgroundPattern!;

        public string GreetingsTitle => _oolandThemes![ThemeType].GreetingsTitle!;

        public string GreetingsContent => _oolandThemes![ThemeType].GreetingsContent!;

        public MudTheme MudTheme => IsDarkMode ? _yunshanTheme : _nullkoTheme;
    }
}
