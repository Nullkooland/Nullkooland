using MudBlazor;
using Nullkooland.Client.Models.Components;

namespace Nullkooland.Client.ViewModels
{
    public class NavMenuViewModel
    {
        public NavMenuViewModel()
        {
            Items = new[]
            {
                new NavMenuItem {Title = "Home", Icon = Icons.Filled.Home, Url = ""},
                new NavMenuItem {Title = "Archives", Icon = Icons.Filled.AllInbox, Url = "/archives"},
                new NavMenuItem {Title = "Tags", Icon = Icons.Filled.Style, Url = "/tags"},
                new NavMenuItem {Title = "Search", Icon = Icons.Filled.Search, Url = "/search"},
                new NavMenuItem {Title = "About", Icon = Icons.Filled.TagFaces, Url = "/about"}
            };
        }

        public NavMenuItem[] Items { get; }
    }
}