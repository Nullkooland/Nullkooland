namespace Nullkooland.Client.ViewModels
{
    public class MainLayoutViewModel
    {
        public bool IsNavMenuOpened { get; set; }

        public void OnNavMenuButtonClicked()
        {
            IsNavMenuOpened = !IsNavMenuOpened;
        }
    }
}