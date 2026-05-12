window.utterancesHelper = {
    render: function (element) {
        let utterances = document.createElement("script");
        utterances.type = "text/javascript";
        utterances.src = "https://utteranc.es/client.js";
        utterances.async = true;
        utterances.crossorigin = "anonymous";
        utterances.setAttribute("issue-term", "pathname")
        utterances.setAttribute("theme", "preferred-color-scheme")
        utterances.setAttribute("repo", "Goose-Bomb/Nullkooland")
        element.appendChild(utterances);
    }
};