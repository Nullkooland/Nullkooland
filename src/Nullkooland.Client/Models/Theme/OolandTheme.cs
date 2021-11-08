using MudBlazor;

namespace Nullkooland.Client.Models.Theme
{
    public record OolandTheme
    {
        public string? SiteTitle { get; init; }

        public string? AvatarImage { get; init; }

        public string? BackgroundPattern { get; init; }

        public string? GreetingsTitle { get; init; }

        public string? GreetingsContent { get; init; }

        public Palette? Colors { get; init; }

        public string? BorderRadius { get; set; }
    }
}
