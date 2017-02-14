angular.module('starter.controllers', [])
 
.controller('LoginCtrl', function($scope, AuthService, $ionicPopup, $state, $ionicModal, jwtHelper, ConnectivityMonitor, $ionicLoading) {
  
  $scope.user={}; //user object for displaying profile details
  $scope.logo="img/logo.png";

  var logInDetails = window.localStorage.getItem('signIn'); //check localStorage for stored sign in details

  $scope.loginButton=true;

  $scope.validateEmail=function(email){

    var re=/^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    if (email == '' || !re.test(email))
    {
      $scope.emailVal=true;
      $scope.loginButton=true;
    }
    else{
      $scope.emailVal=false;
      $scope.loginButton=false;
    }
  }
    
  if(logInDetails==undefined){ //if no stored details exist set user to take in email and password from user

    //user object to take inputted email and password from login view.  Type automatically set to person
    $scope.user = {
      email: '',
      password: '',
      type: 'person'
    };
  }
  //if sign in details exist in local storage
  else{
    AuthService.checkAuthOnRefresh(); //check if token has expired
    logInDetails = JSON.parse(logInDetails); //parse stored details and set into user objects
     var user = {
      email: logInDetails.email,
      password: logInDetails.password,
      type: 'person'
    };
    //call getInfo method and pass user object - then go to profile page
    AuthService.getInfo(user);
       $state.go('tab.profile');
     
  }//end else

  //modal called for user to set new password if it's the first time login
  $ionicModal.fromTemplateUrl('templates/passwordInput.html', {
            scope: $scope,
            animation: 'slide-in-up'
  }).then(function(modal) {
            $scope.modal = modal;
          });

  //login method called when user presses login button - passes entered user details  
  $scope.login = function(user) {

      //if user is not connected to the internet - popup alert shows
      if(!ConnectivityMonitor.isOnline()){
          var alertPopup = $ionicPopup.alert({
              title: 'No internet connection',
              template: "You need an internet connection to login"
              });
        }
      //if login is successful
      var onSuccess = function () {
        
        $ionicLoading.hide();
        $scope.check={};
        
        //if user password starts with *x* - it's the first time login so open set password modal
        if(user.password.startsWith("*x*")){

              //make sure details are not saved to local storage until password is changed
              window.localStorage.removeItem("signIn"); 
              $scope.modal.show();
        }
        //else send user to profile tab
        else{
              $state.go('tab.profile');
        }

        //called if user cancels changing their password - closes modal
        $scope.closeModal = function() {
              $scope.modal.hide();
              $scope.message="";
        };

        //called when user clicks Save button for new password
        $scope.changePass = function() {
          //if user is not connected to the internet - popup alert shows
          if(!ConnectivityMonitor.isOnline()){
          var alertPopup = $ionicPopup.alert({
              title: 'No internet connection',
              template: "You need an internet connection to continue"
            });
        }//end if

          //if password and confirmation password don't match
          if ($scope.check.pass != $scope.check.repass) {
            $scope.message="Passwords have to match"; //alert user
            $scope.IsMatch=true; //show message
            //reset password fields to blank
            $scope.check.pass="";
            $scope.check.repass="";
            return false;
          }
          //if password is left blank
          else if(($scope.check.pass == undefined)||($scope.check.pass == "")) {
            $scope.message="Please fill in all fields";//alert user
            $scope.IsMatch=true; //show message
            return false;
          }
          //if password starts with reserved symbols *x*
          else if(($scope.check.pass.startsWith("*x*"))) {
            $scope.message="Not a valid password, try another"; //alert user
            $scope.IsMatch=true; //show message
            //reset password fields to blank
            $scope.check.pass="";
            $scope.check.repass="";
            return false;
          }
          else{
            $scope.message="";
            $scope.user.password=$scope.check.pass; //set user password to inputted password
            //send new user object to setPassword service
            AuthService.setPassword(user).then(function(response){
                $scope.modal.hide(); //hide modal
                $state.go('tab.profile'); //go to profile view

            },
            function(data){
              console.log("error------");
            });
            
          }
        $scope.IsMatch=false; //get rid of alert message
      }


    };//end success
    
      //if login is not successful, alert user with popup and allow them to reenter deails
      var onError = function () {
        $ionicLoading.hide();
        var alertPopup = $ionicPopup.alert({
            title: 'Login failed!',
            template: "Check details and try again"
            });
      };

      //call AuthService method login to get token
      AuthService.login().then(function() {
        //show loading spinner while call to Google maps api takes place
        $scope.loading = $ionicLoading.show({
              content: '<i class="icon ion-loading-c"></i>',
              animation: 'fade-in',
              showBackdrop: false,
              maxWidth: 50,
              showDelay: 0
            })
        //call AuthService method getInfo to get user profile details
        AuthService.getInfo(user).then(onSuccess, onError);
          });
      }//end login


  })//end LoginCtrl

 //Profile view controller
