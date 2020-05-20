//debugger;

var ColorValue = "";
var BattValue = "";
var ChargerValue = "";
var ChargerName = "";
var ColorSample;
var priceFormat;
var CurrencySign;
var BattName;
var Chargers = $("#ChargerPicker").children();

var pageURL = window.location.origin;
if (pageURL.includes("localhost") == true) {
    pageURL = window.location.pathname;
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
                var s = result.name;
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

$(document).ready(function () {
    $("#Confiq").removeClass("bgSizeP");
    $("#Configurator").click(function () {
        $("#Configurator").css("transition", "0.1s ease-in-out");
        $("#Configurator").css("visibility", "hidden");
        $("#Confiq").slideDown();
        $("#hrConfiq").slideDown();
    });

    $('.selectpicker').selectpicker();
    $('#ChargerPicker').selectpicker('val', 'default');
    $('#BattPicker').selectpicker('val', 'default');
    $('#ColorPicker').selectpicker('val', 'Weis');

    $("#PostCode").val("");

    $("#PCGeoLoc").click(function () {

        sessionStorage.removeItem('lat');
        sessionStorage.removeItem('lng');

        //\b\d{5}\b
        var regex = /\b\d{5}\b/g;

        var postCode = $("#PostCode").val();
        var lat;
        var lng;
        if ((postCode != 0 || postCode != "") && (regex.test(postCode))) {

            let urlHEREAPI = "https://places.sit.ls.hereapi.com/places/v1/discover/search?at=51.0847,10.4942";
            urlHEREAPI += "&q=" + postCode + "&apiKey=ptmF3VK80D3vpVx-YNiUfbALMuAgGyMPuaPeeM0C4bQ&addressFilter=countryCode%3DDEU%2C%26zipCode%3D" + postCode + "";
            urlHEREAPI += "&pretty&Accept-Language=de";

            $.ajax({
                async: false,
                url: urlHEREAPI,
                type: "GET",
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
                success: function (result) {

                    for (var i = 0; i < result.results.items.length; i++) {
                        lat = result.results.items[i].position[0];
                        lng = result.results.items[i].position[1];
                    }
                    if (lat != null && lng != null) {
                        sessionStorage.setItem('lat', lat);
                        sessionStorage.setItem('lng', lng);
                        $("#PostCode").val("");
                        let Url = pageURL.split("/");
                        let UrlService = "/" + Url[1] + "/#Service";
                        window.location.replace(UrlService);
                    } else {
                        $('label[for="PostCode"]').html("Dein Postleitzahl ist falsch !!");
                        $('label[for="PostCode"]').addClass("text-danger font-weight-bold");
                    }
                },
                error: function (errormessage) {
                    alert(errormessage.responseText);
                }
            });
        } else {
            return;
        }
    });

    var txtMsg = "";
    $("#SendQuestion").click(function () {
        $('label[for="subject"]').addClass("active");
        $('label[for="message"]').addClass("active");

        $("#subject").val($("#eName").text());
        var txt = "Model: " + $("#eName").text().trim() + " \n";
        txt += "Farbe: " + ColorValue.trim() + " \n";
        txt += "Batterie: " + BattName.trim() + " \n";
        ChargerName = ChargerName.replace(/(\r\n|\n|\r)/gm, "");
        txt += "Ladegeräte: " + ChargerName.trim() + " \n";
        txt += "Preis: " + priceFormat + CurrencySign + " \n";
        txtMsg = txt + "\n"

        $("#message").val(txtMsg + $("#name").val());

        $('#SendModal').modal('show');

    });
    $('[data-id="ColorPicker"]').click(function () {
        $(".badge").each(function (i) {
            CheckColor(this);
        });
    });

    $('#ColorPicker').on('loaded.bs.select', function () {
        var span = $(".filter-option-inner-inner").children();
        span.each(function (i) {
            ColorSample = this;
            CheckColor(this);
        });
        ColorValue = $('#ColorPicker').selectpicker('val');

        ShowResult();
    });

    $('#ColorPicker').on('hide.bs.select', function () {
        var span = $(".filter-option-inner-inner").children();
        span.each(function (i) {
            ColorSample = this;
            CheckColor(this);
        });
        ColorValue = $('#ColorPicker').selectpicker('val');

        ShowResult();
    });

    $('#BattPicker').on('hidden.bs.select', function () {
        var opt = $(this).find(':selected');
        var sel = opt.text();
        var og = opt.closest('optgroup').attr('label');

        BattValue = $('#BattPicker').selectpicker('val');

        BattName = $('#BattPicker').find('[value="' + BattValue + '"]').text();

        if (og != undefined) {
            $('[data-id="BattPicker"]').find(".filter-option-inner-inner").html(og + ' - ' + sel);
            BattName = og + " " + BattName;
        }
        var group = opt.parent().attr("label");

        ShowChargers(group);
        ShowResult();
    });

    $('#ChargerPicker').on('hidden.bs.select', function () {
        ChargerValue = $('#ChargerPicker').selectpicker('val');
        var opt = $(this).find(':selected');
        if (opt.length != 0) {
            if (opt.text() == "Ohne") {
                ChargerName = opt.text();
            } else {
                ChargerName = opt[0].title;
            }
        }

        ShowResult();
    });
});

MinView();
$(window).resize(function () {
    MinView();
});

$(".thumbs").click(function () {

    var id = $(this).attr("id");
    id = id.substring(3);
    id = parseInt(id);

    if (window.screen.width > 1000 && document.body.clientWidth > 1000) {

        var Imgs = $(".CarouselMid").children();
        var contentCarousel = "";
        var CarouselIndicators = "";
        $("#CarouselIndicators").empty();
        $("#CarouselInner").empty();
        for (var i = 0; i < Imgs.length; i++) {
            var active = "active";
            if (i == (id - 1)) {
                active = "active";
            } else {
                active = "";
            }
            contentCarousel += "<div class='carousel-item " + active + "'><img class='d-block m-auto pl-5 pr-5 ModalImg' src='" + Imgs[i].src + "' alt = 'slide'></div>";
            CarouselIndicators += "<li data-target='#carousel-example-1z' data-slide-to='" + i + "' class='" + active + "'></li>";
        }
        $("#CarouselIndicators").html(CarouselIndicators);
        $("#CarouselInner").html(contentCarousel);

        $('#ModalPreview').modal('show');
    } else {
        $('.carousel').carousel(id - 1)
    }
});

Number.prototype.format = function (n, x, s, c) {
    var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
        num = this.toFixed(Math.max(0, ~~n));

    return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
};

function ShowChargers(group) {
    ChargerValue = "0";
    if ($('#BattPicker').selectpicker('val') == 0 || $('#BattPicker').selectpicker('val') == null) {
        $('#ChargerPicker').selectpicker('deselectAll');
        $('#ChargerPicker').selectpicker('val', 'default');
        $('#ChargerPicker').selectpicker('hide');
        $("#ChargerSelect").hide();
        ChargerValue = "0";
        ChargerName = "";
    } else {
        $('#ChargerPicker').selectpicker('show');
        $("#ChargerSelect").show();

        let sessionGR = sessionStorage.getItem('group');

        if (sessionGR != group || group == undefined) {
            $('#ChargerPicker').selectpicker('val', 'default');
            ChargerValue = "0";
            ChargerName = "";
        }
        if (group.includes("Blei") == true) {
            for (var i = 0; i < Chargers.length; i++) {
                if (Chargers[i].title.includes("Blei") != true) {
                    $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').hide();
                }
                if (Chargers[i].title.includes("Blei") == true) {
                    $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').show();
                }
            }
        } else {
            for (var i = 0; i < Chargers.length; i++) {
                if (Chargers[i].title.includes("Blei") == true) {
                    $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').hide();
                }
                if (Chargers[i].title.includes("Blei") != true) {
                    $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').show();
                }
            }
        }
        sessionStorage.setItem('group', group);
        $('#ChargerPicker').selectpicker('refresh');
    }
}

function ShowResult() {
    if (ColorValue != "" && (BattValue != "" && BattValue != null) && (ChargerValue != "" && ChargerValue != null)) {

        var price = $("#eTPrice").text();
        CurrencySign = price.substring(price.lastIndexOf(" "));

        price = price.substring(price.indexOf(" "), price.lastIndexOf(" ")).trim();
        price = price.replace(".", "");
        price = price.replace(",", ".");
        price = parseFloat(price);
        price = price + parseFloat(BattValue) + parseFloat(ChargerValue);

        priceFormat = price.format(2, 3, '.', ',');

        var badge = ColorSample.outerHTML;
        $("#BattVal").html(BattName);
        if (ChargerName == "") {
            ChargerName = "Ohne";
        }
        $("#ChargerVal").html(ChargerName);
        $("#ColorVal").html(ColorValue + " - " + badge);
        $("#PreisVal").html(priceFormat + CurrencySign);
        $("#subject").text(BattName + ChargerName + ColorValue + priceFormat + CurrencySign);
        // $("#ChargerSelect").show();

        $("#ConfiqResult").slideDown("slow");
        ChangeBGSize()
    } else {
        return;
    }
}

function ChangeBGSize() {
    let size = 150;
    for (var i = 0; i < 5000; i++) {
        size = parseFloat(size) - parseFloat(1 / 100);
        $("#Confiq").css("background-size", "100% " + size.toString() + "%");
    }
}

function CheckColor(element) {
    if ((element.style.backgroundColor == "rgb(255, 255, 255)") || (element.style.backgroundColor == "rgb(225, 255, 0)")) {
        $(element).addClass("blackFont");
    } else {
        element.style.color = "";
    }
}

function Rotate(element) {
    element = "#" + element;
    var all_Rest = $('.Arrow_Icon');
    all_Rest = all_Rest.not(element);

    if (all_Rest.hasClass('Rotate_Arrow')) {
        all_Rest.removeClass('Rotate_Arrow');
    }

    $(element).toggleClass('Rotate_Arrow');
}


function MinView() {

    var dt = $(".dtCR");
    var dd = $(".ddCR");
    let txt = $(".hyphenate").text();
    if (window.screen.width < 767 || document.body.clientWidth < 767) {
        $("#con1").removeClass("px-5");
        $("#con2").removeClass("px-4");
        //$("#con1").removeClass("container-fluid");
        //$("#con1").addClass("container");
        $("#DescCard").removeClass("card");
        $("#Confiq").removeClass("card");
        //$("#PTBody").removeClass("card-body");
        $("#ImgCard").removeClass("card");

        $("#DescCard").css("display", "flex");
        $("#DescCard").css("flex-direction", "column");

        $('#ModalPreview').modal('hide');
        //$("#2Buttons").removeClass("h-100");

        dt.each(function (i) {
            $(this).removeClass("col-6");
            $(this).addClass("col-5");
        });
        dd.each(function (i) {
            $(this).removeClass("col-6");
            $(this).addClass("col-7");
        });

        if (!txt.includes("-")) {
            txt = txt.splice(txt.lastIndexOf("s") + 1, 0, "-");
            $(".hyphenate").html(txt);
        }

    } else {
        $("#con1").addClass("px-5");
        $("#con2").addClass("px-4");
        //$("#con1").removeClass("container");
        //$("#con1").addClass("container-fluid");
        $("#DescCard").addClass("card");
        $("#Confiq").addClass("card");
        //$("#PTBody").addClass("card-body");
        $("#ImgCard").addClass("card");
        $("#productCarousel").css("max-height", "55vh !important");
        //$("#2Buttons").addClass("h-100");

        dt.each(function (i) {
            $(this).addClass("col-6");
            $(this).removeClass("col-5");
        });
        dd.each(function (i) {
            $(this).addClass("col-6");
            $(this).removeClass("col-7");
        });
        txt = txt.replace("-", "");
        $(".hyphenate").html(txt);
    }
}
