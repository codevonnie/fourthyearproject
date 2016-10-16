//Variables
var express     = require('express');
var app         = express();
var bodyParser  = require('body-parser');
//var neo4j = require('neo4j-driver').v1;
var port = process.env.PORT || 8080;        // set our port
var morgan = require('morgan');
var jwt = require('jsonwebtoken');
var config = require('./config');
var Person = require('./app/models/person');

//var driver = neo4j.driver("bolt://hobby-gemhpbboojekgbkeihhpigol.dbs.graphenedb.com:24786", neo4j.auth.basic("app57975900-aEgAtX", "tGm6FwOKgU7sQyPDUACj"));

var mongoose   = require('mongoose');
mongoose.connect(config.database);
app.set('superSecret', config.secret);

/*
var session = driver.session();
session
  .run( "CREATE (a:Person {name:'Arthur', title:'King'})" )
  .then( function()
  {
    return session.run( "MATCH (a:Person) WHERE a.name = 'Arthur' RETURN a.name AS name, a.title AS title" )
  })
  .then( function( result ) {
    console.log( result.records[0].get("title") + " " + result.records[0].get("name") );
    session.close();
    driver.close();
  })
*/

// get our request parameters
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

app.use(morgan('dev'));

var router = express.Router(); 

// middleware to use for all requests
router.use(function(req, res, next) {
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

router.route('/persons')

    // create a person (accessed at POST http://localhost:8080/api/persons)
    .post(function(req, res) {
        
        var person = new Person();      // create a new instance of the Person model
        person.name = req.body.name;  // set the persons name (comes from the request)
        person.address = req.body.address;
        person.phone = req.body.phone;
        person.icename = req.body.icename;
        person.icephone = req.body.icephone;
        person.joined = req.body.joined;
        person.gender = req.body.gender;
        person.dob = req.body.dob;
        person.email = req.body.email;
        person.password = req.body.password;
      
        // save the person and check for errors
        person.save(function(err) {
            if (err)
                res.send(err);

            res.json({ message: 'Person created!' });
        });
        
    })

    // get all persons (accessed at GET http://localhost:8080/api/persons)
    .get(function(req, res) {
        Person.find(function(err, person) {
            if (err)
                res.send(err);

            res.json(person);
        });
    });

    router.route('/persons/:person_id')

    // get the person with that id (accessed at GET http://localhost:8080/api/persons/:person_id)
    .get(function(req, res) {
        Person.findById(req.params.person_id, function(err, person) {
            if (err)
                res.send(err);
            res.json(person);
        });
    })

    .put(function(req, res) {

        // use our person model to find the person we want
        Person.findById(req.params.person_id, function(err, person) {

            if (err)
                res.send(err);

            person.name = req.body.name;  // update the persons info

            // save the person
            person.save(function(err) {
                if (err)
                    res.send(err);

                res.json({ message: 'Person updated!' });
            });

        });
    })

    // delete the person with this id (accessed at DELETE http://localhost:8080/api/persons/:person_id)
    .delete(function(req, res) {
        Person.remove({
            _id: req.params.person_id
        }, function(err, person) {
            if (err)
                res.send(err);

            res.json({ message: 'Successfully deleted' });
        });
    });



// REGISTER OUR ROUTES -------------------------------
// all of our routes will be prefixed with /api
app.use('/api', router);

// START THE SERVER
// =============================================================================
app.listen(port);
console.log('Magic happens on port ' + port);




