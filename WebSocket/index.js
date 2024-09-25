import express from "express";
const app = express();
import { createServer } from "http";
import { Server } from "socket.io";
import cors from "cors";

app.use(cors());
app.use(express.json());

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
    console.log("Individual message received:", { message: data.message });
    socket.to(data.room).emit("receive_individual", { message: data.message });
  });

  socket.on("disconnect", () => {
    console.log(`User disconnected: ${socket.id}`);
  });
});

app.post("/send-notification", (req, res) => {
  const { message, roomId } = req.body;

    console.log(req.body)

  if (!message) {
    return res.status(400).send("Message is required");
  }

  if (roomId) {
    io.to(roomId).emit("receive_individual", { message });
  } else {
    io.emit("receive_global", { message });
  }

  console.log("Notification sent:", { message, roomId });
  res.send("Notification sent");
});

server.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
