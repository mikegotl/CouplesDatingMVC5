//Google Places Bar on Home Page


function RenderGPlacesHome(cityIn, qryIn, max) {
    $.ajax({
        type: "GET",
        url: "/Places/GetGPlacesJson",
        data: { city: cityIn, query: qryIn, limit: max },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function () {
                if (this.place_id == null || this.photoUrls[0] == undefined) return null;

                img = '<div class="col-md-3" style="height:400px;"> <div class="thumbnail"> '
                    + '<img style="max-height:200px;" id="' + this.photoUrls[0].place_id + '" src="' + this.photoUrls[0].photoUrl + '" alt="..."> '//Photo Img
                    + '<div class="caption">'
                    + '<h4>' + this.name + '</h4>'//Name of Place
                    + ' <p>' + this.formattedAddress + '</p>'//Address
                    //+ '<a href="">' + this.website_url + '</a>'//Website URL
                    + '<p><a href="/places/details/' + this.place_id + '" class="btn btn-primary" role="button">View Details</a></p>'//View Details Button
                    + ' </div></div></div>'

                $("#HomeRowGooglePlaces").append(img);
            });
        },
        error: function (response) {
            alert('error');
        }
    });
}

function RenderMostRecentCouples(max) {
    $.ajax({
        type: "GET",
        url: "/Members/GetMostRecentCouples",
        data: { max: max },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function () {
                thumbnail = '<div class="col-md-3" style="height:400px;"><div class="thumbnail">'
                    + '<img style="max-height:200px;" id="' + this.ID + '" src="' + this.ImgPath + '" alt="...">'//Photo
                    + '<div class="caption">'
                    + '<h3>' + this.ScreenName + '</h3>'//ScreenName
                    + '<h4>[' + this.FirstName1 + ' and ' + this.FirstName2 + ']</h3>'//Names
                    + '<p>Status: ' + this.Status + '</p> '//Couple's Status
                    + '<p>Met in ' + this.YearMet + '</p>'//Year Met
                    + '<p><a href="/members/details/' + this.ID + '"class="btn btn-primary" role="button">View Details</a></p>'//View Details Button
                    + ' </div> </div> </div>'

                $("#MostRecentCouples").append(thumbnail);
            });
        },
        error: function (response) {
            alert('error');
        }
    });
}

$('.datetimepicker').datetimepicker({
    startDate: '-0d',
    pickTime: true,
    sideBySide: true
});

$('.datetimepicker').click(function () {
    alert('clicked');
});

$('#myCarousel').carousel({
    interval: 10000
})

$('#carousel-example-generic').carousel({
    interval: 10000
})

$('.carousel .item').each(function () {
    var next = $(this).next();
    if (!next.length) {
        next = $(this).siblings(':first');
    }
    next.children(':first-child').clone().appendTo($(this));

    if (next.next().length > 0) {
        next.next().children(':first-child').clone().appendTo($(this));
    }
    else {
        $(this).siblings(':first').children(':first-child').clone().appendTo($(this));
    }
});

//age slider on home page
$("#ex2").slider({});
$("#ex2").on("slide", function (slideEvt) {
    $("#SliderVal1").text(slideEvt.value);
});

$("#ex3").slider({});
$("#ex3").on("slide", function (slideEvt) {
    $("#SliderVal2").text(slideEvt.value);
});

$("#ex4").slider({});
$("#ex4").on("slide", function (slideEvt) {
    $("#SliderVal3").text(slideEvt.value);
});

/*---------------------------------------*/
/*	GOOGLE MAP
/*---------------------------------------*/
//"use strict";
//set your google maps parameters

//var $latitude = 28.538335, //If you unable to find latitude and longitude of your address. Please visit http://www.latlong.net/convert-address-to-lat-long.html you can easily generate.
//    $longitude = -81.379236,
//    $map_zoom = 16; /* ZOOM SETTING */

////google map custom marker icon - .png fallback for IE11
//var is_internetExplorer11 = navigator.userAgent.toLowerCase().indexOf('trident') > -1;
//var $marker_url = (is_internetExplorer11) ? 'images/map-marker.png' : 'images/map-marker.svg';

////we define here the style of the map
//var style = [{
//    "stylers": [{
//        "hue": "#00aaff"
//    }, {
//        "saturation": -100
//    }, {
//        "gamma": 2.15
//    }, {
//        "lightness": 12
//    }]
//}];

////set google map options
//var map_options = {
//    center: new google.maps.LatLng($latitude, $longitude),
//    zoom: $map_zoom,
//    panControl: true,
//    zoomControl: true,
//    mapTypeControl: false,
//    streetViewControl: true,
//    mapTypeId: google.maps.MapTypeId.ROADMAP,
//    scrollwheel: false,
//    styles: style,
//}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

//modal for image previews
$('img').on('click', function () {
    var src = $(this).attr('src');
    var img = '<img src="' + src + '" class="img-responsive" />';
    $('#myModal').modal();
    $('#myModal').on('shown.bs.modal', function () {
        $('#myModal .modal-body').html(img);
    });
    $('#myModal').on('hidden.bs.modal', function () {
        $('#myModal .modal-body').html('');
    });
});

$(".partialContents").each(function (index, item) {
    var url = $(item).data("url");
    if (url && url.length > 0) {
        $(item).load(url);
    }
});