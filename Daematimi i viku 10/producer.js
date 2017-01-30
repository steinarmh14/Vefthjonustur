#!/usr/bin/env node

var amqp = require('amqplib/callback_api');

var send = function (_key,msgObject)
{
    amqp.connect('amqp://localhost', function(err, conn) {
    conn.createChannel(function(err, ch) {
      var ex = 'punchapi';
      var args = process.argv.slice(2);
      var key = _key;
      var msg = JSON.stringify(msgObject);
      console.log(key);

      ch.assertExchange(ex, 'topic', {durable: false});
      ch.publish(ex, key, new Buffer(msg));
      console.log(" [x] Sent %s:'%s'", key, msg);
    });


  
  setTimeout(function() { conn.close();}, 500);
  });

}


module.exports = {
  send
}


/*
  function addUser(){

  }

  function getPunch(){

  }

  function userDiscount(){
*/