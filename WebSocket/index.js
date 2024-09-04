import express from "express";
const app = express();
import { createServer } from "http";
import { Server } from "socket.io";
import cors from "cors";

app.use(cors());

const port = 3069;

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

  socket.on("global", (data) => {
    console.log("Global message received:", {
      message: `${socket.id}: ${data.message}`,
    });
    socket.broadcast.emit("receive_global", {
      message: `${socket.id}: ${data.message}`,
    });
  });

  socket.on("individual", (data) => {
    console.log("Individual message received:", data.message);
    socket.to(data.room).emit("receive_individual", data.message);
  });

  socket.on("disconnect", () => {
    console.log(`User disconnected: ${socket.id}`);
  });
});

server.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
