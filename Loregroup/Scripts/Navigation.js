Desertfire.Navigation = {}
Desertfire.Navigation.SetupNav = function () {
    $(document).on("click", "a[data-action-url]", function(e) {
        e.preventDefault();
        var element = $(this);
        var actionUrl = element.data("action-url");
        var actionType = element.data("action-type");
        switch (actionType) {
            case "Redirect":
                Desertfire.ChangeLocation(actionUrl);
            case "Ajax":
            case "Overlay":
            case "Alert":
        default:
        }
    });
}