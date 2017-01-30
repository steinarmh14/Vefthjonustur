'use strict'
const express = require("express");
const app = express();
const compEntity = require("./companyEntity");
const uuid = require("node-uuid");
const bodyParser = require('body-parser');
const elasticsearch = require('elasticsearch');

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extend: true
}));

const adminToken = "admin";

const client = new elasticsearch.Client({
    host: 'localhost:9200',
    log: 'error'
});

app.post("/companies", (req, res) => {

    if (req.headers.authorization === adminToken) {

        var query = {
            name: req.body.name,
        }

        compEntity.Company.find(query, function (err, docs) {
            if (err) {
                res.status(500).send("Internal server error");
                return;
            } else if (docs[0] === undefined) {

                var data = {
                    name: req.body.name,
                    punchCount: req.body.punchCount,
                    description: req.body.description
                }

                var entity = new compEntity.Company(data);
                entity.save(function (err, docs) {
                    if (err) {
                        res.status(412).send("ERROR: " + err);
                        return;
                    } else {

                        client.index({
                            index: 'companies',
                            type: 'company',
                            id: String(entity._id),
                            body: {
                                name: data.name,
                                punchCount: data.punchCount,
                                description: data.description
                            }
                        }, function (error) {
                            if (error) {
                                res.status(500).send("Internal server error");
                            } else {
                                res.status(201).send({
                                    company_id: entity._id
                                });
                            }
                        });
                    }
                });
            } else {
                res.status(409).send("This company already exist");
                return;
            }
        });
    } else {
        res.status(401).send("You need to be a admin to post!")
    }
});


app.get("/companies/:id", (req, res) => {

    var query = compEntity.Company.findOne({ '_id': req.params.id });

    query.exec(function (err, docs) {
        if (err) {
            res.status(403).send("Company not found")
        } else {
            res.status(200).send({
                name: docs.name,
                punchCount: docs.punchCount,
                description: docs.description
            });

        }
    });
});

app.get("/companies", (req, res) => {

    /*
        Ég skildi eftir commentaðan kóða neðst í fallinu til að sjá all companies í gegnum mongodb ef þu villt sjá hvort þau fara ekki öll í databaseinn
        annars virkar þetta ekki alveg hjá mér væri gaman að fá feedback einhverntíman hvað ég þarf að laga svo ég geti fixað í jólafríinu
    */

    var page = req.query.page;
    if (page === null || typeof page === "undefined") {
        page = 0;
    }

    var max = req.query.max;
    if (max === null || typeof max === "undefined") {
        max = 20;
    }

    var searching = req.query.search;
    if (searching === null || searching === "undefined") {
        searching = "";

        Client.search({
            index: 'companies',
            type: 'company',
            size: max,
            from: page * max,
            body: {
                sort: {
                    'name.keyword': { order: asc }
                },
                query: {
                    match_all: {}
                }
            }
        }, function (error) {
            if (error) {
                res.status(500).send("Internal server error");
            } else {
                res.status(200).send({
                    company_id: entity._id,
                    name: entity.name

                });
            }
        }).then(function (body) {
            var hits = body.hits.hits;

            res.status(200).send({
                company_id: hits._id,
                name: hits.name
            })
        });
    } else {
        Client.search({
            index: 'companies',
            type: 'company',
            size: max,
            from: page * max,
            body: {
                sort: {
                    'name.keyword': { order: asc }
                },
                query: {
                    match: {
                        _all: search,
                    }
                }
            }
        }, function (error) {
            if (error) {
                res.status(500).send("Internal server error");
            } else {
                res.status(200).send({
                    company_id: entity._id,
                    name: entity.name

                });
            }
        }).then(function (body) {
            var hits = body.hits.hits;

            res.status(200).send({
                company_id: hits._id,
                name: hits.name
            })
        });

    }

    /*
        compEntity.Company.find({}, function (err, docs) {
        var allComps = [];

        docs.forEach(function (company) {
            allComps.push(company);
        });

        if (allComps.length < 1) {
            res.status(403).send("There are no companies in the database")
        } else {
            res.status(200).send(allComps);
        }
    });*/

});

module.exports = app;