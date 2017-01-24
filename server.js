//Variables
var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var neo4j = require('neo4j-driver').v1;
var port = process.env.PORT || 8100;        // set the port if testing locally
var morgan = require('morgan');
var config = require('./config/database');
var cors = require('cors');
var passport = require('passport');
var mongoose = require('mongoose');
var jwt = require('express-jwt');
var rsaValidation = require('auth0-api-jwt-rsa-validation');
var cloudinary = require('cloudinary');
var Person = require('./app/models/person');
var Business = require('./app/models/business');

var driver = neo4j.driver("bolt://hobby-gemhpbboojekgbkeihhpigol.dbs.graphenedb.com:24786", neo4j.auth.basic("app57975900-aEgAtX", "tGm6FwOKgU7sQyPDUACj"));

// Get our request parameters
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());
app.use(cors());
app.use(morgan('dev'));

var jwtCheck = jwt({
  secret: rsaValidation(),
  audience: 'https://restapicust.herokuapp.com/api/',
  issuer: "https://membermeauth.eu.auth0.com/",
  algorithms: ['RS256']
});

app.use(jwtCheck);

app.use(function (err, req, res, next) {
  if (err.name === 'UnauthorizedError' || err.name === 'TypeError') {
    res.status(401).json({ message: 'Missing or invalid token' });
  }
});

//----------------- Cloudinary Configuration  NOT WORKING-----------------
cloudinary.config({
  cloud_name: 'hlqysoka2',
  api_key: '836473758946851',
  api_secret: 'HvbKatij3YP8zxk_1I7DOY5m8mA'
});

var router = express.Router();

// middleware to use for all requests
app.all('*', function (req, res, next) {
  // do logging
  res.header("Access-Control-Allow-Origin", "*");
  res.header('Access-Control-Allow-Methods', 'PUT, GET, POST, DELETE, OPTIONS');
  res.header("Access-Control-Allow-Headers", "Content-Type, Accept");


  console.log('Something is happening.');
  next(); // make sure we go to the next routes and don't stop here
});



// ----------------- Authentication Route For Persons/Buisness Via DataBase ----------------- 
// POST http://localhost:8080/api/authenticate)
router.post('/authenticate', function (req, res) {

  var queryString = "";
  var session = driver.session();
  console.log('I am authenticating');

  // if req.body.password.contains(¬¬¬)
  //respond to user with change password and relocate them to set up new pass page

  // check whether authentication request is from mobile app or web app - make appropriate database call
  if (req.body.type === "person")
    queryString = "Match (a:Person) WHERE a.email='" + req.body.email + "' AND a.password='" + req.body.password + "' Return a LIMIT 1";
  else if (req.body.type === "business")
    queryString = "Match (a:Business) WHERE a.email='" + req.body.email + "' AND a.password='" + req.body.password + "' Return a LIMIT 1";

  session
    .run(queryString)
    .then(function (result) {
      // If Person/Biz is found and UsrName/password is Correct
      if (result.records[0] == null)
        res.json({ success: false });
      else {
        var credList = [];//create a new list

        //Loop over Results, should always just be 1 returned
        result.records.forEach(function (record) {
          //If its A PERSON logging In
          if (req.body.type === "person") {

            //Send the Response Back [List]
            res.json({
              success: true,
              name: record._fields[0].properties.name,
              age: record._fields[0].properties.age,
              dob: record._fields[0].properties.dob.toString(),
              address: record._fields[0].properties.address,
              phone: record._fields[0].properties.phone,
              iceName: record._fields[0].properties.iceName,
              icePhone: record._fields[0].properties.icePhone,
              joined: record._fields[0].properties.joined.toString(),
              email: record._fields[0].properties.email,
              imgUrl: record._fields[0].properties.imgUrl,
              guardianName: record._fields[0].properties.guardianName,
              guardianNum: record._fields[0].properties.guardianNum,
            });

            /*JSON RESPONSE =
              "success": true,
              "name": "Paul Potts4564",
              "dob": "2315648943215",
              "address": "123 Fake Street5464",
              "phone": "353879876543",
              "iceName": "Bob Potts5464",
              "icePhone": "353871234567",
              "joined": "1484921393189",
              "email": "test@email.com",
              "imgUrl": "https://res.cloudinary.com/hlqysoka2/image/upload/v1484837295/itxmpiumdiu56q7sbebn.jpg",
              "guardianName": "timtim",
              "guardianNum": "1800696969"
            */

          }
          else {
            //Add The biz Details to the [list]
            //console.log(record._fields[0].properties.name);
            res.json({ success: true, name: record._fields[0].properties.name })
          }
        });

        session.close();//close the session
        driver.close();////close driver
      }
    })
    .catch(function (error) {
      console.log(error);
      res.json({ success: false });
    });
});


