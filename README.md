# MemberMe - Membership System

# Introduction
The initial idea for this project was to create a generic membership system for businesses.

# Project Structure
The Project Was Broken up into Four Main Branches:

1. **Master-WebApp**

 This branch contains the Web Application, which was Developed in Visual Studio 2015 using Asp.net.
 
 The Web App Enabled a Business to:

* Set up a personal Account on the MemberMe Web App.
* Add/Update/Delete New Members.
* Display Analytics Based Off Members.
* Display SOS Messages on a google map with the locations of Members In Distress.
* Check Customers In Via QR Code or Email.

2. **Master-MobileApp**

 This branch contains the Mobile Appliaction, which was Developed using Ionic.
 
 The Mobile Application Enabled Members to:
  
* View/Update Personal information
* Send SOS Messages To Associated Business.
* Create a Person QR Code, Used for Fast Check-ins.
* See The Total Amount of Times They Visited a Business 

3. **Master-MainServer**

 This branch contains the Main Server Api, which both the Web/Mobile app send Http Rest request to, to retrive data from our graph  DataBase.
 
 The Server contains several routes, which acomplish the following:
   
  * Login - Business/Members.
  * Autherization Validation. 
  * Add/Update/Delete - Members/Business.
  * Retrive Valid - Member/Business Data
  * Retive Analytic Data

4. **Master-MessageServer**

 This branch contains the SOS Message Server Api, which handles all the SOS messages sent from the Members. These messages then get sent to there associated business. 
  




