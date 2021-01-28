namespace Nullkooland.Client.Models.Components
{
    public record NavMenuItem
    {
        public string Title { get; init; }

        public string Icon { get; init; }

        public string Url { get; init; }
    }
}