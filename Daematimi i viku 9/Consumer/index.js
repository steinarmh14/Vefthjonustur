#!/usr/bin/env node

var amqp = require('amqplib/callback_api');

var args = ['punchcardApi.addUser', 'punchcardApi.addPunch', 'punchcardApi.addDiscount'];

if (args.length == 0) {
    console.log("Usage: receive_logs_topic.js <facility>.<severity>");
    process.exit(1);
}

amqp.connect('amqp://localhost', function (err, conn) {
    conn.createChannel(function (err, ch) {
        var ex = 'punchcardApi';

        ch.assertExchange(ex, 'topic', { durable: false });

        ch.assertQueue('', { exclusive: true }, function (err, q) {
            console.log(' [*] Waiting for logs. To exit press CTRL+C');

            args.forEach(function (key) {
                ch.bindQueue(q.queue, ex, key);
            });

            ch.consume(q.queue, function (msg) {
                var message = "";
                if (msg.fields.routingKey === 'punchcardApi.addUser') {
                    message = "User was added";
                } else if (msg.fields.routingKey === 'punchcardApi.addPunch') {
                    message = "User got a punch";
                } else if (msg.fields.routingKey === 'punchcardApi.addDiscount') {
                    message = "User got a discount";
                }
                console.log(" [x] %s:'%s'", message, msg.content.toString());
            }, { noAck: true });
        });
    });
});