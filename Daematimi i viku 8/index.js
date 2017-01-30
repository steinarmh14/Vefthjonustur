const express = require('express');
var app = express();
const bodyParser = require('body-parser');
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extend: true
}));

const companies = [];
const users = [];
const punches = [];

app.get("/api/companies", (req, res) => {
    if (companies.length < 1) {
        res.statusCode = 403
        res.send("There are no companies in the system")
    } else {
        res.statusCode = 200;
        res.json(companies);
    }
});

app.get("/api/companies/:id", (req, res) => {

    var id = parseInt(req.params.id);
    var idCheckLength = id;
    id = id - 1;

    if (idCheckLength > companies.length || id < 0) {
        res.statusCode = 403;
        return res.send("ERROR: Company doesnt exist");
    }

    res.json(companies[id]);
});

app.post("/api/companies", (req, res) => {

    if (!req.body.hasOwnProperty('name') || !req.body.hasOwnProperty('punchCount')) {
        res.statusCode = 400;
        return res.send('Error 400: Post syntax incorrect.');
    }

    var newCompany = {
        name: req.body.name,
        punchCount: req.body.punchCount,
    };

    if (newCompany.name === null || typeof newCompany.name === "undefined" || newCompany.name === "" || newCompany.punchCount === "" || newCompany.punchCount === null || typeof newCompany.punchCount === "undefined") {
        res.statusCode = 400;
        return res.send("ERROR: 400 get syntax incorrect");
    }

    companies.forEach(function (item) {
        if (item.name === newCompany.name) {
            res.statusCode = 403
            return res.send("ERROR: company already exists")
        }
    });

    companies.push(newCompany);
    res.statusCode = 200;
    return res.send("The company " + newCompany.name + " has been added");
});

app.get("/api/users", (req, res) => {
    if (users.length < 1) {
        res.statusCode = 403
        res.send("There are no users in the system")
    } else {
        res.statusCode = 200;
        res.json(users);
    }
});

app.post("/api/users", (req, res) => {

    if (!req.body.hasOwnProperty('name') || !req.body.hasOwnProperty('email')) {
        res.statusCode = 400;
        return res.send('Error 400: Post syntax incorrect.');
    }

    var newUser = {
        name: req.body.name,
        email: req.body.email
    };

    if (newUser.name === null || typeof newUser.name === "undefined" || newUser.name === "" || newUser.email === "" || newUser.email === null || typeof newUser.email === "undefined") {
        res.statusCode = 400;
        return res.send("ERROR: 400 get syntax incorrect");
    }

    users.forEach(function (item) {
        if (item.name === newUser.name) {
            res.statusCode = 403
            return res.send("ERROR: user already exists")
        }
    });

    users.push(newUser);
    res.statusCode = 200;
    return res.send("The user " + newUser.name + " has been added");

});

app.post("/api/users/:id/punches", (req, res) => {
    var id = parseInt(req.params.id);
    var idCheckLength = id;
    id = id - 1;


    if (idCheckLength > users.length || id < 0) {
        res.statusCode = 403;
        return res.send("ERROR: user doesnt exist");
    }

    var theCompId = req.body.compId;
    theCompId = theCompId - 1;

    if (req.body.compId > companies.length || theCompId < 0) {
        res.statusCode = 403;
        return res.send("ERROR: Company doesnt exist");
    }

    var exist = false;

    punches.forEach(function (item) {
        if (item.userId === id+1 && item.companyId === req.body.compId) {
            exist = true;
            if (item.punch === 0) {
                item.punch = companies[theCompId].punchCount;
            } else {
                item.punch = item.punch - 1;
            }
        }
    });



    if (exist === false) {
        var punch = {
            userId: idCheckLength,
            companyId: req.body.compId,
            punch: companies[theCompId].punchCount - 1,
            created: new Date()
        };

        punches.push(punch);
    }

    res.statusCode = 200;
    return res.send("Punch added to the user with id " + idCheckLength + " for the company with the id " + req.body.compId);
});

app.get("/api/users/:id/punches", (req, res) => {
    var id = parseInt(req.params.id);

    if (id > users.length || id < 1) {
        res.statusCode = 403;
        return res.send("ERROR: user doesnt exist");
    }

    exist = false;
    var listOfPunches = [];
    var query = req.query.company;
    if (query === null || typeof query === "undefined") {
        punches.forEach(function (item) {
            if (item.userId === id) {
                exist = true;
                listOfPunches.push(item);
            }
        });
    } else {
        punches.forEach(function (item) {
            if (item.userId === id && item.companyId === query) {
                exist = true;
                listOfPunches.push(item);
            }
        });
    }

    if (exist === false) {
        res.statusCode = 403
        return res.send("ERROR: this user has no punches");
    }

    res.statusCode = 200;
    res.send(listOfPunches);
});

app.listen(process.env.PORT || 5000);