.controller('ProfileCtrl', function($scope, AuthService, API_ENDPOINT, $http, $state, $ionicListDelegate, $ionicPopup, ConnectivityMonitor) {
  
    $scope.profile={}; //empty profile object
    $scope.origProfile={};
    $scope.toggle=false; 
    $scope.listCanSwipe = true; //profile list is swipeable
    var profileData=window.localStorage.getItem('profile'); //get profile data from local storage
    profileData=JSON.parse(profileData); //parse JSON object
    $scope.profile=profileData; //save JSON object to $scope variable
    $scope.qrcode=$scope.profile.email; //qrcode text is the object 
    $scope.profile.icePhone = parseInt(profileData.icePhone); //convert phone numbers to strings
    $scope.profile.phone = parseInt(profileData.phone);
     
    //convert date milliseconds to date string
    var joined = new Date(parseInt(profileData.joined)).toDateString();
    var dob = new Date(parseInt(profileData.dob)).toDateString();
    var membership = membership = new Date(parseInt(profileData.membership)).toDateString();;
    
    $scope.joined = joined;
    $scope.dob = dob;

    
    //if membership is not a valid date set it to null or save the date value to the scope
    if(membership=='Invalid Date'){
      $scope.membership = null;
    }
    else{
      $scope.membership = membership;
    }

    
    //edit function - triggered when user hits edit on profile items
    $scope.edit = function() {
      $ionicListDelegate.closeOptionButtons(); //closes the slide effect
      $scope.toggle=!$scope.toggle; //turns toggle on/off
    }
    //cancel method on profile edit
    $scope.cancel = function(){
      
      $scope.toggle=!$scope.toggle; //hide input box
      profileData=window.localStorage.getItem('profile');//get profile details from local storage
      profileData=JSON.parse(profileData); //parse JSON object
      $scope.profile=profileData; //save JSON object to $scope variable
      $scope.profile.icePhone = parseInt(profileData.icePhone); //convert phone number string to number
      $scope.profile.phone = parseInt(profileData.phone);
    }
    //submit function for profile edits
    $scope.submit = function() {
      
      if(!ConnectivityMonitor.isOnline()){
        var alertPopup = $ionicPopup.alert({
            title: 'No internet connection',
            template: "You need an internet connection to update details"
            });

        $scope.toggle=!$scope.toggle; //hide input box
        profileData=window.localStorage.getItem('profile');
        profileData=JSON.parse(profileData); //parse JSON object
        $scope.profile=profileData; //save JSON object to $scope variable
        $scope.profile.icePhone = parseInt(profileData.icePhone);//convert phone number string to number
        $scope.profile.phone = parseInt(profileData.phone);
    }
    else{
      AuthService.checkAuthOnRefresh(); //check if token has expired
      $scope.toggle=!$scope.toggle; //hide input box
      $scope.profile.tempEmail=$scope.profile.email; //set tempEmail for updateMethod on server
      $scope.profile.icePhone = $scope.profile.icePhone.toString();//convert phone number string to number
      $scope.profile.phone = $scope.profile.phone.toString();
      profileData=$scope.profile;
      AuthService.updateProfile(profileData).then(onSuccess, onError); //call AuthService method updateProfile and pass profileData object
      $scope.profile.icePhone = parseInt(profileData.icePhone); //convert phone numbers to strings
      $scope.profile.phone = parseInt(profileData.phone);

      }//end else

    }//end submit

    var onSuccess = function(){
        var alertPopup = $ionicPopup.alert({
            title: 'Success',
            template: "Details updated."
            });

        profileData=$scope.profile; //save updated $scope obj to profileData obj
        window.localStorage.setItem('profile', JSON.stringify(profileData)); //set updated profile details to local storage
      }
      
      var onError = function(){
        console.log("boo not successful");
        var alertPopup = $ionicPopup.alert({
            title: 'Could Not Update',
            template: "Details not updated. Please try again."
            });
      }

  //calls AuthService method getInfo to authorize user on db and retrieve profile details
  $scope.getInfo = function() {
    AuthService.getInfo();

  };
   
})//end ProfileCtrl

 .controller('LogoutCtrl', function($scope, AuthService, $state, $ionicPopup) {
  
    //logout function
    $scope.logout = function() {
        
        var confirmPopup = $ionicPopup.confirm({
        title: 'Logout',
        template: 'Are you sure you want to logout?'
      });
   //call confirmation popup
   confirmPopup.then(function(res) {
     //if user selects ok
     if(res) {
      AuthService.logout(); //call AuthService method logout
      $state.go('login'); //go to login view
     }
   })
  }//logout
 }) //LogoutCtrl


