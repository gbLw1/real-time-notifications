import { useEffect } from "react";
import { socket } from "../../socket";
import { AuthTokenModel } from "../../interfaces/models/auth-token.model";
import { Notification } from "../../routes";
import { CustomToast } from "../../components/custom-toast";
import toast, { Toast } from "react-hot-toast";

export default function Home() {
  useEffect(() => {
    socket.connect();

    const { socketRoom }: AuthTokenModel = JSON.parse(
      sessionStorage.getItem("auth") || "{}"
    );

    if (!socketRoom) {
      return;
    }

    socket.emit("join_room", socketRoom);

    function onIndividual(data: Notification) {
      const { message } = data;
      toast.custom((t: Toast) => (
        <CustomToast type="individual" data={message} toast={t} />
      ));
    }

    socket.on("receive_individual", onIndividual);

    return () => {
      socket.off("receive_individual", onIndividual);
      socket.disconnect();
    };
  }, []);

  return (
    <div className="h-screen w-screen flex flex-col items-center justify-center bg-slate-800">
      <h1 className="text-3xl font-bold underline text-slate-200">
        Hello world!
      </h1>
    </div>
  );
}
