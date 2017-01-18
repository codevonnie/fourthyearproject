angular.module('starter.services', [])
 
.service('AuthService', function($q, $http, API_ENDPOINT) {
  var LOCAL_TOKEN_KEY = 'yourTokenKey';
  var isAuthenticated = false;
  var authToken;

  var options = { method: 'POST',
  url: 'https://membermeauth.eu.auth0.com/oauth/token',
  headers: { 'Content-Type': 'application/json' },
  data: '{"client_id":"fXqMFIGFPGXAPLNm6ltd0NsGV6fWpvDM","client_secret":"HHnBRmKTpK99fx4RYIVnxiJFQourT1RkbWnrs0jIUP1vdYrgWZ1104Tew7cb5-wp","audience":"https://restapicust.herokuapp.com/api/","grant_type":"client_credentials"}' };

 
  function loadUserCredentials() {
    var token = window.localStorage.getItem(LOCAL_TOKEN_KEY);
    if (token) {
      useCredentials(token);
    }
  }

  function storeUserCredentials(token) {
    window.localStorage.setItem(LOCAL_TOKEN_KEY, token);
    useCredentials(token);

  }
 
  function useCredentials(token) {
    isAuthenticated = true;
    authToken = token;
    
 
    // Set the token as header for your requests!
    $http.defaults.headers.common.Authorization = 'Bearer ' + authToken;
  }

  function destroyUserCredentials() {
    authToken = undefined;
    isAuthenticated = false;
    $http.defaults.headers.common.Authorization = undefined;
    window.localStorage.removeItem(LOCAL_TOKEN_KEY);
  }
   
 
  var login = function(){
    return $q(function(resolve, reject) {
      $http(options).then(function(result) {
        if (result.statusText=="OK") {
          storeUserCredentials(result.data.access_token);
          resolve(result.data.msg);
        } else {
          reject(result.data.msg);
          console.log("Something went wrong");
        }
      });
    })
  }
  
 
  var logout = function() {
    console.log("logout service");
    destroyUserCredentials();
  };

  var getInfo = function(user) {
       return $q(function(resolve, reject) {
         $http.post(API_ENDPOINT.url + '/authenticate', user).then(function(result) {
           if (result.data.success) {
             resolve(result.data.msg);
             //window.localStorage.setItem('profile', result.data);
             window.localStorage.setItem('profile', JSON.stringify(result.data.message));
           } else {
             reject(result.data.success);
             console.log("getMembers failed" + result.data.success);
           }
         });
       });
     };
 
  loadUserCredentials();
 
  return {
    login: login,
    options: options,
    logout: logout,
    getInfo: getInfo,
    isAuthenticated: function() {return isAuthenticated;},
  };
})
 
// .factory('AuthInterceptor', function ($rootScope, $q, AUTH_EVENTS) {
//   return {
//     responseError: function (response) {
//       $rootScope.$broadcast({
//         401: AUTH_EVENTS.notAuthenticated,
//       }[response.status], response);
//       return $q.reject(response);
//     }
//   };
// })
 
// .config(function ($httpProvider) {
//   $httpProvider.interceptors.push('AuthInterceptor');
// });