# Main Server - API

## Brief
The following branch contains a Main Server, developed using NodeJs. 

This server acts as the middle man between the Web/Mobile App and the Database.

Http RESTful requests are sent from the Web/Mobile App to the different routes on the API. The API then retrieves data from the Database according to the route and sends it back as a response.

## Running The Server
Below are the steps To run the Server locally:

* Download the Source Code into a local folder
* Install [Nodejs](https://nodejs.org/en/)
* Open the folder in your favorite Text Editor, we used [Visual Studio Code](https://code.visualstudio.com/download))
* Open up the Integrated Terminal on VS Code
* CD into the Projects Directory if not already there
* Run the Following command to run the server locally
```
node .\server
```

The Server will now be running on Port 8100. 

In order to connect via this port and start using the routes on the Mobile and Web Application, you will need to open up the source code for each application and change the Port the applications sends requests too.
