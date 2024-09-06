import { BrowserRouter, Route, Routes } from "react-router-dom";
import Home from "../pages/home/home";
import Login from "../pages/login";
import { useEffect } from "react";
import { socket } from "../socket";
import { NotificationModel } from "../interfaces/models/notification.model";
import { CustomToast } from "../components/custom-toast";
import toast, { Toast } from "react-hot-toast";

export type Notification = {
  message: NotificationModel;
};

function App() {
  useEffect(() => {
    function onConnect() {
      toast.success("Connected to server");
    }

    function onReceiveGlobal(data: Notification) {
      const { message } = data;
      toast.custom((t: Toast) => (
        <CustomToast type="global" data={message} toast={t} />
      ));
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
