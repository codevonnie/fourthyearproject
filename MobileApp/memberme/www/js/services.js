angular.module('starter.services', [])
 
.service('AuthService', function($q, $http, API_ENDPOINT) {
  var LOCAL_TOKEN_KEY = 'yourTokenKey';
  var isAuthenticated = false;
  var authToken;
 
  function loadUserCredentials() {
    var token = window.localStorage.getItem(LOCAL_TOKEN_KEY);
    if (token) {
      useCredentials(token);
      console.log(token);
    }
  }
 
  function storeUserCredentials(token) {
    window.localStorage.setItem(LOCAL_TOKEN_KEY, token);
    useCredentials(token);
    console.log(token);
  }
 
  function useCredentials(token) {
    isAuthenticated = true;
    authToken = token;
    console.log(token);
 
    // Set the token as header for your requests!
    $http.defaults.headers.common.Authorization = 'JWT ' + authToken;
  }
 
  function destroyUserCredentials() {
    authToken = undefined;
    isAuthenticated = false;
    $http.defaults.headers.common.Authorization = undefined;
    window.localStorage.removeItem(LOCAL_TOKEN_KEY);
  }
   
  var login = function(user) {
    return $q(function(resolve, reject) {
      $http.post(API_ENDPOINT.url + '/authenticate', user).then(function(result) {
        if (result.data.success) {
          storeUserCredentials(result.data.token);
          resolve(result.data.msg);
        } else {
          reject(result.data.msg);
        }
      });
    });
  };
 
  var logout = function() {
    destroyUserCredentials();
  };

  var getInfo = function() {
       return $q(function(resolve, reject) {
         $http.get(API_ENDPOINT.url + '/memberinfo').then(function(result) {
         console.log("in members service");
         console.log(result);
           if (result.status.success) {

             window.localStorage.setItem('members', JSON.stringify(result.data));
             console.log(JSON.stringify(result.data));
           } else {
             reject(result.data.msg);
             console.log("getMembers failed")
           }
         });
       });
     };
 
  loadUserCredentials();
 
  return {
    login: login,
    logout: logout,
    getInfo: getInfo,
    isAuthenticated: function() {return isAuthenticated;},
  };
})
 
.factory('AuthInterceptor', function ($rootScope, $q, AUTH_EVENTS) {
  return {
    responseError: function (response) {
      $rootScope.$broadcast({
        401: AUTH_EVENTS.notAuthenticated,
      }[response.status], response);
      return $q.reject(response);
    }
  };
})
 
.config(function ($httpProvider) {
  $httpProvider.interceptors.push('AuthInterceptor');
});