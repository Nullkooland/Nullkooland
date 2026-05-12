class DarkModeHelper {
    constructor() {
        this.query = window.matchMedia("(prefers-color-scheme: dark)")
        this.dotnet = null
    }

    isDarkMode() {
        return this.query.matches;
    }

    registerColorSchemeChangedCallback(dotnetRef) {
        this.dotnet = dotnetRef
        this.query.addEventListener("change", e => {
            this.dotnet.invokeMethodAsync("OnColorSchemeChanged", e.matches)
        });
    }
};

window.darkModeHelper = new DarkModeHelper();