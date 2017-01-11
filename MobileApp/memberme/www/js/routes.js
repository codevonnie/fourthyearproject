angular.module('starter.routes', ['ionic'])

.config(function($stateProvider, $urlRouterProvider) {

  $stateProvider

  // setup an abstract state for the tabs directive
    .state('tab', {
    url: '/tab',
    abstract: true,
    templateUrl: 'templates/tabs.html'
  })

  // Each tab has its own nav history stack:

  .state('tab.profile', {
    url: '/profile',
    views: {
      'profile': {
        //controller: 'ProfileCtrl',
        templateUrl: 'templates/profile.html'
        //controller: 'ProfileCtrl'
      }
    }
  })

  
  .state('tab.sos', {
    url: '/sos',
    views: {
      'sos': {
        templateUrl: 'templates/sos.html',
        controller: 'SosCtrl'
      }
    }
  })
  
  .state('login', {
    url: '/login',
    templateUrl: 'templates/login.html',
    controller: 'LoginCtrl'
    });

  // if none of the above states are matched, use this as the fallback
  //$urlRouterProvider.otherwise('/tab/profile');
  $urlRouterProvider.otherwise('/profile');

});