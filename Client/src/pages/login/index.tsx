import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import api from "../../services/client/api";
import { AuthTokenArgs } from "../../interfaces/arguments/auth-token.args";
import { AuthTokenModel } from "../../interfaces/models/auth-token.model";

export default function Login() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState<boolean>(false);

  const {
    handleSubmit,
    formState: { errors },
    register,
  } = useForm<AuthTokenArgs>({
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const handleLogin = async (data: AuthTokenArgs) => {
    setLoading(true);

    api
      .post<AuthTokenModel>("/auth/token", data)
      .then(({ data }) => {
        sessionStorage.setItem("auth", JSON.stringify(data));
        navigate("/");
        toast.success("Bem vindo!");
      })
      .catch(() => {
        toast.error(
          "Erro ao fazer login. Verifique suas credenciais e tente novamente."
        );
      })
      .finally(() => {
        setLoading(false);
      });
  };

  return (
    <div className="h-screen w-screen flex flex-col items-center justify-center bg-slate-200">
      <h1 className="text-3xl font-bold mb-4">Login</h1>

      <form
        onSubmit={handleSubmit(handleLogin)}
        className="bg-white p-8 rounded-lg shadow-lg w-96"
      >
        <div className="mb-4">
          <label className="block text-gray-700 font-bold" htmlFor="user">
            Usuário
          </label>
          <input
            type="text"
            placeholder="Usuário"
            className="w-full p-2 border border-gray-300 rounded-lg"
            {...register("email", { required: true })}
          />
          {errors.email && (
            <span className="text-red-500">Campo obrigatório</span>
          )}
        </div>

        <div className="mb-4">
          <label className="block text-gray-700 font-bold" htmlFor="password">
            Senha
          </label>
          <input
            type="password"
            placeholder="Senha"
            className="w-full p-2 border border-gray-300 rounded-lg"
            {...register("password", { required: true })}
          />
          {errors.password && (
            <span className="text-red-500">Campo obrigatório</span>
          )}
        </div>

        <button
          type="submit"
          className="w-full p-2 my-2 bg-blue-500 text-white font-bold rounded-lg"
          disabled={loading}
        >
          {loading ? "Carregando..." : "Entrar"}
        </button>
      </form>
    </div>
  );
}
