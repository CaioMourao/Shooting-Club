import React, { useEffect, useState } from "react";
import InputMask from "react-input-mask";
import { getUsuarioById, atualizarUsuario } from "../../../../Services/UserService/UserService";
import FormErrors from "../../../Utils/FormErrors/FormErrors";
import { ParseFormErrors } from "../../../Utils/ParseFormErrors/ParseFormErrors";
import "./EditarUsuarioModal.css";

const campos = [
  { name: "nome", label: "Nome", type: "text" },
  { name: "email", label: "Email", type: "email" },
  { name: "senha", label: "Senha", type: "password", placeholder: "Informe a senha" },
  { name: "cpf", label: "CPF", mask: "999.999.999-99" },
  { name: "dataNascimento", label: "Data de Nascimento", type: "date" },
  { name: "enderecoPais", label: "País", type: "text" },
  { name: "enderecoEstado", label: "Estado", type: "text" },
  { name: "enderecoCidade", label: "Cidade", type: "text" },
  { name: "enderecoBairro", label: "Bairro", type: "text" },
  { name: "enderecoRua", label: "Rua", type: "text" },
  { name: "enderecoNumero", label: "Número", type: "text" },
  { name: "cr", label: "CR", type: "text" },
  { name: "dataVencimentoCR", label: "Data Vencimento CR", type: "date" },
  { name: "sfpcVinculacao", label: "SFPC Vinculação", type: "text" },
  { name: "numeroFiliacao", label: "Número Filiação", type: "text" },
  { name: "dataFiliacao", label: "Data Filiação", type: "date" },
  { name: "dataRenovacaoFiliacao", label: "Data Renovação Filiação", type: "date" }
];

const EditarUsuarioModal = ({ isOpen, onClose, usuarioId, onSuccess}) => {
  const [form, setForm] = useState(null);
  const [loadingDados, setLoadingDados] = useState(false);
  const [loadingUpdate, setLoadingUpdate] = useState(false);
  const [errors, setErrors] = useState(null);

  useEffect(() => {
    if (!isOpen || !usuarioId) {
      setForm(null);
      setErrors(null);
      return;
    }

    const fetchUsuario = async () => {
      setLoadingDados(true);
      try {
        const data = await getUsuarioById(usuarioId);
        setForm({ ...data, senha: "" });
      } catch (err) {
        setErrors({ geral: ["Erro ao carregar dados do usuário."] });
      } finally {
        setLoadingDados(false);
      }
    };

    fetchUsuario();
  }, [isOpen, usuarioId]);

  const handleOverlayClick = (e) => {
    if (e.target.classList.contains("modal-overlay") && !loadingUpdate) {
      onClose();
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrors(null);

    const newErrors = {};
    for (const key in form) {
      if (!form[key]?.toString().trim()) {
        newErrors[key] = ["Campo obrigatório"];
      }
    }

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    setLoadingUpdate(true);
    try {
      await atualizarUsuario(form);
      alert("Usuário atualizado com sucesso!");
      if (onSuccess) onSuccess();
      onClose();
    } catch (error) {
      const parsedErrors = ParseFormErrors(error);
      setErrors(parsedErrors);
    } finally {
      setLoadingUpdate(false);
    }
  };

  const getInputClass = (field) => (errors?.[field] ? "input-error" : "");

  if (!isOpen || !form) return null;

  
  const camposEmPares = [];
  for (let i = 0; i < campos.length; i += 2) {
    camposEmPares.push(campos.slice(i, i + 2));
  }

  return (
    <div className="editar-usuario-modal">
      <div className="modal-overlay" onClick={handleOverlayClick}>
        <div className="modal-box">
          <h2>Editar Usuário</h2>
          <p>Altere os dados abaixo</p>

          <form className="modal-form" onSubmit={handleSubmit}>
            {camposEmPares.map((par, idx) => (
              <div className="form-row" key={idx}>
                {par.map(({ name, label, type = "text", mask, placeholder }) => (
                  <div className="form-group half" key={name}>
                    <label>{label}</label>
                    {mask ? (
                      <InputMask mask={mask} value={form[name]} onChange={handleChange}>
                        {(inputProps) => (
                          <input
                            {...inputProps}
                            name={name}
                            className={getInputClass(name)}
                            type="text"
                          />
                        )}
                      </InputMask>
                    ) : (
                      <input
                        type={type}
                        name={name}
                        value={form[name] || ""}
                        onChange={handleChange}
                        className={getInputClass(name)}
                        placeholder={placeholder || ""}
                      />
                    )}
                    <FormErrors errors={errors} field={name} />
                  </div>
                ))}
              </div>
            ))}

            <div className="modal-buttons">
              <button type="submit" className="btn-submit" disabled={loadingUpdate}>
                {loadingUpdate ? "Salvando..." : "Salvar"}
              </button>
              <button type="button" className="btn-cancel" onClick={onClose} disabled={loadingUpdate}>
                Cancelar
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default EditarUsuarioModal;
