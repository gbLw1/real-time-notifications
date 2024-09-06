import toast, { Toast } from "react-hot-toast";
import { NotificationModel } from "../../interfaces/models/notification.model";
import "./custom-toast.css";

type Props = {
  type: "global" | "individual";
  data: NotificationModel;
  toast: Toast;
};

export const CustomToast = ({ toast: t, data, type }: Props) => {
  return (
    <div
      className={`${
        t.visible ? "animate-enter" : "animate-leave"
      } max-w-md w-full bg-white shadow-lg rounded-lg pointer-events-auto flex ring-1 ring-black ring-opacity-5`}
    >
      <div className="flex-1 w-0 p-2">
        <div className="flex items-center">
          <div className="flex-shrink-0 pt-0.5">
            <img
              className="h-16 w-16 rounded-full"
              src="https://png.pngtree.com/png-vector/20230316/ourmid/pngtree-admin-and-customer-service-job-vacancies-vector-png-image_6650726.png"
              alt=""
            />
          </div>
          <div className="ml-3 flex flex-col flex-1">
            <p className="text-sm font-medium text-gray-900">
              {type === "global" ? "Global Notification" : "Mr. Bean"}
            </p>
            <p className="mt-1 text-sm text-gray-500">{data.content}</p>
            {data.redirectUrl && (
              <div className="flex justify-end mt-2 text-sm font-medium text-indigo-600 hover:text-indigo-500 text-end">
                <a href={data.redirectUrl} target="_blank">
                  Click here to see more
                </a>
              </div>
            )}
          </div>
        </div>
      </div>
      <div className="flex border-l border-gray-200">
        <button
          onClick={() => toast.dismiss(t.id)}
          className="w-full border border-transparent rounded-none rounded-r-lg p-4 flex items-center justify-center text-sm font-medium text-indigo-600 hover:text-indigo-500 focus:outline-none focus:ring-2 focus:ring-indigo-500"
        >
          Close
        </button>
      </div>
    </div>
  );
};
