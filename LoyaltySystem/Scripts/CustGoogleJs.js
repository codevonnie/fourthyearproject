
var map;
var lonitude = 12.556663;
var latitude = 48.1293954;
var markerMessage;
var markerArray = [];
var iconBase = 'https://maps.gstatic.com/mapfiles/ms2/micons/';

function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: new google.maps.LatLng(latitude, lonitude),//Setting Initial Position
        zoom: 10
    });
}

function newLocation(newLat, newLng) {
    map.setCenter({
        lat: newLat,
        lng: newLng
    });
}

function newPin(newLat, newLng, message) {
    var uluru = { lat: newLat, lng: newLng };

    var marker = new google.maps.Marker({
        position: uluru,
        map: map,
        icon: iconBase + 'red-dot.png',
        Animation: google.maps.Animation.DROP,
    });

    //Creates a Click Event on the Marker and displays a message to the Window
    var infowindow = new google.maps.InfoWindow({
        content: message
    });

    //CLICK EVENT
    google.maps.event.addListener(marker, 'click', function () {
        //Changes the Color of the Icon once Clicked to show message as Viewed
        markerMessage = marker;
        marker.setIcon(iconBase + "green-dot.png");

        // Calling the open method of the infoWindow        
        infowindow.open(map, marker);
    });

    markerArray.push(marker);
}


//Ajax Function That Calls the C# method DeleteMessage()
//Also incharge of updating the MessageCounter
function RemoveMessage() {

    //console.log(JSON.stringify(markerMessage.position));
    $.ajax({
        type: "POST",
        url: "Map.aspx/DeleteMessage",
        data: "{data:" + JSON.stringify(markerMessage.position) + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        failure: function (response) {
            alert(response.d);
        }
    });

    //Delete The Marker From The Map
    markerMessage.setMap(null);

    //Set the InnerText to the current-1
    var num = Number(document.getElementById("messageBoxCount").innerText);
    num--;
    localStorage['messageCount'] = num.toString();
    document.getElementById("messageBoxCount").innerText = num.toString();
}


/*Functions below get the users Current Location, and alerts them if it cant get it maby just display message instead of Alert? ********
*/
var x = document.getElementById("demo");
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition, showError);
    }
    else { alert("Geolocation is not supported by this browser."); }
}

function showPosition(position) {
    latitude = position.coords.latitude;
    lonitude = position.coords.longitude;
    newLocation(latitude, lonitude);
}

function showError(error) {
    if (error.code === 1) {
        alert("User denied the request for Geolocation.");
    }
    else if (err.code === 2) {
        alert("Location information is unavailable.");
    }
    else if (err.code === 3) {
        alert("The request to get user location timed out.");
    }
    else {
        alert("An unknown error occurred.");
        x.innerHTML = "An unknown error occurred.";
    }
}

