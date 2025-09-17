import React, { useState, useEffect } from "react"; 
import InputMask from "react-input-mask";
import { cadastrarUsuario } from "../../../../Services/UserService/UserService";
import FormErrors from "../../../Utils/FormErrors/FormErrors";
import { ParseFormErrors } from "../../../Utils/ParseFormErrors/ParseFormErrors"; 
import "./CadastroUsuarioModal.css";

const campos = [
    { name: "nome", label: "Nome" },
    { name: "email", label: "Email", type: "email" },
    { name: "senha", label: "Senha", type: "password" },
    { name: "cpf", label: "CPF", mask: "999.999.999-99" },
    { name: "dataNascimento", label: "Data de Nascimento", type: "date" },
    { name: "enderecoPais", label: "País" },
    { name: "enderecoEstado", label: "Estado" },
    { name: "enderecoCidade", label: "Cidade" },
    { name: "enderecoBairro", label: "Bairro" },
    { name: "enderecoRua", label: "Rua" },
    { name: "enderecoNumero", label: "Número" },
    { name: "cr", label: "CR" },
    { name: "dataVencimentoCR", label: "Validade do CR", type: "date" },
    { name: "sfpcVinculacao", label: "Vinculação ao SFPC" },
    { name: "numeroFiliacao", label: "Número de Filiação" },
    { name: "dataFiliacao", label: "Data de Filiação", type: "date" },
    { name: "dataRenovacaoFiliacao", label: "Data de Renovação da Filiação", type: "date" },
];

const gerarEstadoInicial = () => {
    return campos.reduce((acc, { name }) => ({ ...acc, [name]: "" }), {});
};

const CadastroUsuarioModal = ({ isOpen, onClose, onSuccess }) => {
    const [form, setForm] = useState(gerarEstadoInicial);
    const [errors, setErrors] = useState(null);

    useEffect(() => {
        if (!isOpen) {
            setErrors(null);
            setForm(gerarEstadoInicial());
        }
    }, [isOpen]);

    const handleChange = ({ target: { name, value } }) => {
        setForm((prev) => ({ ...prev, [name]: value }));
        if (errors?.[name]) {
            setErrors(prevErrors => {
                const novo = { ...prevErrors };
                delete novo[name];
                return Object.keys(novo).length === 0 ? null : novo;
            });
        }
    };

    const validarCampos = () => {
        const erros = {};
        for (const campo of campos) {
            const valor = form[campo.name];
            if (!valor || valor.toString().trim() === "") {
                erros[campo.name] = ["Campo obrigatório"];
            }
        }
        return erros;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrors(null);

        const errosValidacao = validarCampos();
        if (Object.keys(errosValidacao).length > 0) {
            setErrors(errosValidacao);
            return;
        }

        try {
            await cadastrarUsuario(form);
            alert("Usuário cadastrado com sucesso!");
            if (onSuccess) onSuccess();
            onClose();
            setForm(gerarEstadoInicial());
        } catch (error) {
            const parsedErrors = ParseFormErrors(error); 
            setErrors(parsedErrors);

            if (parsedErrors.general && parsedErrors.general.length > 0) {
                alert(parsedErrors.general.join('\n'));
            }
        }
    };

    if (!isOpen) return null;

    return (
        <div className="modal-overlay cadastro-usuario-modal" onClick={(e) => {
            if (e.target.classList.contains("modal-overlay")) {
                onClose();
                setErrors(null);
            }
        }}>
            <div className="modal-box" onClick={(e) => e.stopPropagation()}>
                <h2>Cadastro de Usuário</h2>
                <form onSubmit={handleSubmit} className="modal-form" noValidate>
                    <div className="form-row">
                        {campos.map(({ name, label, type = "text", mask }) => (
                            <div key={name} className="form-group">
                                <label htmlFor={name}>{label}:</label>
                                {mask ? (
                                    <InputMask
                                        mask={mask}
                                        id={name}
                                        name={name}
                                        value={form[name]}
                                        onChange={handleChange}
                                        className={errors?.[name] ? "input-error" : ""}
                                    />
                                ) : (
                                    <input
                                        type={type}
                                        id={name}
                                        name={name}
                                        value={form[name]}
                                        onChange={handleChange}
                                        className={errors?.[name] ? "input-error" : ""}
                                    />
                                )}
                                <FormErrors errors={errors} field={name} />
                            </div>
                        ))}
                    </div>

                    <div className="modal-buttons">
                        <button type="submit" className="btn-submit">Cadastrar</button>
                        <button type="button" className="btn-cancel" onClick={() => {
                            onClose();
                            setErrors(null); 
                        }}>Cancelar</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default CadastroUsuarioModal;