var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var PersonSchema = new Schema({
    name: String,
    address: String,
    phone: String,
    iceName: String,
    icePhone: String,
    joined: Number,
    dob: Number,
    imgUrl:{
        type:String,
        unique: true,
        //required: true
        },
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
