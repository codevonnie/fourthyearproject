angular.module('starter.controllers', [])
 
.controller('LoginCtrl', function($scope, AuthService, $ionicPopup, $state) {
  
  //Add in windows loading while checking if user is logged in!



  $scope.user={};
  var logInDetails = window.localStorage.getItem('signIn');
  
  if(logInDetails==undefined){
    //user object to take inputted email and password from login view.  Type automatically set to person
    $scope.user = {
      email: '',
      password: '',
      type: 'person'
    };
  }

  else{
    logInDetails = JSON.parse(logInDetails);
    console.log(logInDetails);
     var user = {
      email: logInDetails.email,
      password: logInDetails.password,
      type: 'person'
    };
    AuthService.getInfo(user);
       $state.go('tab.profile');
     
  }
  
  
  

$scope.login = function(user) {
    //if login is successful, go to profile view
    var onSuccess = function () {
       $state.go('tab.profile');
       

    };
    //if login is not successful, alert user with popup and allow them to reenter deails
    var onError = function () {
      var alertPopup = $ionicPopup.alert({
          title: 'Login failed!',
          template: "Please try again"
          });
    };

    //call AuthService method login to get token
    AuthService.login().then(function() {
      //call AuthService method getInfo to get user profile details
      AuthService.getInfo(user).then(onSuccess, onError);
        
        });
    }
  })//end LoginCtrl

 //Profile view controller
.controller('ProfileCtrl', function($scope, AuthService, API_ENDPOINT, $http, $state, $ionicListDelegate) {
  
    $scope.profile={}; //empty profile object
    $scope.origProfile={};
    $scope.toggle=false; 
    $scope.listCanSwipe = true; //profile list is swipeable
    var profileData=window.localStorage.getItem('profile'); //get profile data from local storage
    $scope.qrcode=profileData; //qrcode text is the object 
    profileData=JSON.parse(profileData); //parse JSON object
    $scope.profile=profileData; //save JSON object to $scope variable
    $scope.origProfile=profileData;
    var joined = new Date(parseInt(profileData.joined)).toDateString();
    var dob = new Date(parseInt(profileData.dob)).toDateString();
    console.log(joined);
    $scope.joined = joined;
    $scope.dob = dob;

    //edit function - triggered when user hits edit on profile items
    $scope.edit = function() {
      $ionicListDelegate.closeOptionButtons(); //closes the slide effect
      $scope.toggle=!$scope.toggle; //turns toggle on/off
    }
    //cancel method on profile edit
    $scope.cancel = function(){
      
      $scope.toggle=!$scope.toggle; //hide input box
      profileData=window.localStorage.getItem('profile');
      profileData=JSON.parse(profileData); //parse JSON object
      $scope.profile=profileData; //save JSON object to $scope variable
    }
    //submit function for profile edits
    $scope.submit = function() {
      $scope.toggle=!$scope.toggle; //hide input box
      AuthService.updateProfile($scope.profile); //call AuthService method updateProfile and pass $scope.profile object
      profileData=$scope.profile; //save updated $scope obj to profileData obj
      window.localStorage.setItem('profile', JSON.stringify(profileData)); //set updated profile details to local storage
      $scope.qrcode=JSON.stringify(profileData); //set qr text to updated profile details
    }

  //calls AuthService method getInfo to authorize user on db and retrieve profile details
  $scope.getInfo = function() {
    AuthService.getInfo();

  };
   
})//end ProfileCtrl

.controller('SettingsCtrl', function($scope, AuthService, $state) {
  
  //logout function
  $scope.logout = function() {
    
    AuthService.logout(); //call AuthService method logout
    $state.go('login'); //go to login view

  };
})//SettingsCtrl


//Controller for user if they are having an issue while on the business premises
.controller('SosCtrl', function($scope, AuthService, $http, $state, $ionicLoading, $cordovaGeolocation, $ionicPopup) {

  //empty object to take sos details for sending to business
  $scope.sos={};

  //submit function for sos message
  $scope.submit=function(){
    //get user profile details from local storage
    var sosDetails = window.localStorage.getItem('profile');
    sosDetails = JSON.parse(sosDetails); //parse object
    $scope.sos.email = sosDetails.email; //save person email to sos object
    var sosDetails = window.localStorage.getItem('latLng'); //get user lat and long (set below)
    sosDetails = JSON.parse(sosDetails); //parse object
    //save user lat and long coordinates to sos object
    $scope.sos.lat = sosDetails.lat; 
    $scope.sos.lng = sosDetails.lng;
    console.log($scope.sos);

    //popup if message is sent successfully
    // var alertPopup = $ionicPopup.alert({
    //       title: 'Could not get location',
    //       template: "Check your location services"
    //       });


  }
  //Google maps options
  var options = {timeout: 10000, enableHighAccuracy: true};

  //use phone GPS to get user location coordinates
  $cordovaGeolocation.getCurrentPosition(options).then(function(position){
    
    var latLng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
    //save latitude and longitude to local storage
    window.localStorage.setItem('latLng', JSON.stringify(latLng));
    //map options - centre map using lat long coords, zoom into location, use ROADMAP
    var mapOptions = {
      center: latLng,
      zoom: 15,
      mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    //map shown on sos view
    $scope.map = new google.maps.Map(document.getElementById("map"), mapOptions);

    //Wait until the map is loaded to set marker on map
    google.maps.event.addListenerOnce($scope.map, 'idle', function(){

      var marker = new google.maps.Marker({
          map: $scope.map,
          animation: google.maps.Animation.DROP,
          position: latLng
      });      
      //message displayed if user clicks on marker on map
      var infoWindow = new google.maps.InfoWindow({
          content: "You are here"
      });
      //listen for user clicking on marker - open info window
      google.maps.event.addListener(marker, 'click', function () {
          infoWindow.open($scope.map, marker);
      });

    });

  }, function(error){
    var alertPopup = $ionicPopup.alert({
          title: 'Could not get location',
          template: "Check your location services"
          });
  });  
});//end SosCtrl
 
