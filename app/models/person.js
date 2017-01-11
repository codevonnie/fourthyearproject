var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var PersonSchema = new Schema({
    name: String,
    address: String,
    phone: Number,
    icename: String,
    icephone: Number,
    joined: Number,
    gender: String,
    dob: Number,
    email: {
        type: String,
        unique: true,
        required: true
    },
    password: {
        type: String,
        required: true
    }
});



module.exports = mongoose.model('Person', PersonSchema);


/*
    Schema types
    String
    Number
    Date
    Buffer
    Boolean
    Mixed
    ObjectId
    Array
*/
