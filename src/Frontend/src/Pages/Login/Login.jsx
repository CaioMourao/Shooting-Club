import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FaUser, FaLock, FaSignInAlt } from "react-icons/fa";
import { useAuth } from "../../Context/AuthContext/AuthContext";
import "./Login.css";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");

    if (!email.includes("@") || password.length < 4) {
      setError("Por favor, preencha e-mail e senha válidos.");
      return;
    }

    try {
      await login(email, password);
      navigate("/home");
    } catch (err) {
      setError("E-mail ou senha incorretos.");
    }
  };

  return (
    <div className="login-wrapper">
      <div className="container">
        <form onSubmit={handleSubmit} noValidate>
          <div className="logo-header">
            <img src="/logo.jpeg" alt="Logo" className="logo-image" />
            <div className="logo-text-block">
              <h2 className="logo-title">
                CLUBE DE TIRO
                <br />
                ESPORTIVO DE CRATEÚS
              </h2>
            </div>
          </div>
          <h2 className="text-xl font-semibold text-black">Acessar Conta</h2>
          <p className="text-sm text-gray-600">
            Entre com o e-mail e senha para acessar
          </p>
          <div className="input-field">
            <input
              type="email"
              placeholder="E-mail"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
            <FaUser className="icon" />
          </div>
          <div className="input-field">
            <input
              type="password"
              placeholder="Senha"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
            <FaLock className="icon" />
          </div>
          {error && <p className="error-message">{error}</p>}
          <div className="recall-forget">
            <a href="#">Esqueceu sua senha?</a>
          </div>
          <button type="submit">
            <span className="button-text">Entrar</span>
            <FaSignInAlt className="button-icon" />
          </button>
        </form>
      </div>
      <div className="login-footer">
        <p>
          2025 © Projeto desenvolvido por <strong>CTRVF</strong>
        </p>
      </div>
    </div>
  );
};

export default Login;
