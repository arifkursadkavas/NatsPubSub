# Simple Chat Client with NATS

This is a simple chat client which utilizes NATS as the messaging server and written in C# on the top of .Net Core.

It comprises of the following sections


- __API__ : For sending/receiving messages to/from topic
- __NATS Client__ : For interacting with NATS
- __Cache__: Application cache to keep the received messages in order to
         make it available for future GET requests made to the API.
- __AutoFAC__: For IOC 

## Program Flow

At startup, client subscribes to the topic "topicName". With this subscription an event handler is registered for processing of incoming messages. This event handler basically outputs the messages to console and also stores them in the application cache for future use(in case of GET request to API).

Clients Web API is responsible for accepting HTTP calls. Through Controllers, these calls are serviced at MessageService. 

Autofac is used for IOC to handle dependency injection. As a todo, it can be extended for NatsClient

MessageController POST accepts HTTP-POST requests and publishes the message content to the relevant topic in NATS("topicName"). As the client also initially subscribed to this topic, immediately recieves the same message and displays it on the console and stores it in application cache.

When a GET request is made to the API, the stored messages in the cache are returned in the response and the cache is then cleared. Implementation can be altered here.



## Usage
Binary of the application can be found at
```
...\snow_app\snow_app\bin\Debug\netcoreapp3.1\snow_app
```

Double click runs the app and  opens a console window. You may need to press enter on the console to see the messages flow once sent through the Web API.

## Blackbox Test
Testing can be done using curl, Postman or any other tool to generate HTTP requests. By default Web API is served at 
```
https://localhost:5001
```
In order to publish a message at the topic, a POST to

```
https://localhost:5001/message
```

with a body
```
    {
        "userName": "user",
        "textMessage": "text message",
        "timeStamp": "2020-11-25T15:39:48.8905023+01:00"
    }
```
can be sent.

Published messages can be retrieved with a GET to
```
https://localhost:5001/message
```
would result an array of messages
```
 [
    {
        "userName": "user",
        "textMessage": "text message",
        "timeStamp": "2020-11-25T15:39:48.89+01:00"
    }
]
```