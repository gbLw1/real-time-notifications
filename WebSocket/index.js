import express from "express";
const app = express();
import { createServer } from "http";
import { Server } from "socket.io";

const port = 3069;

import cors from "cors";
app.use(cors());

const server = createServer(app);
const io = new Server(server, {
  cors: {
    origin: "*",
  },
});

io.on("connection", (socket) => {
  console.log(`User connected: ${socket.id}`);

  socket.on("join_room", (data) => {
    socket.join(data);
  });

  socket.on("send_global_notification", (data) => {
    socket.broadcast.emit("receive_global_notification", data.message);
  });

  socket.on("send_individual_notification", (data) => {
    socket.to(data.room).emit("receive_individual_notification", data.message);
  });

  socket.on("disconnect", () => {
    console.log(`User disconnected: ${socket.id}`);
  });
});

server.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
