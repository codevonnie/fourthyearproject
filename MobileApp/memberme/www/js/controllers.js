angular.module('starter.controllers', [])
 
.controller('LoginCtrl', function($scope, AuthService, $ionicPopup, $state) {
  $scope.user = {
    email: '',
    password: '',
    type: 'person'
  };
 
$scope.login = function(user) {
  
    var onSuccess = function () {
       $state.go('tab.profile');

    };

    var onError = function () {
      var alertPopup = $ionicPopup.alert({
          title: 'Login failed!',
          template: "Please try again"
          });
    };


    AuthService.login().then(function() {
      AuthService.getInfo(user).then(onSuccess, onError);
        //$state.go('tab.profile');
        });
    }
  })

 
.controller('ProfileCtrl', function($scope, AuthService, API_ENDPOINT, $http, $state, $ionicListDelegate) {
  
    $scope.profile={};
    $scope.toggle=false;
    $scope.listCanSwipe = true;
    $scope.qrcode=window.localStorage.getItem('profile');
    var profileData=window.localStorage.getItem('profile');
    profileData=JSON.parse(profileData);
    $scope.profile=profileData;

    $scope.edit = function() {
      $ionicListDelegate.closeOptionButtons();
      $scope.toggle=!$scope.toggle;
      console.log($scope.profile.name);
    }

    $scope.submit = function() {
      $scope.toggle=!$scope.toggle;
      AuthService.updateProfile($scope.profile);
    }
      
  $scope.destroySession = function() {
    console.log("destroy session");
    AuthService.logout();
  };
 
  $scope.getInfo = function() {
    AuthService.getInfo();
    
    
  };
   
})

.controller('SettingsCtrl', function($scope, AuthService, $state) {
  
  $scope.logout = function() {
    console.log("enter logout");
    AuthService.logout();
    $state.go('login');
    console.log("exit logout");
  };
})

.controller('SosCtrl', function($scope, AuthService, $state) {
  
 
})
 
.controller('AppCtrl', function($scope, $state, $ionicPopup, AuthService, AUTH_EVENTS) {
  $scope.$on(AUTH_EVENTS.notAuthenticated, function(event) {
    AuthService.logout();
    $state.go('login');
    var alertPopup = $ionicPopup.alert({
      title: 'Session Lost!',
      template: 'Sorry, You have to login again.'
    });
  });
});