var express = require('express')
  , app = express()
  , sse = require('./sse')
  , bodyParser = require('body-parser')
  , cors = require('cors')
  , port = process.env.PORT || 3000;        // set the port if testing locally;


// Get our request parameters
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());
app.use(cors());
app.use(sse);


// middleware to use for all requests
app.all('*', function (req, res, next) {
  // do logging
  res.header("Access-Control-Allow-Origin", "*");
  res.header('Access-Control-Allow-Methods', 'PUT, GET, POST, DELETE, OPTIONS');
  res.header("Access-Control-Allow-Headers", "Content-Type, Accept");
  console.log('\nSomeOne Connected');

  next(); // make sure we go to the next routes and don't stop here
});

var router = express.Router();

var connections = [];
var messageObj = { status: "", message: "", latitude: "", longitude: "", email: "", business: "" }

// ------------------------ POST SendMessage Route ---------------------------------
/*Send an SOS Post message to the Route and respond with Status Code 200 (ok)
* 
*BODY KEYS:
* - email
* - latitude
* - longitude
* - message
* - business
* - status
*/
router.post('/sendMessage', function (req, res) {

  messageObj.message = req.body.message;
  messageObj.status = req.body.status;
  messageObj.business = req.body.business;
  messageObj.email = req.body.email;
  messageObj.latitude = req.body.latitude;
  messageObj.longitude = req.body.longitude;

  console.log("\nIn Send Message: Results -", messageObj);

  //For each Connection(connected business) Send the Message Object
  for (var i = 0; i < connections.length; i++) {
    connections[i].sseSend(messageObj)
  }

  res.sendStatus(200)
}).catch(function (error) {
  console.log(error);
  res.sendStatus(400)//Bad Request
})

// ------------------------ GET Stream Route ---------------------------------
//Keeps Track Of All Current Connections and sends them the message
router.get('/stream', function (req, res) {
  console.log("\nIn Stream", messageObj);
  res.sseSetup()//Setup of Stream
  res.sseSend(messageObj)//Send the Object "Message""
  connections.push(res)//Push the New Connection To the Array 

  messageObj = { status: "", message: "", latitude: "", longitude: "", email: "", business: "" }
  console.log("Wiped Message?", messageObj);
})

app.use('/', router);

app.listen(port);
console.log('Fancy Magic Messaging Happening At Port:' + port);

