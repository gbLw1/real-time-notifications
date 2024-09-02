import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import api from "../../services/client/api";

export interface AuthTokenArgs {
  user?: string | null;
  password?: string | null;
}

export default function Login() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState<boolean>(false);

  const {
    handleSubmit,
    formState: { errors },
    register,
  } = useForm<AuthTokenArgs>({
    defaultValues: {
      user: "",
      password: "",
    },
  });

  const handleLogin = async (data: AuthTokenArgs) => {
    setLoading(true);

    api
      .post("/auth/token", data)
      .then((response) => {
        const { result } = response.data;
        sessionStorage.setItem("auth", JSON.stringify(result));
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

    return (
      <section className="gradient-form h-full bg-neutral-200 dark:bg-neutral-700">
        <div className="container h-full p-10">
          <div className="flex h-full flex-wrap items-center justify-center text-neutral-800 dark:text-neutral-200">
            <div className="w-full">
              <div className="block rounded-lg bg-white shadow-lg dark:bg-neutral-800">
                <div className="g-0 lg:flex lg:flex-wrap">
                  {/* Left column container */}
                  <div className="px-4 md:px-0 lg:w-6/12">
                    <div className="md:mx-6 md:p-12">
                      {/* Logo */}
                      <div className="text-center">
                        <img
                          className="mx-auto w-48"
                          src="https://tecdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/lotus.webp"
                          alt="logo"
                        />
                        <h4 className="mb-12 mt-1 pb-1 text-xl font-semibold">
                          We are The Lotus Team
                        </h4>
                      </div>

                      <form onSubmit={handleSubmit(handleLogin)}>
                        <p className="mb-4">Please login to your account</p>

                        {/* Username input */}
                        <div className="relative mb-4">
                          <input
                            {...register("user", {
                              required: "E-mail é obrigatório",
                            })}
                            id="user"
                            type="email"
                            className="peer block min-h-[auto] w-full rounded border-0 bg-transparent px-3 py-[0.32rem] leading-[1.6] outline-none transition-all duration-200 ease-linear focus:placeholder:opacity-100 peer-focus:text-primary"
                          />
                          <label
                            htmlFor="user"
                            className="pointer-events-none absolute left-3 top-0 mb-0 max-w-[90%] origin-[0_0] truncate pt-[0.37rem] leading-[1.6] text-neutral-500 transition-all duration-200 ease-out peer-focus:-translate-y-[0.9rem] peer-focus:scale-[0.8] peer-focus:text-primary"
                          >
                            E-mail do usuário
                          </label>
                          {errors.user && (
                            <p className="text-red-500 text-xs mt-1">
                              {errors.user.message}
                            </p>
                          )}
                        </div>

                        {/* Password input */}
                        <div className="relative mb-4">
                          <input
                            {...register("password", {
                              required: "Senha é obrigatória",
                            })}
                            id="password"
                            type="password"
                            className="peer block min-h-[auto] w-full rounded border-0 bg-transparent px-3 py-[0.32rem] leading-[1.6] outline-none transition-all duration-200 ease-linear focus:placeholder:opacity-100 peer-focus:text-primary"
                          />
                          <label
                            htmlFor="password"
                            className="pointer-events-none absolute left-3 top-0 mb-0 max-w-[90%] origin-[0_0] truncate pt-[0.37rem] leading-[1.6] text-neutral-500 transition-all duration-200 ease-out peer-focus:-translate-y-[0.9rem] peer-focus:scale-[0.8] peer-focus:text-primary"
                          >
                            Senha de acesso
                          </label>
                          {errors.password && (
                            <p className="text-red-500 text-xs mt-1">
                              {errors.password.message}
                            </p>
                          )}
                        </div>

                        {/* Submit button */}
                        <div className="mb-12 pb-1 pt-1 text-center">
                          <button
                            type="submit"
                            className={`mb-3 inline-block w-full rounded px-6 pb-2 pt-2.5 text-xs font-medium uppercase leading-normal text-white shadow-dark-3 transition duration-150 ease-in-out hover:shadow-dark-2 focus:shadow-dark-2 focus:outline-none focus:ring-0 active:shadow-dark-2 dark:shadow-black/30 dark:hover:shadow-dark-strong dark:focus:shadow-dark-strong dark:active:shadow-dark-strong ${
                              loading ? "opacity-50 cursor-not-allowed" : ""
                            }`}
                            style={{
                              background:
                                "linear-gradient(to right, #ee7724, #d8363a, #dd3675, #b44593)",
                            }}
                            disabled={loading}
                          >
                            {loading ? "Entrando..." : "Entrar"}
                          </button>

                          {/* Forgot password link */}
                          <a
                            href="/forgot-password"
                            className="text-sm text-indigo-600 hover:text-indigo-700"
                          >
                            Esqueci minha senha
                          </a>
                        </div>

                        {/* Register button */}
                        <div className="flex items-center justify-between pb-6">
                          <p className="mb-0 me-2">Não tem uma conta?</p>
                          <a
                            href="/register"
                            className="inline-block rounded border-2 border-danger px-6 pb-[6px] pt-2 text-xs font-medium uppercase leading-normal text-danger transition duration-150 ease-in-out hover:border-danger-600 hover:bg-danger-50/50 hover:text-danger-600 focus:border-danger-600 focus:bg-danger-50/50 focus:text-danger-600 focus:outline-none focus:ring-0 active:border-danger-700 active:text-danger-700 dark:hover:bg-rose-950 dark:focus:bg-rose-950"
                          >
                            Criar conta
                          </a>
                        </div>
                      </form>
                    </div>
                  </div>

                  {/* Right column container with background and description */}
                  <div
                    className="flex items-center rounded-b-lg lg:w-6/12 lg:rounded-e-lg lg:rounded-bl-none"
                    style={{
                      background:
                        "linear-gradient(to right, #ee7724, #d8363a, #dd3675, #b44593)",
                    }}
                  >
                    <div className="px-4 py-6 text-white md:mx-6 md:p-12">
                      <h4 className="mb-6 text-xl font-semibold">
                        We are more than just a company
                      </h4>
                      <p className="text-sm">
                        Lorem ipsum dolor sit amet, consectetur adipisicing
                        elit, sed do eiusmod tempor incididunt ut labore et
                        dolore magna aliqua. Ut enim ad minim veniam, quis
                        nostrud exercitation ullamco laboris nisi ut aliquip ex
                        ea commodo consequat.
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    );
  };
}