//Controller for user if they are having an issue while on the business premises
.controller('SosCtrl', function($scope, AuthService, $http, $state, $ionicLoading, $cordovaGeolocation, $ionicPopup, ConnectivityMonitor) {
  
  //empty object to take sos details for sending to business
  $scope.sos={};
  $scope.emer=true;

    //submit function called from sos page when Send SOS button is clicked
  $scope.submit=function(){
    //show confirmation popup to ensure message is not sent in error
    var confirmPopup = $ionicPopup.confirm({
     title: 'Send SOS Message',
     template: 'Are you sure you want to send this message?'
   });
   //call confirmation popup
   confirmPopup.then(function(res) {
     //if user selects ok
     if(res) {

       if(!ConnectivityMonitor.isOnline()){
        var alertPopup = $ionicPopup.alert({
            title: 'No internet connection',
            template: "You need an internet connection to send SOS"
            });
        $scope.toggle=false; //hide map div on sos page

      }
      else{
        //get user profile details from local storage
        var sosDetails = window.localStorage.getItem('profile');
        sosDetails = JSON.parse(sosDetails); //parse object
        $scope.sos.email = sosDetails.email; //save person email to sos object
        $scope.sos.business = sosDetails.businessName;
        $scope.sos.name = sosDetails.name;
        var sosDetails = window.localStorage.getItem('latLng'); //get user lat and long (set below)
        sosDetails = JSON.parse(sosDetails); //parse object
        //save user lat and long coordinates to sos object
        $scope.sos.latitude = sosDetails.lat; 
        $scope.sos.longitude = sosDetails.lng;
        sosDetails=$scope.sos;
        //calls service sendMessage and passes sosDetails object
        AuthService.sendMessage(sosDetails).then(onSuccess, onError);
      }
    }
      else {
      //if user selects to cancel sending SOS
       var alertPopup = $ionicPopup.alert({
          title: 'Message not sent' ,
          template: "Message not sent"
          });
     }
   });
   //if call to sendMessage is successful - alert user
   var onSuccess = function () {
      var alertPopup = $ionicPopup.alert({
          title: 'Message sent' ,
          template: "Message sent"
          });
          //reset fields when message is sent
          $scope.sos.status="";
          $scope.sos.message="";
      }
    //if call to sendMessage is unsuccessful - alert user
    var onError = function () {
      var alertPopup = $ionicPopup.alert({
          title: 'Message sending failed!',
          template: "Try calling " + $scope.sos.businessName
          });
    };


  }//end submit

  //if user is not connected to the internet - popup alert shows
  if(!ConnectivityMonitor.isOnline()){
        var alertPopup = $ionicPopup.alert({
            title: 'No internet connection',
            template: "You need an internet connection to send SOS"
            });
      $scope.toggle=false; //hide map div on sos page

  }

  $scope.emerSelect = function(){
    $scope.emer=false;
  }

  //function to reload page if user connects to internet or gps after initially not having them turned on
  $scope.reload = function(){
    $state.reload();
  }//end reload

  //if device is connected to the internet
  if(ConnectivityMonitor.isOnline()){

    AuthService.checkAuthOnRefresh();//check if token has expired
    $scope.toggle=true; //show map div on SOS page

    //show loading spinner while call to Google maps api takes place
    $scope.loading = $ionicLoading.show({
          content: '<i class="icon ion-loading-c"></i>',
          animation: 'fade-in',
          showBackdrop: false,
          maxWidth: 50,
          showDelay: 0
        })

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
      $ionicLoading.hide(); //when api call finishes hide loading spinner

    }, function(error){
      //if map can't be retrieved, hide loading spinner
      $ionicLoading.hide();
      //alert user to check gps settings
      var alertPopup = $ionicPopup.alert({
            title: 'Could not get location',
            template: "Check your location services"
            });
        $scope.toggle=false; //hide map div
    });  
  }//end
    
});//end SosCtrl