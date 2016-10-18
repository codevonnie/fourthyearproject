var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var BusinessSchema = new Schema({
    name: String,
    address: String,
    phone: Number,
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

module.exports = mongoose.model('Business', BusinessSchema);


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
