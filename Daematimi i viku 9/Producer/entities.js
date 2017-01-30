const mongoose = require("mongoose");

const User = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    token: String,
    gender: String,
    created: {
        type: Date,
        default: new Date()
    }
});

const Punch = new mongoose.Schema({
    userId: {
        type: String,
        required: true
    },
    created: {
        type: Date,
        default: new Date()    
    },
    companyId: {
        type: String,
        required: true
    },
    used: {
        type: Boolean,
        default: false
    }
});

const Company = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    punchCount: {
        type: Number,
        default: 10
    }
});

const UserEntity = mongoose.model("User", User);
const PunchEntity = mongoose.model("Punch", Punch);
const CompanyEntity = mongoose.model("Company", Company);

const entities = {
    User: UserEntity,
    Punch: PunchEntity,
    Company: CompanyEntity
};

module.exports = entities;