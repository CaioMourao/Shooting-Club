import React, { useState, useEffect } from "react";
import InputMask from "react-input-mask";
import FormErrors from "../../../Utils/FormErrors/FormErrors";
import { ParseFormErrors } from "../../../Utils/ParseFormErrors/ParseFormErrors";
import { atualizarUsuarioProfile } from "../../../../Services/UserService/UserService";
import "./EditarPerfilModal.css";

const campos = [
  { name: "nome", label: "Nome", type: "text" },
  { name: "dataNascimento", label: "Data de Nascimento", mask: "99/99/9999" },
  { name: "enderecoPais", label: "País", type: "text" },
  { name: "enderecoEstado", label: "Estado", type: "text" },
  { name: "enderecoCidade", label: "Cidade", type: "text" },
  { name: "enderecoBairro", label: "Bairro", type: "text" },
  { name: "enderecoRua", label: "Rua", type: "text" },
  { name: "enderecoNumero", label: "Número", type: "text" },
];

function formatarDataParaBR(dataIso) {
  if (!dataIso) return "";
  const [ano, mes, dia] = dataIso.split("-");
  if (!ano || !mes || !dia) return "";
  return `${dia}/${mes}/${ano}`;
}

function formatarDataParaISO(dataBr) {
  if (!dataBr || !dataBr.includes("/")) return null;
  const [dia, mes, ano] = dataBr.split("/");
  if (!dia || !mes || !ano) return null;
  return `${ano}-${mes.padStart(2, "0")}-${dia.padStart(2, "0")}`;
}

const EditarPerfilModal = ({ isOpen, onClose, dadosIniciais, onSuccess }) => {
  const [form, setForm] = useState(null);
  const [errors, setErrors] = useState(null);
  const [loadingUpdate, setLoadingUpdate] = useState(false);

  useEffect(() => {
    if (!isOpen || !dadosIniciais) {
      setForm(null);
      setErrors(null);
      return;
    }
    
    setForm({
      ...dadosIniciais,
      dataNascimento: dadosIniciais.dataNascimento
        ? formatarDataParaBR(dadosIniciais.dataNascimento)
        : "",
    });
    setErrors(null);
  }, [isOpen, dadosIniciais]);

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
    campos.forEach(({ name }) => {
      if (!form[name]?.toString().trim()) {
        newErrors[name] = ["Campo obrigatório"];
      }
    });
    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    setLoadingUpdate(true);
    try {
      const dadosParaEnviar = {
        ...form,
        dataNascimento: formatarDataParaISO(form.dataNascimento),
      };

      await atualizarUsuarioProfile(dadosParaEnviar);
        alert("Perfil atualizado com sucesso!");
        if (onSuccess) onSuccess(dadosParaEnviar);
        onClose();
    } catch (error) {
      const parsedErrors = ParseFormErrors(error);
      setErrors(parsedErrors);
      alert("Erro ao atualizar o perfil. Verifique os campos preenchidos.");
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
    <div className="editar-perfil-modal modal-overlay" onClick={handleOverlayClick}>
      <div className="modal-box">
        <h2>Editar Perfil</h2>
        <form className="modal-form" onSubmit={handleSubmit}>
          {camposEmPares.map((par, idx) => (
            <div className="form-row" key={idx}>
              {par.map(({ name, label, type = "text", mask }) => (
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
            <button
              type="button"
              className="btn-cancel"
              onClick={onClose}
              disabled={loadingUpdate}
            >
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditarPerfilModal;
