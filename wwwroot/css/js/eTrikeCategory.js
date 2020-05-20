function preLoadImage(img) {
    const src = img.getAttribute("data-src");
    if (!src) {
        return;
    }

    img.src = src;
}

const images = document.querySelectorAll("[data-src]");
const options = {
    threshold: 0
};
const imgObserver = new IntersectionObserver((entries, imgObserver) => {
    entries.forEach(entry => {
        if (!entry.isIntersecting /*isIntersection*/) {
            return;
        } else {
            preLoadImage(entry.target);
            imgObserver.unobserve(entry.target);
        }
    });
}, options);

images.forEach(image => {
    imgObserver.observe(image);
});

$(document).ready(function () {

    //$('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
    //    if (window.screen.width < 999 || document.body.clientWidth < 999) {
    //        if (!$(this).next().hasClass('SubMClick')) {
    //            $(this).parents('.dropdown-menu').first().find('.SubMClick').removeClass('SubMClick');
    //        }
    //        var $subMenu = $(this).next('.dropdown-menu');
    //        $subMenu.toggleClass('SubMClick');


    //        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
    //            $('.dropdown-submenu .SubMClick').removeClass('SubMClick');
    //        });

    //    }


    //    return false;
    //});
});