const mongose = require("mongoose");

const CompanySchema = new mongose.Schema({
    name: {
        type: String,
        required: true
    },
    punchCount: {
        type: Number,
        default: 10
    },
    description: {
        type: String,
        required: true
    }
    
});

const CompanyEntity = mongose.model("Companies", CompanySchema);

const compEntity = {
    Company: CompanyEntity
}

module.exports = compEntity;