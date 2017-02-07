angular.module('starter.services', [])
 
 //AuthService - controls login, Authorization and logout
.service('AuthService', function($q, $http, API_ENDPOINT, MESSAGE_SERVER, jwtHelper) {
  
  //auth token variables
  var LOCAL_TOKEN_KEY = 'yourTokenKey'; 
  var isAuthenticated = false;
  var authToken;

  //POST method for getting Auth0 token - used in login function
  var options = { method: 'POST',
  url: 'https://membermeauth.eu.auth0.com/oauth/token',
  headers: { 'Content-Type': 'application/json' },
  data: '{"client_id":"fXqMFIGFPGXAPLNm6ltd0NsGV6fWpvDM","client_secret":"HHnBRmKTpK99fx4RYIVnxiJFQourT1RkbWnrs0jIUP1vdYrgWZ1104Tew7cb5-wp","audience":"https://restapicust.herokuapp.com/api/","grant_type":"client_credentials"}' };

  //retrieve saved token from local storage and send to useCredentials function
  function loadUserCredentials() {
    var token = window.localStorage.getItem(LOCAL_TOKEN_KEY);
    if (token) { //if token is not null
      useCredentials(token);
    }
  }//end loadUserCredentials

  //save Auth0 token to local storage
  function storeUserCredentials(token) {
    window.localStorage.setItem(LOCAL_TOKEN_KEY, token);
    useCredentials(token);

  }//end storeUserCredentials

  //set authenticated variables
  function useCredentials(token) {
    isAuthenticated = true;
    authToken = token;
    // Set the token as header for all requests
    $http.defaults.headers.common.Authorization = 'Bearer ' + authToken;
  }//end useCredentials

  //check if auth token has expired
  function checkAuthOnRefresh() {
    // get stored token from local storage
     var token = window.localStorage.getItem(LOCAL_TOKEN_KEY);
      //if token is not empty
      if (token) {
        //use jwtHelper method to determine if token has expired
        if (jwtHelper.isTokenExpired(token)) {
          //console.log("is expired");
          //if token has expired, call login method to get new token
          login();
        }
      }
    }//end checkAuthOnRefresh
  
  //reset auth variables and remove token from local storage
  function destroyUserCredentials() {
    authToken = undefined;
    isAuthenticated = false;
    $http.defaults.headers.common.Authorization = undefined;
    window.localStorage.removeItem(LOCAL_TOKEN_KEY);
    window.localStorage.removeItem('signIn');
  }//end destroyUserCredentials
  
 //use POST call options
  var login = function(){
    return $q(function(resolve, reject) {
      $http(options).then(function(result) {
        if (result.statusText=="OK") {
          //console.log(result.data.access_token);
          //if successful send token to storeUserCredentials
          storeUserCredentials(result.data.access_token);
          
          resolve(result.data.message);
        } else {
          reject(result.data.success);
          console.log("Something went wrong");
        }
      });
    })
  }//end login
  
  //logout func calls destroyUserCredentials function
  var logout = function() {
    destroyUserCredentials();
  };//end logout

  //method sends token and user email and password to api to be authenticated
  var getInfo = function(user) {
       return $q(function(resolve, reject) {
         $http.post(API_ENDPOINT.url + '/authenticate', user).then(function(result) {
           if (result.data.success) {
             resolve(result.data.msg);
             //if call successful, stringify JSON object and save to local storage
             window.localStorage.setItem('signIn', JSON.stringify(user));
             window.localStorage.setItem('profile', JSON.stringify(result.data));
           } else {
             reject(result.data.success);
             console.log("getMembers failed" + result.data.success);
           }
         });
       });
     };//end getInfo

     //sends updated profile details to server updatePerson method
     var updateProfile = function(user) {
       return $q(function(resolve, reject) {
         $http.put(API_ENDPOINT.url + '/updatePerson', user).then(function(result) {
           if (result.data.success) {
             resolve(result.data.message);
           } else {
             reject(result.data.success);
           }
         });
       });
     };//end updateProfile

    var setPassword = function(user) {

       return $q(function(resolve, reject) {
         $http.put(API_ENDPOINT.url + '/newPassword', user).then(function(result) {
           if (result.data.success) {
             console.log(result);
             resolve(result.data.message);
           } else {
             console.log(result);
             reject(result.data.success);
           }
           
          }).catch(function(err){
            console.log(err);
          });
       });
     };//end setPassword

  //use POST call options
  var sendMessage = function(sosMessage){
    return $q(function(resolve, reject) {
      $http.post(MESSAGE_SERVER.url, sosMessage).then(function(result){
         if (result.statusText=="OK") {
           resolve(result.data.msg);
           console.log("Message sent successfully");
         } else {
           reject(result.data.msg);
           console.log("Something went wrong");
         }
      });
    })
  }//sendMessage
 
  loadUserCredentials();
 
  return {
    login: login,
    options: options,
    logout: logout,
    checkAuthOnRefresh: checkAuthOnRefresh,
    getInfo: getInfo,
    updateProfile: updateProfile,
    setPassword: setPassword,
    sendMessage: sendMessage,
    isAuthenticated: function() {return isAuthenticated;},
  };
})

.factory('ConnectivityMonitor', function($rootScope, $cordovaNetwork){
 
  return {
    isOnline: function(){
      if(ionic.Platform.isWebView()){
        return $cordovaNetwork.isOnline();    
      } else {
        return navigator.onLine;
      }
    },
    isOffline: function(){
      if(ionic.Platform.isWebView()){
        return !$cordovaNetwork.isOnline();    
      } else {
        return !navigator.onLine;
      }
    },
    startWatching: function(){
        if(ionic.Platform.isWebView()){
 
          $rootScope.$on('$cordovaNetwork:online', function(event, networkState){
            console.log("went online");
          });
 
          $rootScope.$on('$cordovaNetwork:offline', function(event, networkState){
            console.log("went offline");
          });
 
        }
        else {
 
          window.addEventListener("online", function(e) {
            console.log("went online");
          }, false);    
 
          window.addEventListener("offline", function(e) {
            console.log("went offline");
          }, false);  
        }       
    }
  }
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