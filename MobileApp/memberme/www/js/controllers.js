angular.module('starter.controllers', [])
 
.controller('LoginCtrl', function($scope, AuthService, $ionicPopup, $state) {
  //user object to take inputted email and password from login view.  Type automatically set to person
  $scope.user = {
    email: '',
    password: '',
    type: 'person'
  };
 
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



.controller('SosCtrl', function($scope, AuthService, $state, $ionicLoading) {
  
    google.maps.event.addDomListener(window, 'load', function() {
        var myLatlng = new google.maps.LatLng(37.3000, -120.4833);
 
        var mapOptions = {
            center: myLatlng,
            zoom: 16,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
 
        var map = new google.maps.Map(document.getElementById("map"), mapOptions);
 
        navigator.geolocation.getCurrentPosition(function(pos) {
            map.setCenter(new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude));
            var myLocation = new google.maps.Marker({
                position: new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude),
                map: map,
                title: "My Location"
            });
        });
 
        $scope.map = map;
    });
});//end SosCtrl
 
