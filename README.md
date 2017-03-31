# Message Server - Api

## Brief
The following branch contains a Messaging Server, developed using NodeJs. It acts as the middle man between the Web App and the Mobile App.

It allows Businesses on the Web App to connect to the server over Http using Server Side Events and wait for any outgoing messages from the server without continual polling.

SOS Messages are sent to a route setup on the Api using Http RESTful request from the Mobile App, which in turn, are then sent out to connected Companies on the Web App.


## Running The Server
Below are the following Steps To run the Server Locally:

* Download the Source Code into a Folder Locally.
* Install [Nodejs.](https://nodejs.org/en/)
* Open the Folder in your favorite Text Editor, We Used [Visual Studio Code.](https://code.visualstudio.com/download))
* Open up the Integrated Terminal on VS Code.
* CD into the Projects Directory if not already there.
* Run the Following Command to run the server locally.
```
node .\app
```

The Server will now be running on Port 3000. 

In order to connect via this port and start using the routes on the Mobile and Web Application, you will need to open up the source code for each application and change the Port the applications sends Requests too.
