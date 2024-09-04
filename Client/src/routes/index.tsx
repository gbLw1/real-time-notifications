import { BrowserRouter, Route, Routes } from "react-router-dom";
import Home from "../pages/home/home";
import Login from "../pages/login";
import { toast } from "react-toastify";
import { useEffect } from "react";
import { socket } from "../socket";

export type GlobalNotification = {
  message: string;
};

function App() {
  useEffect(() => {
    function onConnect() {
      console.log("Connected to server");
      toast.success("Connected to server");
    }

    function onReceiveGlobal(data: GlobalNotification) {
      console.log(data);
      toast.success(data.message);
    }

    socket.on("connect", onConnect);
    socket.on("receive_global", onReceiveGlobal);

    return () => {
      socket.off("connect", onConnect);
      socket.off("receive_global", onReceiveGlobal);
    };
  }, []);

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/" element={<Home />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
