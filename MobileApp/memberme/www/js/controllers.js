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
    $scope.toggle=false; 
    $scope.listCanSwipe = true; //profile list is swipeable
    var profileData=window.localStorage.getItem('profile'); //get profile data from local storage
    $scope.qrcode=profileData; //qrcode text is the object 
    profileData=JSON.parse(profileData); //parse JSON object
    $scope.profile=profileData; //save JSON object to $scope variable

    //edit function - triggered when user hits edit on profile items
    $scope.edit = function() {
      $ionicListDelegate.closeOptionButtons(); //closes the slide effect
      $scope.toggle=!$scope.toggle; //turns toggle on/off
    }
    //cancel method on profile edit
    $scope.cancel = function(){
      $scope.toggle=!$scope.toggle; //hide input box
      console.log($scope.profile.name);
    }
    //submit function for profile edits
    $scope.submit = function() {
      $scope.toggle=!$scope.toggle; //hide input box
      AuthService.updateProfile($scope.profile); //call AuthService method updateProfile and pass $scope.profile object
      profileData=$scope.profile; //save $scope obj to profileData obj
      $scope.qrcode=JSON.stringify(profileData); //set qr text to updated profile details
    }
  
  //func when user is logging out

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



.controller('SosCtrl', function($scope, AuthService, $state) {
  
 
});//end SosCtrl
 
