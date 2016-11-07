angular.module('starter.controllers', [])

.controller('LoginCtrl', function($scope, AuthService, $ionicPopup, $state) {

  $scope.user = {
    email: '',
    password: ''
  };

  $scope.login = function() {
    AuthService.login($scope.user).then(function(msg) {
      $state.go('tab.profile');
    }, function(errMsg) {
      var alertPopup = $ionicPopup.alert({
        title: 'Login failed!',
        template: errMsg
      });
    });
  };

});
