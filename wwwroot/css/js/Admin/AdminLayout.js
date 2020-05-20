var Chargers = $("#ChargerPicker").children();
var Blei = new Array();
var Lithium = new Array();
for (var i = 0; i < Chargers.length; i++) {
    if (Chargers[i].title.includes("Blei") == true) {
        Blei.push({
            Charger: Chargers[i]
        });
    } else {
        Lithium.push({
            Charger: Chargers[i]
        });
    }
}

var pageURL = window.location.origin;
if (pageURL.includes("localhost") == true) {
    pageURL = window.location.pathname;
    var index = pageURL.indexOf("ke") + 2;
    pageURL = pageURL.substring(0, index);
}


function GetArray(array, selectId) {
    let Ids = new Array();
    array.forEach(function (element) {
        Ids.push({
            Id: $(selectId).find('[value="' + element + '"]').attr("name")
        });
    });
    return Ids;
}

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
        if (group.length == 2) {

            let a = group.length;
            sessionStorage.setItem('group', group);
        } else {
            let GroupName = group[0].Name;
            if (sessionGR != GroupName || group == undefined) {
                $('#ChargerPicker').selectpicker('val', 'default');
                ChargerValue = "0";
                ChargerName = "";
            }
            if (GroupName.includes("Blei") == true) {
                for (var i = 0; i < Chargers.length; i++) {
                    if (Chargers[i].title.includes("Blei") != true) {
                        $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').hide();
                        $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').prop('disabled', true);
                    }
                    if (Chargers[i].title.includes("Blei") == true) {
                        $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').show();
                        $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').prop('disabled', false);
                    }
                }
            } else {
                for (var i = 0; i < Chargers.length; i++) {
                    if (Chargers[i].title.includes("Blei") == true) {
                        $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').hide();
                        $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').prop('disabled', true);
                    }
                    if (Chargers[i].title.includes("Blei") != true) {
                        $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').show();
                        $('#ChargerPicker').find('[id="' + Chargers[i].id + '"]').prop('disabled', false);
                    }
                }
            }
            sessionStorage.setItem('group', GroupName);
        }
        sessionStorage.setItem('group', group);
        $('#ChargerPicker').selectpicker('refresh');
    }
}

