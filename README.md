
                                    __  __                _               __  __      
                                   |  \/  | ___ _ __ ___ | |__   ___ _ __|  \/  | ___ 
                                   | |\/| |/ _ \ '_ ` _ \| '_ \ / _ \ '__| |\/| |/ _ \
                                   | |  | |  __/ | | | | | |_) |  __/ |  | |  | |  __/
                                   |_|  |_|\___|_| |_| |_|_.__/ \___|_|  |_|  |_|\___|

# Introduction
MemberMe is a Membership Sytem designed for Businesses. It enables them to manage new/old members, help speed up the process of check-ins and help find members in Distress faster using Geolocation via Google Maps.

# Project Dissertation
The project dissertation can be found [HERE](https://github.com/codevonnie/fourthyearproject/blob/master/Dissertation.pdf).

# Project Screen cast

A screen cast demonstrating the projects features and functionality can be found [HERE](https://youtu.be/KZUqt7QmrgU)

# Project Structure
The Project Was Broken up into Four Main Branches:

1. **[Master-WebApp](https://github.com/codevonnie/fourthyearproject/tree/Master-WebApp)**

 This branch contains the Web Application, which was Developed in Visual Studio 2015 using Asp.net.
 
 The Web App Enables a Business to:

* Set up a personal Account on the MemberMe Web App.
* Add/Update/Delete New Members.
* Display Analytics Based Off Members.
* Display SOS Messages on a google map with the locations of Members In Distress.
* Check Customers In Via QR Code or Email.

2. **[Master-MobileApp](https://github.com/codevonnie/fourthyearproject/tree/Master-MobileApp)**

 This branch contains the Mobile Appliaction, which was Developed using Ionic.
 
 The Mobile Application Enables Members to:
  
* View/Update Personal information
* Send SOS Messages To Associated Business.
* Create a Person QR Code, Used for Fast Check-ins.
* See The Total Amount of Times They Visited a Business 

3. **[Master-MainServer](https://github.com/codevonnie/fourthyearproject/tree/Master-MainServer)**

 This branch contains the Main Server Api, which both the Web/Mobile app send Http Rest request to, to retrive data from our graph  DataBase.
 
 The Server contains several routes, which accomplish the following:
   
  * Login - Business/Members.
  * Autherization Validation. 
  * Add/Update/Delete - Members/Business.
  * Retrive Valid - Member/Business Data
  * Retive Analytic Data

4. **[Master-MessageServer](https://github.com/codevonnie/fourthyearproject/tree/Master-MessageServer)**

 This branch contains the SOS Message Server Api, which handles all the SOS messages sent from the Members. These messages then get sent to there associated business. 
  




