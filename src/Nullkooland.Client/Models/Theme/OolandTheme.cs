using MudBlazor;

namespace Nullkooland.Client.Models.Theme
{
    public enum OolandThemeType
    {
        // Moonbear of the day.
        Nullko,

        // Tiger of the night.
        Yunshan,
    }

    public record OolandTheme
    {
        public string? SiteTitle { get; init; }

        public string? AvatarImage { get; init; }

        public string? BackgroundPattern { get; init; }

        public string? GreetingsTitle { get; init; }

        public string? GreetingsContent { get; init; }

        public PaletteDark? DarkColors { get; init; }

        public PaletteLight? LightColors { get; init; }

        public string? BorderRadius { get; init; }

        public Typography? Typography { get; init; }
    }
}