$(document).ready(function () {

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
        var Labels = new Array();
        var group = opt.parent();
        group.each(function () {
            Labels.push({
                Name: $(this).attr("label")
            });
        });

        ShowChargers(Labels);
    });

    var css = "url('" + pageURL + "/images/car-24px.svg')";
    $("#eTrike").css("background-image", css);

    var parent = $("#eTrike").parent().parent();

    if (parent.hasClass("active") == true) {
        css = "url('" + pageURL + "/images/carWhite-24px.svg')";
        $("#eTrike").css("background-image", css);
    }

    $("#AddNewModel").click(() => {
        $('#NewEditModal').modal("show");
    });


    $(".Available").change(function () {
        let id = $(this).attr("value");
        let url = pageURL + "/api/eTrikes/" + id;
        let value = $(this).prop("checked");
        $.ajax({
            url: url,
            type: "PUT",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            data: JSON.stringify(value),
            success: function (result) {

            }
        });
    });

    $(".DeleteButtons").click(function () {
        let id = $(this).attr("id");
        let name = $(this).parent().parent().find("td.w-25").text();
        name = name.trim();

        let txt = "Willst du wirklich löschen ";
        let input = "<input id='eId' value='" + id + "' hidden>";
        $("#ModalBody").html("<p class='BiggerFont'>" + txt + name + " ?</p>" + input);
        $('#DeleteModal').modal("show");

    });
    $("#DeleteButton").click(function () {
        let id = $("#eId").val();
        let url = pageURL + "/api/eTrikes/" + id;
        let answer = "";
        $.ajax({
            url: url,
            type: "DELETE",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            //data: Json,
            success: function (result) {
                answer = result
            }
        });
        if (answer == "") {
            $('#DeleteModal').modal("hide");
            let button = $("#" + id);
            button.parent().parent().remove();
        } else {
            $("#ModalBody").html("<p class='BiggerFont text-danger'>" + result + "</p>");
        }
    });

    $("#UpdateTrike").click(function () {

        let url = pageURL + "/api/eTrikes";

        let CatValue = $('#CatPicker').selectpicker('val');
        let IdCategory = $('#CatPicker').find('[value="' + CatValue + '"]').attr("name");

        let ColorsVal = $("#ColorPicker").selectpicker('val');
        let BattVal = $("#BattPicker").selectpicker('val');
        let ChargerVal = $("#ChargerPicker").selectpicker('val');

        const object = {
            Name: $("#Name").val(),
            Description: $("#Beschreibung").val(),
            Price: $("#Preis").val(),
            DesignType: $("#Bauart").val(),
            Dimensions: $("#Maße").val(),
            Weight: $("#Gewicht").val(),
            Payload: $("#Zuladung").val(),
            Power: $("#Leistung").val(),
            MaxSpeed: $("#Geschwindigkeit").val(),
            Seats: $("#Sitze").val(),
            WheelsFront: $("#ReifengrößeFront").val(),
            WheelsRear: $("#ReifengrößeHinten").val(),
            DrivingLicense: $("#Führerscheinklasse").val(),
            Available: $("#Verfügbar").prop("checked"),
            IdCategory: IdCategory,
            ColorsArray: GetArray(ColorsVal, "#ColorPicker"),
            BatteriesArray: GetArray(BattVal, "#BattPicker"),
            ChargersArray: GetArray(ChargerVal, "#ChargerPicker"),
        }
    });

    $("#AddeTrike").click(function () {

        let url = pageURL + "/api/eTrikes";

        let CatValue = $('#CatPicker').selectpicker('val');
        let IdCategory = $('#CatPicker').find('[value="' + CatValue + '"]').attr("name");

        let ColorsVal = $("#ColorPicker").selectpicker('val');
        let BattVal = $("#BattPicker").selectpicker('val');
        let ChargerVal = $("#ChargerPicker").selectpicker('val');

        const object = {
            Name: $("#Name").val(),
            Description: $("#Beschreibung").val(),
            Price: $("#Preis").val(),
            DesignType: $("#Bauart").val(),
            Dimensions: $("#Maße").val(),
            Weight: $("#Gewicht").val(),
            Payload: $("#Zuladung").val(),
            Power: $("#Leistung").val(),
            MaxSpeed: $("#Geschwindigkeit").val(),
            Seats: $("#Sitze").val(),
            WheelsFront: $("#ReifengrößeFront").val(),
            WheelsRear: $("#ReifengrößeHinten").val(),
            DrivingLicense: $("#Führerscheinklasse").val(),
            Available: $("#Verfügbar").prop("checked"),
            IdCategory: IdCategory,
            ColorsArray: GetArray(ColorsVal, "#ColorPicker"),
            BatteriesArray: GetArray(BattVal, "#BattPicker"),
            ChargersArray: GetArray(ChargerVal, "#ChargerPicker"),
        }
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            data: JSON.stringify(object),
            success: function (result) {
                if (result == true) {
                    $(".result").html(result.toString())
                } else {
                    $(".result").html(result.toString())
                }
                $('#DeleteModal').modal("hide");
            }
        });
    });

    $sidebar = $('.sidebar');

    $sidebar_img_container = $sidebar.find('.sidebar-background');

    $full_page = $('.full-page');

    $sidebar_responsive = $('body > .navbar-collapse');

    window_width = $(window).width();

    fixed_plugin_open = $('.sidebar .sidebar-wrapper .nav li.active a p').html();

    if (window_width > 767 && fixed_plugin_open == 'Dashboard') {
        if ($('.fixed-plugin .dropdown').hasClass('show-dropdown')) {
            $('.fixed-plugin .dropdown').addClass('open');
        }

    }

    $('.fixed-plugin a').click(function (event) {
        // Alex if we click on switch, stop propagation of the event, so the dropdown will not be hide, otherwise we set the  section active
        if ($(this).hasClass('switch-trigger')) {
            if (event.stopPropagation) {
                event.stopPropagation();
            } else if (window.event) {
                window.event.cancelBubble = true;
            }
        }
    });

    $('.fixed-plugin .active-color span').click(function () {
        $full_page_background = $('.full-page-background');

        $(this).siblings().removeClass('active');
        $(this).addClass('active');

        var new_color = $(this).data('color');

        if ($sidebar.length != 0) {
            $sidebar.attr('data-color', new_color);
        }

        if ($full_page.length != 0) {
            $full_page.attr('filter-color', new_color);
        }

        if ($sidebar_responsive.length != 0) {
            $sidebar_responsive.attr('data-color', new_color);
        }
    });

    $('.fixed-plugin .background-color .badge').click(function () {
        $(this).siblings().removeClass('active');
        $(this).addClass('active');

        var new_color = $(this).data('background-color');

        if ($sidebar.length != 0) {
            $sidebar.attr('data-background-color', new_color);
        }
    });

    $('.fixed-plugin .img-holder').click(function () {
        $full_page_background = $('.full-page-background');

        $(this).parent('li').siblings().removeClass('active');
        $(this).parent('li').addClass('active');


        var new_image = $(this).find("img").attr('src');

        if ($sidebar_img_container.length != 0 && $('.switch-sidebar-image input:checked').length != 0) {
            $sidebar_img_container.fadeOut('fast', function () {
                $sidebar_img_container.css('background-image', 'url("' + new_image + '")');
                $sidebar_img_container.fadeIn('fast');
            });
        }

        if ($full_page_background.length != 0 && $('.switch-sidebar-image input:checked').length != 0) {
            var new_image_full_page = $('.fixed-plugin li.active .img-holder').find('img').data('src');

            $full_page_background.fadeOut('fast', function () {
                $full_page_background.css('background-image', 'url("' + new_image_full_page + '")');
                $full_page_background.fadeIn('fast');
            });
        }

        if ($('.switch-sidebar-image input:checked').length == 0) {
            var new_image = $('.fixed-plugin li.active .img-holder').find("img").attr('src');
            var new_image_full_page = $('.fixed-plugin li.active .img-holder').find('img').data('src');

            $sidebar_img_container.css('background-image', 'url("' + new_image + '")');
            $full_page_background.css('background-image', 'url("' + new_image_full_page + '")');
        }

        if ($sidebar_responsive.length != 0) {
            $sidebar_responsive.css('background-image', 'url("' + new_image + '")');
        }
    });

    $('.switch-sidebar-image input').change(function () {
        $full_page_background = $('.full-page-background');

        $input = $(this);

        if ($input.is(':checked')) {
            if ($sidebar_img_container.length != 0) {
                $sidebar_img_container.fadeIn('fast');
                $sidebar.attr('data-image', '#');
            }

            if ($full_page_background.length != 0) {
                $full_page_background.fadeIn('fast');
                $full_page.attr('data-image', '#');
            }

            background_image = true;
        } else {
            if ($sidebar_img_container.length != 0) {
                $sidebar.removeAttr('data-image');
                $sidebar_img_container.fadeOut('fast');
            }

            if ($full_page_background.length != 0) {
                $full_page.removeAttr('data-image', '#');
                $full_page_background.fadeOut('fast');
            }

            background_image = false;
        }
    });

    $('.switch-sidebar-mini input').change(function () {
        $body = $('body');

        $input = $(this);

        if (md.misc.sidebar_mini_active == true) {
            $('body').removeClass('sidebar-mini');
            md.misc.sidebar_mini_active = false;

            $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar();

        } else {

            $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar('destroy');

            setTimeout(function () {
                $('body').addClass('sidebar-mini');

                md.misc.sidebar_mini_active = true;
            }, 300);
        }

        // we simulate the window Resize so the charts will get updated in realtime.
        var simulateWindowResize = setInterval(function () {
            window.dispatchEvent(new Event('resize'));
        }, 180);

        // we stop the simulation of Window Resize after the animations are completed
        setTimeout(function () {
            clearInterval(simulateWindowResize);
        }, 1000);

    });
});
