//Variables
var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var neo4j = require('neo4j-driver').v1;
var port = process.env.PORT || 8100;        // set the port if testing locally
var config = require('./config/database');
var cors = require('cors');
var passport = require('passport');
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

// ------> http://stackoverflow.com/questions/39239051/rs256-vs-hs256-whats-the-difference <---------
//Stack overflow on why RS256 is better for encryption, good for Write Up
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

var router = express.Router();

// ----------------- Middleware to use for all Requests ----------------------------------------------------- 
app.all('*', function (req, res, next) {
  // Do logging
  res.header("Access-Control-Allow-Origin", "*");
  res.header('Access-Control-Allow-Methods', 'PUT, GET, POST, DELETE, OPTIONS');
  res.header("Access-Control-Allow-Headers", "Content-Type, Accept");

  console.log('Something is happening.');
  next(); // Make sure we go to the next routes and don't stop here
});


// ----------------- Authentication Route For Persons/Buisness Via DataBase -------------------------------- 
// POST http://localhost:8080/api/authenticate)
router.post('/authenticate', function (req, res) {

  var queryString = "";
  var session = driver.session();

  var pwd = req.body.password.trim();
  var email = req.body.email.trim().toLowerCase();

  console.log('I am authenticating');

  // If req.body.password.contains(*x*)
  //respond to user with change password and relocate them to set up new pass page

  // check whether authentication request is from mobile app or web app - make appropriate database call
  if (req.body.type === "person")
    queryString = "Match (a:Person)-[r:IS_A_MEMBER]->(b:Business) WHERE a.email='" + email + "' AND a.password='" + pwd + "' Return a, b LIMIT 1";
  else if (req.body.type === "business")
    queryString = "Match (a:Business) WHERE a.email='" + email + "' AND a.password='" + pwd + "' Return a LIMIT 1";

  session
    .run(queryString)
    .then(function (result) {
      // If Person/Biz is found and UsrName/password is Correct
      if (result.records[0] == null) {
        res.json({ success: false });
        console.log('Failed To Get Authenticated');
      }
      else {

        //Loop over Results, should always just be 1 returned
        result.records.forEach(function (record) {
          //If its A PERSON logging In
          if (req.body.type === "person") {

            //Send the Response Back [List]
            res.json({
              success: true,
              name: record._fields[0].properties.name,
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
              visited: record._fields[0].properties.visited,
              membership: record._fields[0].properties.membership,
              datesVisited: record._fields[0].properties.datesVisited,

              bEmail: record._fields[1].properties.email//BUSINESS EMAIL
            });
            console.log('Found You! Permission Granted');
          }
          else {
            console.log('Found You! Permission Granted');
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


//----------------------------------- ADD A NEW Business   -------------------------------------------------
router.post('/addCompany', function (req, res) {
  var session = driver.session();

  var business = new Business();                                    //create a new instance of the Business model
  business.name = req.body.name.trim().toString();                  //Set the Business name (comes from the request)
  business.address = req.body.address.trim().toString();            //Set the Business address
  business.phone = req.body.phone.trim().toString();                //Set the Business Phone Num
  business.email = req.body.email.trim().toString().toLowerCase();  //Set the Business Email
  business.password = req.body.password.trim().toString();          //Set the Business Password
  business.emergencyNum = req.body.emergencyNum.trim().toString();  //Set the Business Password

  session
    .run("Create (b:Business {name:'" + business.name + "', address:'" + business.address + "', phone:'" + business.phone + "',emergencyNum:'" + business.emergencyNum + "', email:'" + business.email + "', password:'" + business.password + "'})")

    .then(function () {
      console.log("Business created");
      res.json({ message: 'Business created!', success: true });
      session.close();
      driver.close();
    })
    .catch(function (error) {
      // If error is a business with this email address already exists, return fail response
      var s = error.fields[0].message;
      if (s.includes("already exists")) {
        res.json({ success: false });
      }
      else {
        res.send(error);
      }
    });
});


//----------------------------------- ADD a New Person/RelationShip To The DataBase By Biz Email   ----------------------------------
router.post('/addPerson', function (req, res) {
  var session = driver.session();

  var person = newPersonObj(req);      // create a new instance of the Person model

  var joined = new Date(); // Person join date is the current date/time of entry
  var mill = joined.getTime();

  person.joined = mill.toString(); // Join date is converted to milliseconds

  session
    .run("MATCH (b:Business {email: '" + req.body.bEmail.trim().toLowerCase() + "'}) Create (a:Person {name:'" + person.name + "', address:'" + person.address + "', phone:'" + person.phone + "', iceName:'" + person.iceName + "', icePhone:'" + person.icePhone + "', joined:'" + person.joined + "', dob:'" + person.dob + "', email:'" + person.email + "', imgUrl:'" + person.imgUrl + "', password:'" + person.password + "', visited:'" + person.visited + "', membership:'" + person.membership + "', publicImgId:'" + person.publicImgId + "', guardianName:'" + person.guardianName + "', guardianNum:'" + person.guardianNum + "'}) CREATE (a)-[:IS_A_MEMBER]->(b)-[:HAS_A_MEMBER]->(a) RETURN COUNT(*)")
    .then(function () {
      console.log("Person created");
      res.json({ message: 'Person created!', success: true });
      session.close();
    })
    .catch(function (error) {
      console.log(error);
      var s = error.fields[0].message;
      if (s.includes("already exists")) {
        res.json({ message: "Person Already Exists", success: false });
      }
      else {
        res.send(error);
      }
    })
})//addPerson


//----------------------------------- DELETE A Business BY EMAIL  -------------------------- *FIX* --------
//-------------> Should probably add an extra authorisation step for deletion to avoid mistakes! <---------
router.delete('/deleteCompany', function (req, res) {
  var session = driver.session();
  var email = req.body.email.trim().toLowerCase();        //Set the Business email (comes from the request)
  //query checks for business, deletes business, all of it's relationships and its members
  session.run("MATCH (b:Business {email:'" + email + "'}) OPTIONAL MATCH (b)-[r]-(p) DETACH DELETE b, r, p return COUNT(*)")

    .then(function (result) {

      // IF count(*) Returns > 0, Entry has been made
      if (result.records.length != 0)
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


//----------------------------------- POST Find Persons - Arivals WebApp ----------------------------------
//Match (a:Person)-[r:IS_A_MEMBER]->(b:Business) WHERE a.email='" + req.body.email.trim() + "' AND b.email='" + req.body.bEmail.trim()  + "' Return a, b LIMIT 1
router.post('/findPerson', function (req, res) {
  console.log("in findPerson ");
  var session = driver.session();
  var visitedList;
  session
    .run("Match (a:Person)-[r:IS_A_MEMBER]->(b:Business) WHERE a.email='" + req.body.email.trim().toLowerCase() + "' AND b.email='" + req.body.bEmail.trim() + "' Return a, b LIMIT 1")
    .then(function (result) {

      // IF count(*) Returns > 0, Updating has been made successfully
      if (result.records[0] != null) {
        //res.json({ success: true, message: 'Person Found!' });

        //Loop over Results, should always just be 1 returned
        result.records.forEach(function (record) {
          //If its A PERSON logging In
          console.log("Dates Visited: " + record._fields[0].properties.datesVisited);

          //Send the Response Back [List]
          res.json({
            success: true,
            name: record._fields[0].properties.name,
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
            visited: record._fields[0].properties.visited,
            membership: record._fields[0].properties.membership,
            publicImgId: record._fields[0].properties.publicImgId,

            datesVisited: record._fields[0].properties.datesVisited


          });
        })
      }

      else {
        res.json({ success: false, message: 'Cannot Access Unassociated Members' });
        console.log("Cannot Access Unassociated Members");
      }


      session.close();//close the session
      driver.close();////close driver
    })
    .catch(function (err) {
      console.log(err);
      res.json({ success: false, message: err });
    })
})


//----------------------------------- DELETE A Person ----------------------------------------------
router.put('/deletePerson', function (req, res) {

  var session = driver.session();

  session
    .run("Match (a:Person)-[r:IS_A_MEMBER]->(b:Business) WHERE a.email='" + req.body.email + "' AND b.email='" + req.body.bEmail + "' OPTIONAL MATCH (a)-[r]-(b) DETACH DELETE a,r Return a, b LIMIT 1")
    .then(function (result) {
      // IF count(*) Returns > 0, Entry has been made
      if (result.records[0] != null)
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


//----------------------------------- UPDATE A Person -----------------------------------------------------
router.put('/updatePerson', function (req, res) {

  var session = driver.session();
  console.log(req.body.datesVisited);
  session
    .run("Match (a:Person) WHERE a.email='" + req.body.email.trim().toLowerCase() + "' SET a.name='" + req.body.name.trim() + "', a.address='" + req.body.address.trim() + "', a.membership='" + req.body.membership + "', a.phone='" + req.body.phone + "', a.iceName='" + req.body.iceName.trim() + "', a.guardianName='" + req.body.guardianName + "', a.guardianNum='" + req.body.guardianNum + "', a.icePhone='" + req.body.icePhone + "', a.joined='" + req.body.joined + "', a.visited='" + req.body.visited + "', a.datesVisited=" + req.body.datesVisited + ", a.dob='" + req.body.dob + "', a.imgUrl='" + req.body.imgUrl + "', a.email='" + req.body.tempEmail.toLowerCase() + "' return COUNT(*)")
    .then(function (result) {

      // IF count(*) Returns > 0, Updating has been made successfully
      if (result.records[0] != null)
        res.json({ success: true, message: 'User Details Updated' });
      else
        res.json({ success: false, message: 'Problem Updating User, Check Name/Email' });

      session.close();
      //driver.close();
    })
    .catch(function (error) {
      res.json({ success: false, message: "Email Already In Use" });
    })
})//updateperson


//----------------------------------- SET NEW PASSWORD (FOR FIRST LOG IN) -------------------------------- 
router.put('/newPassword', function (req, res) {

  var session = driver.session();

  session
    .run("Match (a:Person) WHERE a.email='" + req.body.email + "' SET a.password='" + req.body.password + "' return COUNT(*)")
    .then(function (result) {

      // IF count(*) Returns > 0, Updating has been made successfully
      if (result.records.length > 0)
        res.json({ success: true, message: 'User Password Updated' });
      else
        res.json({ success: false, message: 'Problem Updating User Check Name/Email' });

      session.close();
      //driver.close();
    })
    .catch(function (error) {
      console.log(error);
      res.json({ success: false, message: err });
    })
})//newPassword


//----------------------------------- Create A New Person Object From Http Request  -----------------------
function newPersonObj(req) {
  var person = new Person();
  person.name = req.body.name.trim();
  person.address = req.body.address.trim();
  person.phone = req.body.phone;

  person.iceName = req.body.iceName.trim();
  person.icePhone = req.body.icePhone;

  person.dob = req.body.dob;
  person.email = req.body.email.trim().toLowerCase();
  person.password = req.body.password.trim();
  person.imgUrl = req.body.imgUrl;

  person.visited = "0";
  person.membership = "0";
  person.publicImgId = req.body.publicImgId;

  person.guardianName = null;
  person.guardianNum = null;

  //Check to see if the person was under 18 and Added a Guardian Name
  if (req.body.guardianName != null)
    person.guardianName = req.body.guardianName;

  //Check to see if the person was under 18 and Added a Guardian Num
  if (req.body.guardianNum != null)
    person.guardianNum = req.body.guardianNum;

  return person;
}


//----------------------------------- GET Top 10 Visited  --------------------------------------------------
router.post('/topTenVisited', function (req, res) {

  var queryTopTen = "Match (a:Person)-[r:IS_A_MEMBER]->(b:Business) WHERE b.email='" + req.body.bEmail + "' WITH a ORDER BY a.lastvisited DESC Return a LIMIT 10";
  var session = driver.session();//Create a new session
  session.run(queryTopTen)
    .then(function (result) {
      var topTenList = [];//create a new list
      var obj = new Person();

      result.records.forEach(function (record) {//Iterate over results
        topTenList.push(record._fields[0].properties)//Add The business To a list
      });

      res.json({ success: true, message: topTenList });//send the bizList as a response

      session.close();//close the session
    })
    .catch(function (error) {
      console.log(error);
      res.json({ success: false, message: "Something Happened" });//send the bizList as a response
    });
});


//----------------------------------- Returns All People associated with The Business --------------------------------------------------
router.post('/visitedTotal', function (req, res) {

  var queryTopTen = "Match (a:Person)-[r:IS_A_MEMBER]->(b:Business) WHERE b.email='" + req.body.bEmail + "' Return a";
  var session = driver.session();//Create a new session
  session.run(queryTopTen)
    .then(function (result) {
      var topTenList = [];//create a new list
      var obj = new Person();

      result.records.forEach(function (record) {//Iterate over results
        topTenList.push(record._fields[0].properties)//Add The business To a list
      });

      res.json({ success: true, message: topTenList });//send the bizList as a response

      session.close();//close the session
      //driver.close();////close driver
    })
    .catch(function (error) {
      console.log(error);
      res.json({ success: false, message: "Something Happened" });//send the bizList as a response
    });
});


//---------------------------------- REGISTER OUR ROUTES --------------------------------------------------
// All of our routes will be prefixed with /api
app.use('/api', router);


// Server listens on either Local OR Heroku port
//=============================================================================
app.listen(port);
console.log('Magic happens on port ' + port);