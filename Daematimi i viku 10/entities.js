const mongoose = require("mongoose");

const UsersSchema = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    token: String,
    gender: {
        type: String,
        validate: {
            validator: function(value) {
                if(value === "m" || value === "f" || value === "o") {
                    return true;
                } else {
                    return false;
                }
            }
        }
    }
});

const PunchSchema = new mongoose.Schema({
    created: {
        type: Date,
        default: new Date()    
    },
    user_id: {
        type: String,
        required: true
    },
    company_id: {
        type: String,
        required: true
    },
    used: {
        type: Boolean,
        default: false
    }
});

const CompanySchema = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    punchCount: {
        type: Number,
        default: 10
    }
})

const UserEntity = mongoose.model("Users", UsersSchema);
const PunchEntity = mongoose.model("Punches", PunchSchema);
const CompanyEntity = mongoose.model("Companies", CompanySchema);

const entities = {
    User: UserEntity,
    Punch: PunchEntity,
    Company: CompanyEntity
};

module.exports = entities;