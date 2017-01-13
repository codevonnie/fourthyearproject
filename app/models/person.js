var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var PersonSchema = new Schema({
    name: String,
    address: String,
    phone: Number,
    iceName: String,
    icePhone: Number,
    joined: Number,
    gender: String,
    dob: Number,
    email: {
        type: String,
        unique: true,
        required: true
    },
    guardianName:String,
    guardianNum:Number,

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
