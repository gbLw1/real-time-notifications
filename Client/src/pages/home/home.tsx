import { socket } from "../../socket";

export default function Home() {
  return (
    <div className="h-screen w-screen flex flex-col items-center justify-center bg-slate-800">
      <h1 className="text-3xl font-bold underline text-slate-200">
        Hello world!
      </h1>
      <button
        className="mt-4 bg-slate-500 text-slate-200 px-4 py-2 rounded-md"
        onClick={() => socket.emit("global", { message: "Hello world!" })}
      >
        Send notification
      </button>
    </div>
  );
}
