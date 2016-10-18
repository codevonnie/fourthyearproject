//Variables
var express     = require('express');
var app         = express();
var bodyParser  = require('body-parser');
var neo4j = require('neo4j-driver').v1;
var port = process.env.PORT || 8080;        // set our port
var morgan = require('morgan');
var jwt = require('jsonwebtoken');
var config = require('./config');
var Person = require('./app/models/person');
var Business = require('./app/models/business');

var driver = neo4j.driver("bolt://hobby-gemhpbboojekgbkeihhpigol.dbs.graphenedb.com:24786", neo4j.auth.basic("app57975900-aEgAtX", "tGm6FwOKgU7sQyPDUACj"));


// get our request parameters
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

var mongoose   = require('mongoose');
mongoose.connect(config.database);
app.set('superSecret', config.secret);

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



    router.post('/addCompany', function(req, res) {
        
        var business = new Business();      // create a new instance of the Person model
        business.name = req.body.name;  // set the persons name (comes from the request)
        business.address = req.body.address;
        business.phone = req.body.phone;
        business.email = req.body.email;
        business.password = req.body.password;

        //add Company 
        session
        .run( "Merge (b:Business {name:'"+business.name+"', address:'"+business.address+"', phone:'"+business.phone+"', email:'"+business.email+"', password:'"+business.email+"'})" )
        
        business.save(function(err) {
        if (err)
            res.send(err);

        res.json({ message: 'Company created!' });
        
        });
            /*.then( function()
            {
                console.log( "Company created successfully" );
                session.close();
                driver.close();
            })
            .catch(function(error) {
                console.log(error);
            });*/


        
        
    })//addCompany

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
          .run( "Merge (a:Person {name:'"+person.name+"', address:'"+person.address+"', phone:"+person.phone+", icename:'"+person.icename+"', icephone:"+person.icephone+", joined:"+person.joined+", gender:'"+person.gender+"', dob:'"+person.dob+"', email:'"+person.email+"', password:'"+person.password+"'})" )
           
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


/* MongoDB routes        
router.route('/persons')

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

*/

// REGISTER OUR ROUTES -------------------------------
// all of our routes will be prefixed with /api
app.use('/api', router);

// START THE SERVER
// =============================================================================
app.listen(port);
console.log('Magic happens on port ' + port);




