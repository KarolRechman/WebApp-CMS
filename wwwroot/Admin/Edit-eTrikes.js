function GetHref(element) {
    let catName;

    if (element != undefined) {
        catName = element;
    } else {
        catName = $(".card-header-tabs").find(".active").text();
    }

    href = $("#AddeTrike").attr("href");
    href = href.substring(0, href.lastIndexOf("=") + 1);
    href = href + catName.trim();
    $("#AddeTrike").attr("href", href);
}

$(document).ready(function () {

    GetHref();

    $(".nav-link").mouseup(function () {
        let element = $(this).text();
        GetHref(element.trim());
    });
});
