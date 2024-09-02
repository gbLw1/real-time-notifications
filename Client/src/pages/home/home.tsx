import { useEffect } from "react";

export default function Home() {
  useEffect(() => {
    document.title = "Home | My App";
  }, []);

  return (
    <div className="h-screen w-screen flex flex-col items-center justify-center bg-slate-200">
      <h1 className="text-3xl font-bold underline">Hello world!</h1>
    </div>
  );
}
