//Variables
var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var neo4j = require('neo4j-driver').v1;
var port = process.env.PORT || 8080;        // set our port
var morgan = require('morgan');
var jwt = require('jsonwebtoken');
var config = require('./config');
var Person = require('./app/models/person');
var Business = require('./app/models/business');

var driver = neo4j.driver("bolt://hobby-gemhpbboojekgbkeihhpigol.dbs.graphenedb.com:24786", neo4j.auth.basic("app57975900-aEgAtX", "tGm6FwOKgU7sQyPDUACj"));


// Get our request parameters
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

/*
var session = driver.session();
session
  .run( "CREATE (a:Person {name:'Arthur', title:'King'})" )
  .then( function()
  {
session.run( "MATCH (a:Person) WHERE a.name = 'Arthur' RETURN a.name AS name, a.title AS title" )
  })
  .then( function( result ) {
    console.log( result.records[0].get("title") + " " + result.records[0].get("name") );
    console.log( result.records[1].get("title") + " " + result.records[1].get("name") );
    console.log( result.records[2].get("title") + " " + result.records[2].get("name") );
    session.close();
    driver.close();
  })
*/



/*
var name = "Bart Simpson";
var address = "124 Evergreen Terrace";
var phone = "0871409512";
var icename = "Marge Simpson";
var icephone = "0878274541";
//var joined = +new Date("2012-02-10T13:19:11+0000");
var myDate = new Date("2012-02-10T13:19:11+0000");
var joined = myDate.getTime();
var gender = "m";
var dob = "20/4/2001";
var email = "bs@email.com";
var password = "mypass";

//add Person
session
  .run( "Merge (a:Person {name:'"+name+"', address:'"+address+"', phone:'"+phone+"', icename:'"+icename+"', icephone:'"+icephone+"', joined:"+joined+", gender:'"+gender+"', dob:'"+dob+"', email:'"+email+"', password:'"+password+"'})" )
  .then( function()
  {
    console.log( "Person created successfully" );
    session.close();
    driver.close();
  })
  .catch(function(error) {
    console.log(error);
  });

session.run( "MATCH (a:Person {name: '"+name+"'}), (b:Business {name: '"+bname+"'}) CREATE (a)-[r:MEMBER_OF]->(b)")
.then( function()
  {
    console.log( "Person->Business relationship created" );
    session.close();
    driver.close();
  })
  .catch(function(error) {
    console.log(error);
  });

session.run( "MATCH (a:Person {name: '"+name+"'}), (b:Business {name: '"+bname+"'}) CREATE (b)-[r:HAS_MEMBER]->(a)")
.then( function()
  {
    console.log( "Business->Person relationship created" );
    session.close();
    driver.close();
  })
  .catch(function(error) {
    console.log(error);
  });

/*
  var deleteuser = "Fred Flintstone";


  //delete Person
session
  .run( "Match (a:Person) WHERE a.name='"+deleteuser+"' DETACH DELETE a" )
  .then( function()
  {
    console.log( "Person deleted successfully" );
    session.close();
    driver.close();
  })
  .catch(function(error) {
    console.log(error);
  });

*/



app.use(morgan('dev'));

var router = express.Router();

// middleware to use for all requests
router.use(function (req, res, next) {
  // do logging
  console.log('Something is happening.');
  next(); // make sure we go to the next routes and don't stop here
});

/* AUTHENTICATION STUFF

// route to authenticate a person (POST http://localhost:8080/api/authenticate)
router.post('/authenticate', function(req, res) {

  // find the person
  Person.findOne({
    email: req.body.email
  }, function(err, person) {

    if (err) throw err;

    if (!person) {
      res.json({ success: false, message: 'Authentication failed. Person not found.' });
    } else if (person) {

      // check if password matches
      if (person.password != req.body.password) {
        res.json({ success: false, message: 'Authentication failed. Wrong password.' });
      } else {

        // if person is found and password is right
        // create a token
        var token = jwt.sign(person, app.get('superSecret'), {
          expiresIn: 1440 
        });

        // return the information including token as JSON
        res.json({
          success: true,
          message: 'Enjoy your token!',
          token: token
        });
      }   

    }

  });
});

// route middleware to verify a token
router.use(function(req, res, next) {

  // check header or url parameters or post parameters for token
  var token = req.body.token || req.query.token || req.headers['x-access-token'];

  // decode token
  if (token) {

    // verifies secret and checks exp
    jwt.verify(token, app.get('superSecret'), function(err, decoded) {      
      if (err) {
        return res.json({ success: false, message: 'Failed to authenticate token.' });    
      } else {
        // if everything is good, save to request for use in other routes
        req.decoded = decoded;    
        next();
      }
    });

  } else {

    // if there is no token
    // return an error
    return res.status(403).send({ 
        success: false, 
        message: 'No token provided.' 
    });
    
  }
});

*/



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
    .run("Merge (b:Business {name:'" + business.name + "', address:'" + business.address + "', phone:'" + business.phone + "', email:'" + business.email + "', password:'" + business.email + "'})")

    .then(function () {
      console.log("Business created");
      res.json({ message: 'Business created!' });
      session.close();
      driver.close();
    })
    .catch(function (error) {
      console.log(error);
      res.send(error);
    });
});


//-----------------------------------    DELETE A Business BY NAME  -------------------------------- 
router.delete('/deleteCompany', function (req, res) {
  var session = driver.session();
  var name = req.body.name;         //Set the Business name (comes from the request)

  session.run("MATCH (b:Business {name:'" + name + "'}) DELETE b")
    .then(function () {
      console.log("Business Deleted");
      res.json({ message: 'Business Deleted!' });
      session.close();
      driver.close();
    })
    .catch(function (error) {
      console.log(error);
      res.send(error);
    });
});



/*-----------------------------------    GET ALL Business   -------------------------------- 
* GET Request returns all the Buisness Nodes and sends them all as a JSON response to the client
*/
router.get('/BuisinessMembers', function (req, res) {
  var session = driver.session();//Create a new session
  session.run('MATCH (a:Business) RETURN a LIMIT 25')
    .then(function(result) {
      var bizList = [];//create a new list
      result.records.forEach(function(record){//Iterate over results
       console.log(record._fields[0].properties);//log results
       bizList.push(record._fields[0].properties)//Add The business To a list
      });
      res.json({ message: bizList});//send the bizList as a response
      session.close();//close the session
      driver.close();////close driver
    })
    .catch(function (error) {
      console.log(error);
      res.send(error);
    });
});



//-----------------------------------    CREATE A RELATIONSHIP BETWEEN Business AND PERSON   -------------------------------- 
router.post('/createRelationship', function (req, res) {
  var session = driver.session();
  var name = req.body.name;         //Set the Business name (comes from the request)
  var bname = req.body.bName;

  session.run("MATCH (a:Person {name: '" + name + "'}), (b:Business {name: '" + bname + "'}) CREATE (a)-[r:IS_MY_ISLAND_HUSBAND]->(b)")
    .then(function () {
      console.log("Relationship created");
      res.json({ message: 'Relationship created!' });
      session.close();
      driver.close();
    })
    .catch(function (error) {
      console.log(error);
      res.send(error);
    });
});



















// REGISTER OUR ROUTES -------------------------------
// all of our routes will be prefixed with /api
app.use('/api', router);

// START THE SERVER
// =============================================================================
app.listen(port);
console.log('Magic happens on port ' + port);




