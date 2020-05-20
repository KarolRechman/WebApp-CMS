var pageURL = window.location.origin;
if (pageURL.includes("localhost") == true) {
    pageURL = window.location.pathname;
}

(() => {
    'use strict';
    // Page is loaded
    const objects = document.getElementsByClassName('asyncImage');
    Array.from(objects).map((item) => {
        // Start loading image
        const img = new Image();
        img.src = item.dataset.src;
        // Once image is loaded replace the src of the HTML element
        img.onload = () => {
            item.classList.remove('asyncImage');
            return item.nodeName === 'IMG' ?
                item.src = item.dataset.src :
                item.style.backgroundImage = `url(${item.dataset.src})`;
        };
    });
})();

$(window).on("load", function () {
    $(".loader-wrapper").fadeOut("slow");
});
////////////////////////////////////////////////
var geocoder;
var infowindow;
var map;

/*lazy load 4 imgs*/
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
    $(image).fadeIn();
});

var lat = sessionStorage.getItem('lat');
var lng = sessionStorage.getItem('lng');
var service = new Array();

function load() {
    var markers = [];

    if (markers.length == 0) {
        //ajax for locations, create url var and handler on razor page !!!!!!!!!!!!!
        var url = pageURL + '/GMapCoordinates';
        if ((url.includes("//") == true) && (url.includes("http") != true)) {
            url = url.replace("//", "/");
        }
        $.ajax({
            async: false,
            url: url,
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {

                for (var i = 0; i < result.length; i++) {
                    var txt = "<div class='card'><h4>" + result[i].name + "</h4><p>";
                    if (result[i].name2 != "") {
                        txt += result[i].name2 + "<br>";
                    }
                    txt += result[i].address + "<br><br>" + result[i].postCode + " " + result[i].location + "</p><div><p><a href='" + result[i].link + "' target='_blank' rel='noopener'>Mehr zum Standort</a></p></div></div>";

                    if (txt.includes("Händler")) {
                        service.push({
                            lat: result[i].lat,
                            lng: result[i].lng
                        });
                    }

                    markers.push({
                        lat: result[i].lat,
                        lng: result[i].lng,
                        txt: txt,
                        icon: result[i].icon
                    });
                }
                if (lat != null && lng != null) {
                    markers.push({
                        lat: lat,
                        lng: lng,
                        txt: "Du bist hier !",
                        icon: pageURL + "images/blue-dot.webp"
                    })
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
    return markers;
}

$(document).ready(function () {

    var css = "url('" + pageURL + "images/car.svg')";
    $("#Car").css("background-image", css);

    $('#navCollapse>li>a').on('click', function () {
        $('.navbar-collapse').collapse('hide');
    });

    $(".dropdown-item").css('color', 'white');

    // Add smooth scrolling to all links
    $(".smooth_Link").on('click', function (event) {
        // Make sure this.hash has a value before overriding default behavior
        if (this.hash !== "") {
            // Prevent default anchor click behavior
            event.preventDefault();

            // Store hash
            var hash = this.hash;

            // Using jQuery's animate() method to add smooth page scroll
            // The optional number (400) specifies the number of milliseconds it takes to scroll to the specified area
            $('html,body').animate({
                scrollTop: $(hash).offset().top - 120
            }, 400, 'swing', 'slow', function () {

                // Add hash (#) to URL when done scrolling (default click behavior)
                window.location.hash = $(hash).offset().top - 120;
            });
        } // End if
    });
    $('#carousel-example').on('slide.bs.carousel', function (e) {

        var $e = $(e.relatedTarget);
        var idx = $e.index();
        var itemsPerSlide = 4;
        var totalItems = $('.carousel-item').length;

        if (idx >= totalItems - (itemsPerSlide - 1)) {
            var it = itemsPerSlide - (totalItems - idx);
            for (var i = 0; i < it; i++) {
                // append slides to end
                if (e.direction == "left") {
                    $('.carousel-item').eq(i).appendTo('.carousel-inner');
                } else {
                    $('.carousel-item').eq(0).appendTo('.carousel-inner');
                }
            }
        }
    });
});

//spysrcroll function
$(window).bind('scroll', function () {
    var currentTop = $(window).scrollTop();
    var elems = $('.scrollspy');
    elems.each(function (index) {
        var elemTop = $(this).offset().top - 175;
        var elemBottom = elemTop + $(this).height();
        var id = $(this).attr('id');
        var navElem = $('a[href="#' + id + '"]');
        if (currentTop >= elemTop && currentTop <= elemBottom) {
            navElem.parent().addClass('active').siblings().removeClass('active');
            navElem.parent().css('border-bottom', '2px solid white');
            navElem.parent().css('transition', '0.5s ease');
            navElem.parent().siblings().css('border-bottom', 'none');

        } else if (currentTop <= elemTop) {
            navElem.parent().removeClass('active');
            navElem.parent().css('border-bottom', 'none');
        }
    })
    if (window.screen.width > 780 || document.body.clientWidth > 780) {
        var color = $("#scroll_Spy").css('background-color');
        if (color != 'rgba(0, 0, 0, 0.2)') {
            $(".dropdown-menu").css('background-color', '#4caf50');
            $(".dropdown-item").css('color', 'black');
        }
        if (currentTop == 0) {
            $(".dropdown-menu").css('background-color', 'rgba(0, 0, 0, 0.7)');
            $(".dropdown-item").css('color', 'white');

        }
    }
});


function initMap() {

    var Messages = load();

    let center = { lat: 52.00, lng: 10.37 };
    let zoom = 6;
    let Client;
    let Shop;

    if (lat != null && lng != null) {
        Client = new google.maps.LatLng(lat, lng);
        Shop = new google.maps.LatLng(service[0].lat, service[0].lng);

        zoom = 5;
    }

    map = new google.maps.Map(document.getElementById('googleMap'), {
        center: center,
        zoom: zoom
    });

    if (Client != null && Shop != null) {
        var bounds = new google.maps.LatLngBounds();
        bounds.extend(Client);
        bounds.extend(Shop);
        map.fitBounds(bounds);
    }

    for (var i = 0; i < Messages.length; ++i) {
        var marker = new google.maps.Marker({
            position: {
                lat: parseFloat(Messages[i].lat),
                lng: parseFloat(Messages[i].lng)
            },
            icon: Messages[i].icon,
            map: map
        });
        attachMessage(marker, Messages[i].txt);
    }

    var legend = document.getElementById('legend');
    var div = document.createElement('div');
    div.innerHTML = '<P><img src="' + pageURL + 'images/orange-dot.webp" width ="20"></img><span class="legend_Span"> - Service</span></P>';
    div.innerHTML += '<P><img src="' + pageURL + 'images/green-dot.webp" width ="20"></img><span class="legend_Span"> - Händler</span> <br><span class="legend_Span">&nbsp;&nbsp;&nbsp;&nbsp;und Service</span></P>';

    if (lat != null && lng != null) {
        div.innerHTML += '<P><img src="' + pageURL + 'images/blue-dot.webp"  width ="20"></img><span class="legend_Span"> - Du bist hier</span></P>';
    }

    legend.appendChild(div);

    map.controls[google.maps.ControlPosition.LEFT_TOP].push(legend);
}

function attachMessage(marker, Message) {
    var infowindow = new google.maps.InfoWindow({
        content: Message
    });

    marker.addListener('click', function () {
        infowindow.open(marker.get('map'), marker);
    });
}

function Send() {

    var email = new Array();
    var email = {
        Name: $("#name").val(),
        Email: $("#email").val(),
        Subject: $("#subject").val(),
        Message: $("#message").val()
    }

    const isEmpty = Object.values(email).every(x => (x === null || x === ''));
    if (isEmpty === false) {
        var Json = JSON.stringify(email);
        var url = pageURL + '/Email';
        if ((url.includes("//") == true) && (url.includes("http") != true)) {
            url = url.replace("//", "/");
        }
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            headers: {
                "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            dataType: "json",
            data: Json,
            success: function (result) {
                var s = result.email;
                if (s != "false") {
                    $("#name").val("");
                    $("#email").val("");
                    $("#subject").val("");
                    $("#message").val("");
                    $("#statusMsg").html("Email wurde gesendet");
                } else {
                    $('label[for="email"]').html("Dein email ist falsch !!");
                    $('label[for="email"]').addClass("text-danger font-weight-bold");
                    $("#statusMsg").html("Email wurde nicht gesendet");
                }
            }
        });
    } else {
        return;
    }
}

initMap();



new WOW().init();
