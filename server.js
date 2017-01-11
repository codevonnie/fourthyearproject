//Variables
var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var neo4j = require('neo4j-driver').v1;
var port = process.env.PORT || 8080;        // set our port
var morgan = require('morgan');
//var jwt = require('jwt-simple');
var config = require('./config/database');
var cors = require('cors');
var passport = require('passport');
var mongoose = require('mongoose');
var jwt = require('express-jwt');
var request = require("request");
 
var Person = require('./app/models/person');
var Business = require('./app/models/business');

var driver = neo4j.driver("bolt://hobby-gemhpbboojekgbkeihhpigol.dbs.graphenedb.com:24786", neo4j.auth.basic("app57975900-aEgAtX", "tGm6FwOKgU7sQyPDUACj"));


// Get our request parameters
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());
app.use(cors());
app.use(morgan('dev'));

var jwtCheck = jwt({
  secret: 'jfeiosIJDL938jiJ0234Djbxm9Gfysd1ddLy',
  audience: 'Az124jileIjfefneig89IUfjesu'
});

app.use('/addCompany', jwtCheck);

 //app.use(passport.initialize());
   
 //require('./config/passport')(passport);

var router = express.Router();
//var db = mongoose.connect(config.database);

// middleware to use for all requests
app.all('*', function (req, res, next) {
  // do logging
  res.header("Access-Control-Allow-Origin", "*");
  res.header('Access-Control-Allow-Methods', 'PUT, GET, POST, DELETE, OPTIONS');
  res.header("Access-Control-Allow-Headers", "Content-Type, Accept");

  console.log('Something is happening.');
  next(); // make sure we go to the next routes and don't stop here
});


