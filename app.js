var express = require('express')
  , app = express()
  , sse = require('./sse')
  , bodyParser = require('body-parser')
  , cors = require('cors');


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
var messageObj = { status: "", message: "" }


router.post('/sendMessage', function (req, res) {

  messageObj.message = req.body.personMessage;
  messageObj.status = req.body.status;

  console.log("\nIn Send Message: Results -", messageObj);

  //For each connection send the results
  for (var i = 0; i < connections.length; i++) {
    connections[i].sseSend(messageObj)
  }
  res.sendStatus(200)
})

router.get('/stream', function (req, res) {
  console.log("\nIn Stream", messageObj);
  res.sseSetup()
  res.sseSend(messageObj)
  connections.push(res)
  messageObj = { status: "", message: "" }
  console.log("Wiped Message?", messageObj);
})

app.use('/', router);

app.listen(3000, function () {
  console.log('Listening on port 3000...')
})

