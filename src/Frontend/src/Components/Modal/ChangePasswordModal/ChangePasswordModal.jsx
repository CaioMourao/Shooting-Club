import React, { useState, useEffect } from "react";
import { changeSenha } from "../../../Services/UserService/UserService"; 
import "./ChangePasswordModal.css";
import FormErrors from "../../../Components/Utils/FormErrors/FormErrors";
import { ParseFormErrors } from "../../../Components/Utils/ParseFormErrors/ParseFormErrors";

const ChangePasswordModal = ({ isOpen, onClose, onSuccess }) => {
  const [senha, setSenha] = useState("");
  const [novaSenha, setNovaSenha] = useState("");
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (isOpen) {
      setSenha("");
      setNovaSenha("");
      setErrors({});
      setLoading(false);
    }
  }, [isOpen]);

  if (!isOpen) return null;

  const validar = () => {
    const erros = {};
    if (!senha) erros.senha = ["Campo obrigatório"];
    if (!novaSenha) erros.novaSenha = ["Campo obrigatório"];
    return erros;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const errosValidados = validar();
    if (Object.keys(errosValidados).length > 0) {
      setErrors(errosValidados);
      return;
    }

    setErrors({});
    setLoading(true);

    try {
      await changeSenha({ senha, novaSenha });
      setLoading(false);
      if (onSuccess) onSuccess();
      onClose();
      alert("Senha alterada com sucesso!");
    } catch (error) {
      setLoading(false);
      const parsedErrors = ParseFormErrors(error);
      
      setErrors(parsedErrors || {});
    }
  };

  return (
    <div className="change-password-modal-overlay">
      <div className="change-password-modal">
        <h2>Alterar Senha</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="senha">Senha atual</label>
            <input
              type="password"
              id="senha"
              name="senha"
              value={senha}
              onChange={(e) => setSenha(e.target.value)}
              placeholder="Digite sua senha atual"
              disabled={loading}
              className={errors.senha ? "input-error" : ""}
            />
            <FormErrors errors={errors} field="senha" />
          </div>

          <div className="form-group">
            <label htmlFor="novaSenha">Nova senha</label>
            <input
              type="password"
              id="novaSenha"
              name="novaSenha"
              value={novaSenha}
              onChange={(e) => setNovaSenha(e.target.value)}
              placeholder="Digite a nova senha"
              disabled={loading}
              className={errors.novaSenha ? "input-error" : ""}
            />
            <FormErrors errors={errors} field="novaSenha" />
          </div>

          <div className="botoes-modal">
            <button type="submit" className="btn-salvar" disabled={loading}>
              {loading ? "Salvando..." : "Salvar"}
            </button>
            <button
              type="button"
              className="btn-cancelar"
              onClick={onClose}
              disabled={loading}
            >
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ChangePasswordModal;
