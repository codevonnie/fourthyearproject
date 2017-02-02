angular.module('starter')

.constant('AUTH_EVENTS', {
  notAuthenticated: 'auth-not-authenticated'
})

.constant('tokenReq', {
  url: 'https://membermeauth.eu.auth0.com/oauth/token',
  headers: { 'content-type': 'application/json' },
  body: '{"client_id":"fXqMFIGFPGXAPLNm6ltd0NsGV6fWpvDM","client_secret":"HHnBRmKTpK99fx4RYIVnxiJFQourT1RkbWnrs0jIUP1vdYrgWZ1104Tew7cb5-wp","audience":"https://restapicust.herokuapp.com/api/","grant_type":"client_credentials"}'
})

.constant('REQUESTERTYPE', {
  headers: { 'type': 'person' }
})

.constant('MESSAGE_SERVER', {
  url: 'https://membermemessageserver.herokuapp.com/sendMessage'
})

.constant('API_ENDPOINT', {
  url: 'https://restapicust.herokuapp.com/api'
  //url: 'http://127.0.0.1:8080/api'
});





