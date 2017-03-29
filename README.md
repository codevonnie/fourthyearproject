# Main Server - API

## Brief
This Branch acts as the Middle Man between the Web/Mobile App and the Database.

Http Rest Requests are sent from the Web/Mobile App to the differnt Routes on ther API. The API then retrieves data from the Database according to the Route and sends it back as a response.

## Running The Server
Below are the following Steps To run the Server Locally:

* Download the Source Code into a Folder Locally.
* Install [Nodejs.](https://nodejs.org/en/)
* Open the Folder in your favorite Text Editor, We Used [Visual Studio Code.](https://code.visualstudio.com/download))
* Open up the Integrated Terminal on VS Code.
* CD into the Projects Directory if not already there.
* Run the Following Command to run the server locally.
```
node .\server
```

The Server will now be running on Port 8100. 

In order to connect via this port and start using the routes on the Mobile and Web Application, you will need to open up the source code for each application and change the Port the applications sends Requests too.
