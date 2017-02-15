
var map;
var lonitude = 12.556663;
var latitude = 48.1293954;
var markerMessage;
var markerArray = [];
var MessageObjects;
var iconBase = 'https://maps.gstatic.com/mapfiles/ms2/micons/';
var LatClick;
var LongClick;

var script = document.createElement('script');
script.type = 'text/javascript';
script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAaJMX4i5TEKA3UL5I7DArt42MRSxqg4LI&callback=initMap';
document.body.appendChild(script);

//Init Function with Start up code 
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: new google.maps.LatLng(latitude, lonitude),//Setting Initial Position
        zoom: 10
    });

    getLocation();

    if (localStorage) {
        MessageObjects = JSON.parse(localStorage.getItem("MessageObjects"));

        if (MessageObjects != null)
            CreatePins(MessageObjects);
    }
    else {
        alert("Your Browser Does Not Support This Feature")
    }

}

//Sets a new Location on the Map
function newLocation(newLat, newLng) {
    map.setCenter({
        lat: newLat,
        lng: newLng
    });
}

//Create a new Marker
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
        LatClick = this.position.lat();
        LongClick = this.position.lng();

        LatClick = parseFloat(LatClick).toFixed(6); //*************************** FIX IF NEEDED
        LongClick = parseFloat(LongClick).toFixed(6);

        // Calling the open method of the infoWindow        
        infowindow.open(map, marker);
    });

    markerArray.push(marker);
}

//Creates the Markers + the messages
function CreatePins(MessageObjects) {

    for (var key in MessageObjects) {
        if (MessageObjects.hasOwnProperty(key)) {

            var pin = MessageObjects[key];
            var latitude = parseFloat(pin.latitude);
            var longitude = parseFloat(pin.longitude);

            newLocation(latitude, longitude);

            var PinMessage = "'" +
            "<div class=\"MessageBoxMaxWidth text-capitalize\">" +
            "<h3 class=\"text-center\">" + pin.name + "</h3>" +
            "<p><br/><b>Email</b>: " + pin.email + "</p>" +
            "<p><br/><b>Status</b>: " + pin.status + "</p>" +
            "<p><br/><b>Message</b>: " + pin.message + "</p>" +
            "<button class=\"btn btn-danger btn-block\" type=\"button\" onclick=\"RemoveMessage()\">Delete Pin And Message</button>" +
            "</div >" +
            "'";

            newPin(latitude, longitude, PinMessage);
        }
    }
}


//Removes the Markers from the map and local storage
function RemoveMessage() {
    //Delete The Marker From The Map
    markerMessage.setMap(null);
    var TempList = markerMessage;
    var num = document.getElementById("messageBoxCount").innerText;

    //Loop over all objects in the array
    for (var key in MessageObjects) {
        if (MessageObjects.hasOwnProperty(key)) {
            var pin = MessageObjects[key];

            //Remove The Pin Clicked From The Array
            if (pin.longitude === LongClick && pin.latitude === LatClick) {
                MessageObjects.splice(key, 1);

                num--;
                localStorage.setItem("MessageCount", num);
                document.getElementById("messageBoxCount").innerText = num;
                //Reset Local Storage with new Messages
                localStorage.setItem("MessageObjects", JSON.stringify(MessageObjects));
                return;
            }
        }
    }

}




/*Functions below get the users Current Location, and alerts them if it cant get it maby just display message instead of Alert?
*/

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

