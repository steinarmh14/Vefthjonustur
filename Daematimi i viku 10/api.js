const express = require("express");
const app = express();
const entities = require("./entities");
const uuid = require("node-uuid");
const producer = require('./producer');
const bodyParser = require("body-parser");
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extend: true
}));

const adminToken = "smuuu";

app.get("/companies", (req, res) => {
    entities.Company.find(function(err, docs) {
        if(err) {
            res.status(500).send(err);
        } else {
            res.status(200).send(docs);
        }
    });
});

app.get("/companies/:id", (req, res) => {
    var query = {
        _id: req.params.id
    };

    entities.Company.find(query, function(err, docs) {
        if(err) {
            res.status(404).send("Company not found.");
        } else {
            res.status(200).send(docs);
        }
    });
});

app.post("/companies", (req, res) => {
    if(req.headers.authorization !== adminToken) {
        res.status(401).send("Not authorized");
        return;
    }

    var data = {
        name: req.body.name,
        punchCount: req.body.punchCount
    };

    var entity = new entities.Company(data);
    entity.save(function(err) {
        if(err) {
            res.status(412).send(err);
            return;
        } else {
                producer.send('punchapi.getPunch',msgObject);
            res.status(201).send({
                company_id: entity._id,
            });
            return;
        }
    })
});

app.get("/users", (req, res) => {
    entities.User.find(function(err, docs) {
        if(err) {
            res.status(500).send(err);
        } else {
            var docsWithoutToken = [];

            for(var i in docs) {
                docsWithoutToken.push({
                    _id: docs[i]._id,
                    name: docs[i].name,
                    gender: docs[i].gender
                });
            }

            res.status(200).send(docsWithoutToken);
        }
    });
});

app.post("/users", (req, res) => {
    if(req.headers.authorization !== adminToken) {
        res.status(401).send("Not authorized");
        return;
    }

    var data = {
        name: req.body.name,
        gender: req.body.gender,
        token: uuid.v1()
    };

    var entity = new entities.User(data);
    entity.save(function(err) {
        if(err) {
            res.status(412).send(err);
            return;
        } else {
            producer.send('punchapi.addUser',msgObject);
            res.status(201).send({
                _id: entity._id,
                token: data.token
            });
            return;
        }
    });
});

app.post("/my/punches", (req, res) => {
    //var companyId = parseInt(req.body.company_id);
    var companyId = req.body.company_id;
    var userId;
    var numOfPunchesNeeded;
    var numOfPunches;

    var userQuery = {
        token: req.headers.authorization
    };
    entities.User.find(userQuery, function(err, docs) {
        if(err) {
            res.status(401).send("No user with this id found.");
            return;
        } else {
            userId = docs[0]._id;

            var companyQuery = {
                _id: companyId
            };
            entities.Company.find(companyQuery, function(err, docs) {
                if(err) {
                    res.status(404).send("No company with this id found.");
                    return;
                } else {
                    numOfPunchesNeeded = docs[0].punchCount;

                    var punchQuery = {
                        user_id: userId,
                        company_id: companyId,
                        used: false
                    };
                    entities.Punch.find(punchQuery, function(err, docs) {
                        if(err) {
                            res.status(500).send("PunchFind: " + err);
                            return;
                        } else {
                            numOfPunches = Object.keys(docs).length;

                            if(numOfPunches >= numOfPunchesNeeded) {
                                // Change punches to used.
                                for(var i in docs) {
                                    docs[i].used = true;
                                    docs[i].save(function(err) {
                                        if(err) {
                                            res.status(412).send("Save: " + err);
                                            return;
                                        }
                                    });
                                }
                                producer.send('punchapi.discount', msgObject);
                                res.status(201).send({
                                    discount: true
                                });
                            } else {
                                // Add new punch
                                var data = {
                                    user_id: userId,
                                    company_id: companyId
                                }
                                var entity = new entities.Punch(data);
                                entity.save(function(err) {
                                    if(err) {
                                        res.status(412).send("Save2: " + err);
                                        return;
                                    } else {
                                        producer.send('punchapi.addpunch', msgObject);
                                        res.status(201).send({
                                            _id: entity._id
                                        });
                                    }
                                });
                            }
                        }
                    });
                }
            });
        }
    });
});

module.exports = app;