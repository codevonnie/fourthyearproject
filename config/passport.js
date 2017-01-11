var JwtStrategy = require('passport-jwt').Strategy,
    ExtractJwt = require('passport-jwt').ExtractJwt;

var Person = require('../app/models/person');
var config = require('../config/database');
 
module.exports = function(passport) {
var opts = {};
opts.jwtFromRequest = ExtractJwt.fromAuthHeader();
opts.secretOrKey = config.secret;
passport.use(new JwtStrategy(opts, function(jwt_payload, done) {
    Person.findOne({id: jwt_payload.id}, function(err, person) {
        if (err) {
            return done(err, false);
        }
        if (person) {
            done(null, person);
        } else {
            done(null, false);

            // or you could create a new account

        }
    });
}));
};