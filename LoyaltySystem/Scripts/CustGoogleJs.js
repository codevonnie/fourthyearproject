
var map;
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: new google.maps.LatLng(48.1293954, 12.556663),//Setting Initial Position
        zoom: 15
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
        map: map
    });

    //Creates a Click Event on the Marker and displays a message to the Window
    var infowindow = new google.maps.InfoWindow({
        content: message
    });
    google.maps.event.addListener(marker, 'click', function () {
        // Calling the open method of the infoWindow 
        infowindow.open(map, marker);
    });
}

