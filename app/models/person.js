
//Mongoose used for validation on Input to Object
var mongoose = require('mongoose');
var Schema = mongoose.Schema;


var PersonSchema = new Schema({
    name: String,
    address: String,
    phone: String,
    iceName: String,
    icePhone: String,
    joined: String,
    dob: String,
    visited: String,
    lastVisited: String,
    membership: String,
    publicImgId: String,
    imgUrl:{
        type:String,
        unique: true,
        required: true,
        },
    email: {
        type: String,
        unique: true,
        required: true
    },
    guardianName:String,
    guardianNum: String,
    bEmail: String,

    password: {
        type: String,
        required: true
    }
});

module.exports = mongoose.model('Person', PersonSchema);

