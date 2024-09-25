# Real time notifications

## Description

WebSockets to send real time notifications to the client from the database.

This project uses SQL Server to store the data, the API is built with dotnet core web api and the client it's a React app.

## Motivation

The main motivation for this project is to learn how to communicate with the client in real time as data changes in the database.

Also, I might use this project as a base for future implementations in my current job.

## Progress

- [x] Web API CRUD
- [x] WebSocket Server
- [x] React client

## To do

### Client

- Register page
- Private Routes
- User logout
- Subscribe to individual notifications (auth token)
- Header notifications dropdown
  - Notification list
  - Mark as read
  - Delete
  - Mark all as read
  - Delete all

### Server

- Swagger documentation (summary)

## How to test

1. Run the socket, the API and the client
2. Create a notification using the swagger
3. Check the notification in real time in the client