// AUTHENTICATION STUFF
// route to authenticate a person (POST http://localhost:8080/api/authenticate)
/*
router.post('/authenticate', function(req, res) {
  
  console.log('I am authenticating');
  
  var session = driver.session();

  var person = new Person();  
  person.email = req.body.email;
  person.password = req.body.password;
  console.log(person.email);
  console.log(person.password);

  session
    .run("Match (a:Person) WHERE a.email='" + person.email + "' AND a.password='" + person.password+"' Return a")

    .then(function (result) {
      // if person is found and password is right
        // create a token
        if(result.records[0]==null){
          res.json({ success: false, message: 'Authentication failed.' });
        }
        else{
          var result=result.records;
          console.log(result)
          var token = jwt.encode(person, config.secret);
          
          // return the information including token as JSON
          res.json({
            success: true,
            message: 'Enjoy your token!',
            token: token,
          })
        }
        
        
    })
    .catch(function (error) {
      console.log(error);
      res.json({ success: false, message: 'Authentication failed.' });
    });
});

// secure route
router.get('/memberinfo', passport.authenticate('jwt', { session: false}), 
function(req, res) {
  console.log("IT'S ME");
  var token = getToken(req.headers);
  console.log("in memberinfo");
  if (token) {
    var decoded = jwt.decode(token, config.secret);
    console.log(decoded);
    Person.findOne({
      email: decoded.email
    }, function(err, person) {
        if (err) throw err;
 
        if (!person) {
          return res.status(403).send({success: false, msg: 'Authentication failed. User not found.'});
        } else {
          res.json({success: true, msg: 'Welcome in the member area ' + person.name + '!'});
        }
    });
  } else {
    return res.status(403).send({success: false, msg: 'No token provided.'});
  }
});
 
getToken = function (headers) {
  console.log("gettoken func");
  if (headers && headers.authorization) {
    var parted = headers.authorization.split(' ');
    if (parted.length === 2) {
      return parted[1];
    } else {
      return null;
    }
  } else {
    return null;
  }
};
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
router.get('/businessMembers', function (req, res) {
  console.log("in business members");
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

router.post('/addperson', function(req, res) {
        
        var session = driver.session();
        var person = new Person();      // create a new instance of the Person model
        person.name = req.body.name;
        person.address = req.body.address;
        person.phone = req.body.phone;
        person.icename = req.body.icename;
        person.icephone = req.body.icephone;
        var joined = new Date(req.body.joined);
        person.joined = joined.getTime();
        person.gender =  req.body.gender;
        var dob =  new Date(req.body.joined);
        person.dob = dob.getTime();
        person.email = req.body.email;
        person.password = req.body.password;
       
        //add Person 
        session
              .run("Merge (a:Person {name:'" + person.name + "', address:'" + person.address + "', phone:'" + person.phone + "', icename:'" + person.icename + "', icephone:'" + person.icephone + "', joined:'" + person.joined + "', gender:'" + person.gender + "', dob:'" + person.dob + "', email:'" + person.email + "', password:'" + person.password + "'})")
           
        .then( function()
        {
          console.log( "Person created" );
          res.json({ message: 'Person created!' });
          session.close();
          //driver.close();
      })
      .catch(function(error) {
          console.log(error);
          res.send(error);
    })

      
    })//addPerson

router.post('/addrelationship', function(req, res) {

    var session = driver.session();
    var person = new Person();      // create a new instance of the Person model
    person.name = req.body.name;
    var business = new Business();
    business.bname = req.body.bname;
   
    session.run( "MATCH (a:Person {name: '"+person.name+"'}), (b:Business {name: '"+business.bname+"'}) CREATE (a)-[r:MEMBER_OF]->(b)")
    session.run( "MATCH (a:Person {name: '"+person.name+"'}), (b:Business {name: '"+business.bname+"'}) CREATE (b)-[r:HAS_MEMBER]->(a)")
   
    .then( function()
    {
    console.log( "Person->Business relationship created" );
    res.json({ message: 'Person->Business relationship created!' });
    session.close();
   // driver.close();
  })
  .catch(function(error) {
    console.log(error);
    res.send(error);
  })
})//addrelationship

router.delete('/deleteperson', function(req, res) {

    var session = driver.session();
    var person = new Person();      // create a new instance of the Person model
    person.name = req.body.name;
       
    session
    .run( "Match (a:Person) WHERE a.name='"+person.name+"' DETACH DELETE a" )
    .then( function()
    {
    console.log( "Person deleted" );
    res.json({ message: 'Person deleted!' });
    session.close();
    //driver.close();
  })
  .catch(function(error) {
    console.log(error);
    res.send(error);
  })
})//deleteperson

router.put('/updateperson', function(req, res) {

    var session = driver.session();
    var person = new Person();      // create a new instance of the Person model
    person.name = req.body.name;
    person.address = req.body.address;
    person.phone = req.body.phone;
    person.icename = req.body.icename;
    person.icephone = req.body.icephone;
    var joined = new Date(req.body.joined);
    person.joined = joined.getTime();
    person.gender =  req.body.gender;
    var dob =  new Date(req.body.joined);
    person.dob = dob.getTime();
    person.email = req.body.email;
    person.password = req.body.password;
       
    session
    .run( "Match (a:Person) WHERE a.name='"+person.name+"' SET a.name='"+person.name+"', a.address='"+person.address+"', a.phone="+person.phone+", a.icename='"+person.icename+"', a.icephone="+person.icephone+", a.joined='"+person.joined+"', a.gender='"+person.gender+"', a.dob="+person.dob+", a.email='"+person.email+"', a.password='"+person.password+"'")
    .then( function()
    {
    console.log( "Person updated" );
    res.json({ message: 'Person updated!' });
    session.close();
    //driver.close();
  })
  .catch(function(error) {
    console.log(error);
    res.send(error);
  })
})//updateperson



// REGISTER OUR ROUTES -------------------------------
// all of our routes will be prefixed with /api
app.use('/api', router);

// START THE SERVER
// =============================================================================
app.listen(port);
console.log('Magic happens on port ' + port);