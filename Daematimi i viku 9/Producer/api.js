const entities = require("./entities");
const producer = require('./producer');
const express = require("express");
const uuid = require("node-uuid");
const bodyParser = require("body-parser");

const app = express();
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extend: true
}));

const adminToken = "lol";

app.get("/users", (req, res) => {
    entities.User.find(function (err, docs) {
        if (err) {
            res.status(500).send(err);
        } else {
            var noToken = [];

            for (var i in docs) {
                noToken.push({
                    id: docs[i].id,
                    name: docs[i].name,
                    gender: docs[i].gender,
                });
            }

            res.status(200).send(noToken);
        }
    });
});

app.post("/users", (req, res) => {
    if (req.headers.authorization !== adminToken) {
        res.status(401).send("Unauthorized");
        return;
    }

    var data = {
        name: req.body.name,
        gender: req.body.gender,
        token: uuid.v1()
    };

    var userEntity = new entities.User(data);
    userEntity.save(function (err) {
        if (err) {
            res.status(412).send(err);
            return;
        } else {
            producer.send('punchcardApi.addUser', data);
            res.status(201).send({
                id: userEntity.id,
                token: data.token
            });
            return;
        }
    });
});

app.get("/companies", (req, res) => {
    entities.Company.find(function (err, docs) {
        if (err) {
            res.status(500).send(err);
        } else {
            res.status(200).send(docs);
        }
    });
});

app.get("/companies/:id", (req, res) => {
    var data = {
        id: req.params.id
    };

    entities.Company.find(data, function (err, docs) {
        if (err) {
            res.status(404).send("No company found");
        } else {
            res.status(200).send(docs);
        }
    });
});

app.post("/companies", (req, res) => {
    if (req.headers.authorization !== adminToken) {
        res.status(401).send("Unauthorized");
        return;
    }

    var data = {
        name: req.body.name,
        punchCount: req.body.punchCount
    };

    var companyEntity = new entities.Company(data);

    companyEntity.save(function (err) {
        if (err) {
            res.status(412).send(err);
            return;
        } else {
            producer.send('punchcardApi.getPunch', data);
            res.status(201).send({
                companyId: companyEntity.id,
            });
            return;
        }
    })
});

app.post("/punches", (req, res) => {
    var companyId = req.body.companyId;
    var userId = req.body.userId;
    var punches;

    var data = {
        token: req.headers.authorization
    };

    entities.User.find(data, function (err, docs) {
        if (err) {
            res.status(404).send("No user found");
            return;
        } else {

            for(var i in docs) {
                userId = docs[i].id;
            }

            var data1 = {
                id: companyId
            };

            entities.Company.find(data1, function (err, docs) {
                if (err) {
                    res.status(404).send("No company found");
                    return;
                } else {
                    var data2 = {
                        userId: userId,
                        companyId: companyId,
                        used: false
                    };
                    entities.Punch.find(data2, function (err, docs) {
                        if (err) {
                            res.status(500).send(err);
                            return;
                        } else {
                            punches = Object.keys(docs).length;

                            if (punches >= 0) {
                                for (var i in docs) {
                                    docs[i].used = true;
                                    docs[i].save(function (err) {
                                        if (err) {
                                            res.status(412).send(err);
                                            return;
                                        }
                                    });
                                }
                                producer.send('punchcardApi.addDiscount', data2);
                                res.status(201).send({
                                    discount: true
                                });
                            } else {
                                var data3 = {
                                    user_id: userId,
                                    company_id: companyId
                                }

                                var entity = new entities.Punch(data3);
                                entity.save(function (err) {
                                    if (err) {
                                        res.status(412).send(err);
                                        return;
                                    } else {
                                        producer.send('punchcardApi.addPunch', data3);
                                        res.status(201).send({
                                            id: entity.id
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