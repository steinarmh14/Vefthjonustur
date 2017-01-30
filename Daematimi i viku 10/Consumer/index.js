#!/usr/bin/env node

var amqp = require('amqplib/callback_api');

var args = ["punchapi.addUser", "punchapi.addpunch", "punchapi.discount"];


amqp.connect('amqp://localhost', function(err, conn) {
  conn.createChannel(function(err, ch) {
    var ex = 'punchapi';

    ch.assertExchange(ex, 'topic', {durable: false});

    ch.assertQueue('', {exclusive: true}, function(err, q) {
      console.log(' [*] Waiting for logs. To exit press CTRL+C');

      args.forEach(function(key) {
        ch.bindQueue(q.queue, ex, key);
      });

      ch.consume(q.queue, function(msg) {
        var message;
        if(msg.fields.routingKey == 'punchapi.addUser')
        {
          message = "User was added";
        }
        else if(msg.fields.routingKey == 'punchapi.addpunch')
        {
          message = "User got a punch";
        }
        else if(msg.fields.routingKey == 'punchapi.discount')
        {
          message = "User got discount";
        }

        console.log(" [x] %s:'%s'", message, msg.content.toString());
      }, {noAck: true});
    });
  });
});