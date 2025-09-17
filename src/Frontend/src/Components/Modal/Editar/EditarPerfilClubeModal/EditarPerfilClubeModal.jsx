import React, { useEffect, useState } from "react";
import FormErrors from "../../../Utils/FormErrors/FormErrors";
import { editarClube, getClube } from "../../../../Services/ClubeService/ClubeService";
import { ParseFormErrorsClube } from "../../../Utils/ParseFormErrors/ParseFormErrorsClube";
import "./EditarPerfilClubeModal.css";

const campos = [
    { name: "nome", label: "Nome", type: "text" },
    { name: "cnpj", label: "CNPJ", type: "text" },
    { name: "certificadoRegistro", label: "Certificado de Registro", type: "text" },
    { name: "enderecoPais", label: "País", type: "text" },
    { name: "enderecoEstado", label: "Estado", type: "text" },
    { name: "enderecoCidade", label: "Cidade", type: "text" },
    { name: "enderecoBairro", label: "Bairro", type: "text" },
    { name: "enderecoRua", label: "Rua", type: "text" },
    { name: "enderecoNumero", label: "Número", type: "text" },
];

const EditarPerfilClubeModal = ({ isOpen, onClose, onSuccess, dadosIniciais }) => {
    const [form, setForm] = useState(null);
    const [errors, setErrors] = useState(null);
    const [loadingUpdate, setLoadingUpdate] = useState(false);
    const [loadingDados, setLoadingDados] = useState(false);

    useEffect(() => {
        if (!isOpen) {
            setForm(null);
            setErrors(null);
            return;
        }

        if (dadosIniciais) {
            setForm(dadosIniciais);
            return;
        }

        const fetchClube = async () => {
            setLoadingDados(true);
            try {
                const data = await getClube();
                setForm(data);
            } catch {
                setErrors({ geral: ["Erro ao carregar dados do clube."] });
            } finally {
                setLoadingDados(false);
            }
        };

        fetchClube();
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

    const validarCampos = () => {
        const newErrors = {};
        campos.forEach(({ name }) => {
            if (!form[name] || form[name].toString().trim() === "") {
                newErrors[name] = ["Campo obrigatório"];
            }
        });
        return newErrors;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrors(null);

        const newErrors = validarCampos();

        if (Object.keys(newErrors).length > 0) {
            setErrors(newErrors);
            return;
        }

        setLoadingUpdate(true);
        try {
            await editarClube(form);
            alert("Clube editado com sucesso!");
            if (onSuccess) onSuccess();  // Atualiza e fecha o modal no pai
        } catch (error) {
            const parsed = ParseFormErrorsClube(error);
            setErrors(parsed);
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
        <div className="editar-perfil-clube-modal">
            <div className="modal-overlay" onClick={handleOverlayClick}>
                <div className="modal-box">
                    <h2>Editar Clube</h2>
                    <p>Altere os dados abaixo</p>

                    <form className="modal-form" onSubmit={handleSubmit}>
                        <FormErrors errors={errors} />

                        {camposEmPares.map((par, idx) => (
                            <div className="form-row" key={idx}>
                                {par.map(({ name, label, type = "text" }) => (
                                    <div className="form-group" key={name}>
                                        <label>{label}</label>
                                        <input
                                            type={type}
                                            name={name}
                                            value={form[name] || ""}
                                            onChange={handleChange}
                                            className={getInputClass(name)}
                                            disabled={loadingUpdate || loadingDados}
                                        />
                                        <FormErrors errors={errors} field={name} />
                                    </div>
                                ))}
                            </div>
                        ))}

                        <div className="modal-buttons">
                            <button
                                type="submit"
                                className="btn-submit"
                                disabled={loadingUpdate}
                            >
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
        </div>
    );
};

export default EditarPerfilClubeModal;