//-----------------------------------    ADD A NEW Business   -------------------------------- 
router.post('/addCompany', function (req, res) {
  var session = driver.session();

  var business = new Business();         //create a new instance of the Business model
  business.name = req.body.name;         //Set the Business name (comes from the request)
  business.address = req.body.address;   //Set the Business address
  business.phone = req.body.phone;       //Set the Business Phone Num
  business.email = req.body.email;       //Set the Business Email
  business.password = req.body.password; //Set the Business Password

  session
    .run("Create (b:Business {name:'" + business.name + "', address:'" + business.address + "', phone:'" + business.phone + "', email:'" + business.email + "', password:'" + business.password + "'})")

    .then(function () {
      console.log("Business created");
      res.json({ message: 'Business created!' });
      session.close();
      driver.close();
    })
    .catch(function (error) {
      // if error is a business with this email address already exists, return fail response
      var s = error.fields[0].message;
      if (s.includes("already exists")) {
        res.json({ success: false });
      }
      else {
        res.send(error);
      }

    });
});

//-----------------------------------    Add a New Person To The DataBase   -------------------------------- 
router.post('/addPerson', function (req, res) {
  var session = driver.session();

  var person = newPersonObj(req);      // create a new instance of the Person model

  var joined = new Date(); // Person join date is the current date/time of entry
  person.joined = joined.getTime(); // Join date is converted to milliseconds

  session
    .run("Create (a:Person {name:'" + person.name + "', address:'" + person.address + "', phone:'" + person.phone + "', iceName:'" + person.iceName + "', icePhone:'" + person.icePhone + "', joined:" + person.joined + ", dob:" + person.dob + ", email:'" + person.email + "', imgUrl:'" + person.imgUrl + "', password:'" + person.password + "', guardianName:'" + person.guardianName + "', guardianNum:'" + person.guardianNum + "'})")
    .then(function () {
      console.log("Person created");
      res.json({ message: 'Person created!' });
      session.close();
    })
    .catch(function (error) {
      var s = error.fields[0].message;
      if (s.includes("already exists")) {
        res.json({ success: false });
      }
      else {
        res.send(error);
      }
    })
})//addPerson


//-----------------------------------    DELETE A Business BY EMAIL  ---------------------------------------

//-------------> Should probably add an extra authorisation step for deletion to avoid mistakes! <----------

router.delete('/deleteCompany', function (req, res) {
  var session = driver.session();
  var email = req.body.email;         //Set the Business email (comes from the request)
  //query checks for business, deletes business, all of it's relationships and its members
  session.run("MATCH (b:Business {email:'" + email + "'}) OPTIONAL MATCH (b)-[r]-(p) DETACH DELETE b, r, p return COUNT(*)")

    .then(function () {

      // IF count(*) Returns > 0, Entry has been made
      if (result.records.length > 0)
        res.json({ success: true, message: 'Business Deleted' });
      else
        res.json({ success: false, message: 'Problem Deleting Business Check Email/Password' });

      session.close();
      driver.close();
    })
    .catch(function (error) {

      res.send(error);
    });
});





//-----------------------------------    POST Find Persons - Arivals WebApp -------------------*FIX*--------
// Working if you leave guardianName/num as null or dont add them in request 
// May need to Fix Timeout if person isnt found
router.post('/findPerson', function (req, res) {
  console.log("in findPerson ");
  var session = driver.session();

  var person = newPersonObj(req);
  session
    .run("MATCH (a:Person {name:'" + person.name + "', address:'" + person.address + "', phone:'" + person.phone + "', iceName:'" + person.iceName + "', icePhone:'" + person.icePhone + "', joined:" + person.joined + ", dob:" + person.dob + ", email:'" + person.email + "', imgUrl:'" + person.imgUrl + "', guardianName:'" + person.guardianName + "', guardianNum:'" + person.guardianNum + "'}) RETURN a")

    .then(function (result) {
      result.records.forEach(function (record) {//Iterate over results
        /*var jsonObj;//Create a new person object to add as the response
        jsonObj = ({
          success: true,
          name: record._fields[0].properties.name,
          age: record._fields[0].properties.age,
          dob: record._fields[0].properties.dob,
          address: record._fields[0].properties.address,
          phone: record._fields[0].properties.phone,
          iceName: record._fields[0].properties.iceName,
          icePhone: record._fields[0].properties.icePhone,
          joined: record._fields[0].properties.joined,
          email: record._fields[0].properties.email,
          imgUrl: record._fields[0].properties.imgUrl,
          guardianName: record._fields[0].properties.guardianName,
          guardianNum: record._fields[0].properties.guardianNum,
        });*/

        //Send the Response Back
        res.json({
          success: true,
          message: "Person Found!"
        });
        console.log("Success: Found Person");
      });
      session.close();//close the session
      driver.close();////close driver

    })
    .catch(function (err) {
      console.log(err);
      res.json({ success: false, message: err });
    })
})

