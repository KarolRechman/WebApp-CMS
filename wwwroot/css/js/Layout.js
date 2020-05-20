$(document).ready(function () {

    var Menu = $("#DDMenu");
    Menu.parent().find("a").on("click", function () {
        Menu.children().toggleClass("show");
        //Menu.css("min-height", "50vh!important;");
    });

    $("#Year").html(new Date().getFullYear());

    var urlHash = window.location.href.split("#")[1];
    if (urlHash != undefined) {
        if (urlHash.length > 0) {
            var offset = 120;
            if (urlHash == "Kontakt") {
                offset = 170;
            }
            $('html,body').animate({
                scrollTop: $($('#' + urlHash)).offset().top - offset
            }, 400, 'swing', 'slow', function () {

                // Add hash (#) to URL when done scrolling (default click behavior)
                window.location.hash = $(hash).offset().top - 120;
            });
        }
    }

    ResponsiveNav(Menu);
    $(window).resize(function () {
        ResponsiveNav(Menu);
    });
});

function ResponsiveNav(Menu) {
    //var Menu = $("#DDMenu");
    if (window.screen.width < 780 || document.body.clientWidth < 780) {

        Menu.removeClass("dropleft");
        Menu.removeClass("dropdown-menu");

        //Menu.parent().addClass("accordion");
        Menu.children().addClass("collapse");

        Menu.addClass("MobileListMenu");

        //Menu.parent().find("a").on("click", function () {
        //    Menu.children().toggleClass("show");
        //    Menu.css("min-height", "50vh!important;");
        //});

        $("#scroll_Spy").addClass("OverFlowNav");

    } else {
        Menu.addClass("dropleft");
        Menu.addClass("dropdown-menu");

        Menu.removeClass("MobileListMenu");
        Menu.children().removeClass("collapse");

        $("#scroll_Spy").removeClass("OverFlowNav");

    }
}

String.prototype.splice = function (start, delCount, newSubStr) {
    return this.slice(0, start) + newSubStr + this.slice(start + Math.abs(delCount));
};