//-----------------------------------    Add a Relationship Between Person + Company -------------------------------- 
router.post('/addRelationship', function (req, res) {
  var session = driver.session();
  session.run("MATCH (a:Person {email: '" + req.body.email + "'}), (b:Business {name: '" + req.body.bName + "'}) CREATE (a)-[:IS_A_MEMBER]->(b)-[:HAS_A_MEMBER]->(a) RETURN COUNT(*)")
    .then(function (result) {

      // IF count(*) Returns > 0, Entry has been made
      if (result.records.length > 0)
        res.json({ success: true, message: 'Person<-REL->Business' });
      else
        res.json({ success: false, message: 'Problem Creating Relationship Check Name/Email' });

      session.close();
      driver.close();
    })
    .catch(function (err) {
      console.log(err);
      res.json({ success: false, message: err });
    })
})//addRelationship


//-----------------------------------    DELETE A Person -------------------------------- 
router.delete('/deletePerson', function (req, res) {

  var session = driver.session();
  // find person by email and also find any relationships it may have - delete node and relationships
  session
    .run("Match (a:Person) WHERE a.email='" + req.body.email + "' OPTIONAL MATCH (a)-[r]-() DETACH DELETE a, r return COUNT(*)")
    .then(function (result) {
      console.log("Person deleted");
      console.log(result);

      // IF count(*) Returns > 0, Entry has been made
      if (result.records.length > 0)
        res.json({ success: true, message: 'User Deleted' });
      else
        res.json({ success: false, message: 'Problem Deleting User Check Name/Email' });

      session.close();
      driver.close();
    })
    .catch(function (error) {
      console.log(error);
      res.json({ success: false, message: err });
    })
})//deletePerson


//----------------------------------- UPDATE A Person -------------------------------- 
router.put('/updatePerson', function (req, res) {

  var session = driver.session();
  var person = newPersonObj(req);

  session
    .run("Match (a:Person) WHERE a.email='" + person.email + "' SET a.name='" + person.name + "', a.address='" + person.address + "', a.phone='" + person.phone + "', a.iceName='" + person.iceName + "', a.icePhone='" + person.icePhone + "', a.joined='" + person.joined + "', a.dob=" + person.dob + ", a.imgUrl='" + person.imgUrl + "', a.email='" + person.email + "' return COUNT(*)")
    .then(function (result) {

      // IF count(*) Returns > 0, Updating has been made successfully
      if (result.records.length > 0)
        res.json({ success: true, message: 'User Details Updated' });
      else
        res.json({ success: false, message: 'Problem Updating User Check Name/Email' });

      session.close();
      //driver.close();
    })
    .catch(function (error) {
      console.log(error);
      res.json({ success: false, message: err });
    })
})//updateperson


//-----------------------------------    Create A New Person Object From Http Request  -------------------------------- 
function newPersonObj(req) {
  var person = new Person();
  person.name = req.body.name;
  person.address = req.body.address;
  person.phone = req.body.phone;

  person.iceName = req.body.iceName;
  person.icePhone = req.body.icePhone;

  person.joined = req.body.joined;
  person.dob = req.body.dob;
  person.email = req.body.email;
  person.password = req.body.password;
  person.imgUrl = req.body.imgUrl;

  person.guardianName = null;
  person.guardianNum = null;

  //Check to see if the person was under 18 and Added a Guardian
  if (req.body.guardianName != null && req.body.guardianNum != null) {
    person.guardianName = req.body.guardianName;
    person.guardianNum = req.body.guardianNum;
  }
  return person;
}



/*-----------------------------------    GET ALL Business   ----------------------------------- 
* GET Request returns all the Buisness Nodes and sends them all as a JSON response to the client
*/
//---------------------------> IS THIS A NECESSARY METHOD? Testing Only <-----------------

router.get('/businessMembers', function (req, res) {

  var session = driver.session();//Create a new session
  session.run('MATCH (a:Business) RETURN a LIMIT 25')
    .then(function (result) {
      var bizList = [];//create a new list
      result.records.forEach(function (record) {//Iterate over results
        console.log(record._fields[0].properties);//log results
        bizList.push(record._fields[0].properties)//Add The business To a list
      });
      res.json({ message: bizList });//send the bizList as a response
      session.close();//close the session
      driver.close();////close driver
    })
    .catch(function (error) {
      console.log(error);
      res.send(error);
    });
});









//-----------------------------------    Upload Images To Cloudinary *NOT WORKING*    -------------------------------- 
/*router.post('/uploadPic', function (req, res) {
  console.log("IN uploadPic")
  var fileStream = req.body.fileStream;
  var barCode = req.body.barCode;
  console.log("Got array")
console.log(fileStream)
  
 
 
cloudinary.v2.uploader.upload(fileStream, 
    function(error, result) {
      console.log(result); 
      res.send(result);
    })
    .catch(function (error) {
      console.log(error);
      res.send(error);
    })
})*/




// REGISTER OUR ROUTES -------------------------------
// all of our routes will be prefixed with /api
app.use('/api', router);

// Server listens on local port or on Heroku port
//=============================================================================

app.listen(port);
console.log('Magic happens on port ' + port);