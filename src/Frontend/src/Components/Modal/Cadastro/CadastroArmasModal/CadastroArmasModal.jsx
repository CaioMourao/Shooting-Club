import React, { useState, useEffect } from "react";
import InputMask from "react-input-mask";
import { cadastrarArmas } from "../../../../Services/ArmasService/ArmasService";
import FormErrors from "../../../Utils/FormErrors/FormErrors";
import { ParseFormErrorsGun } from "../../../Utils/ParseFormErrors/ParseFormErrorsGun";
import "./CadastroArmasModal.css";

const camposBase = [
    { name: "tipo", label: "Tipo de Arma", placeholder: "Ex: Pistola, Carabina" },
    { name: "marca", label: "Marca", placeholder: "Ex: Taurus, Glock" },
    { name: "calibre", label: "Calibre", placeholder: "Ex: .40, 9mm" },
    { name: "numeroSerie", label: "Número de Série", placeholder: "Ex: ABC123456" },
    {
        name: "tipoPosse",
        label: "Tipo de Posse",
        type: "select",
        options: [
            { value: "", label: "Selecione..." },
            { value: "0", label: "Exército" },
            { value: "1", label: "Polícia Federal" },
            { value: "2", label: "Porte Pessoal" },
        ],
    },
];

const camposPorPosse = {
    "0": [
        { name: "numeroSigma", label: "Número SIGMA" },
        { name: "dataExpedicaoCRAF", label: "Data Expedicao CRAF", type: "date" },
        { name: "validadeCRAF", label: "Validade CRAF", type: "date" },
        { name: "localRegistro", label: "Local de Registro" },
        { name: "numeroGTE", label: "Número GTE" },
        { name: "validadeGTE", label: "Validade GTE", type: "date" },
    ],
    "1": [
        { name: "numeroRegistroPF", label: "Número Registro PF" },
        { name: "numeroSinarm", label: "Número SINARM" },
        { name: "numeroNotaFiscal", label: "Número Nota Fiscal" },
        { name: "dataValidadePF", label: "Data Validade PF", type: "date" },
    ],
    "2": [
        { name: "numeroCertificado", label: "Número Certificado" },
        { name: "dataCertificacao", label: "Data da Certificação", type: "date" },
        { name: "validadeCertificacao", label: "Validade Certificação", type: "date" },
    ],
};

const gerarEstadoInicial = () => {
    const base = { isArmaClube: false, cpf_proprietario: "" };
    camposBase.forEach(({ name }) => {
        if (name !== "cpf_proprietario") {
            base[name] = "";
        }
    });
    Object.values(camposPorPosse).flat().forEach(({ name }) => (base[name] = ""));
    return base;
};

const CadastroArmasModal = ({ isOpen, onClose, onSuccess }) => {
    const [form, setForm] = useState(gerarEstadoInicial);
    const [errors, setErrors] = useState(null);

    useEffect(() => {
        if (!isOpen) {
            setErrors(null);
            setForm(gerarEstadoInicial());
        }
    }, [isOpen]);

    const handleChange = ({ target: { name, value, type, checked } }) => {
        setForm(prev => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));

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

        camposBase.forEach(({ name }) => {
            if (!form[name] || form[name].toString().trim() === "") {
                erros[name] = ["Campo obrigatório"];
            }
        });

        if (!form.isArmaClube && (!form.cpf_proprietario || form.cpf_proprietario.toString().trim() === "")) {
            erros.cpf_proprietario = ["Campo obrigatório"];
        }


        const posse = form.tipoPosse;
        if (posse in camposPorPosse) {
            camposPorPosse[posse].forEach(({ name }) => {
                if (!form[name] || form[name].toString().trim() === "") {
                    erros[name] = ["Campo obrigatório"];
                }
            });
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

        const tipoPosseInt = parseInt(form.tipoPosse);
        const payloadBase = {
            tipoPosse: tipoPosseInt,
            tipo: form.tipo,
            marca: form.marca,
            calibre: form.calibre,
            numeroSerie: form.numeroSerie,
            cpf_proprietario: form.isArmaClube ? "" : form.cpf_proprietario,
        };

        let payloadEspecifico = {};
        switch (tipoPosseInt) {
            case 0:
                payloadEspecifico = {
                    numeroSigma: form.numeroSigma,
                    dataExpedicaoCRAF: form.dataExpedicaoCRAF,
                    validadeCRAF: form.validadeCRAF,
                    localRegistro: form.localRegistro,
                    numeroGTE: form.numeroGTE,
                    validadeGTE: form.validadeGTE,
                };
                break;
            case 1:
                payloadEspecifico = {
                    numeroRegistroPF: form.numeroRegistroPF,
                    numeroSinarm: form.numeroSinarm,
                    numeroNotaFiscal: form.numeroNotaFiscal,
                    dataValidadePF: form.dataValidadePF,
                };
                break;
            case 2:
                payloadEspecifico = {
                    numeroCertificado: form.numeroCertificado,
                    validadeCertificacao: form.validadeCertificacao,
                    dataCertificacao: form.dataCertificacao,
                };
                break;
            default:
                alert("Tipo de posse inválido!");
                return;
        }

        const payload = {
            ...payloadBase,
            ...payloadEspecifico,
        };

        try {
            await cadastrarArmas(payload);
            alert("Arma cadastrada com sucesso!");
            onClose();
            if (onSuccess) onSuccess();
            setForm(gerarEstadoInicial());
            setErrors(null);
        } catch (error) {
            const parsedErrors = ParseFormErrorsGun(error);
            setErrors(parsedErrors);

            if (parsedErrors.general && parsedErrors.general.length > 0) {
                alert(parsedErrors.general.join('\n'));
            }
        }
    };

    if (!isOpen) return null;

    return (
        <div
            className="modal-overlay cadastro-armas-modal"
            onClick={(e) => {
                if (e.target.classList.contains("modal-overlay")) {
                    onClose();
                    setErrors(null);
                }
            }}
        >
            <div className="modal-box" onClick={(e) => e.stopPropagation()}>
                <h2>Cadastro de Armas</h2>
                <p>Preencha as informações abaixo para cadastrar uma arma</p>

                <form onSubmit={handleSubmit} className="modal-form" noValidate>
                    <div className="form-row">
                        <div className="form-group form-control-checkbox"> 
                            <input
                                type="checkbox"
                                id="isArmaClube"
                                name="isArmaClube"
                                checked={form.isArmaClube}
                                onChange={handleChange}
                            />
                            <label htmlFor="isArmaClube">É arma do clube?</label>
                        </div>

                        {!form.isArmaClube && (
                            <div key="cpf_proprietario" className="form-group">
                                <label htmlFor="cpf_proprietario">CPF do Proprietário:</label>
                                <InputMask
                                    mask="999.999.999-99"
                                    id="cpf_proprietario"
                                    name="cpf_proprietario"
                                    value={form.cpf_proprietario}
                                    onChange={handleChange}
                                    className={errors?.cpf_proprietario ? "input-error" : ""}
                                    placeholder="Ex: 123.456.789-00"
                                />
                                <FormErrors errors={errors} field="cpf_proprietario" />
                            </div>
                        )}
                    </div>

                    <div className="form-row">
                        {camposBase.map(({ name, label, placeholder, type = "text", mask, options }) => {

                            if (type === "select") {
                                return (
                                    <div key={name} className="form-group">
                                        <label htmlFor={name}>{label}:</label>
                                        <select
                                            id={name}
                                            name={name}
                                            value={form[name]}
                                            onChange={handleChange}
                                            className={errors?.[name] ? "input-error" : ""}
                                        >
                                            {options.map(({ value, label }) => (
                                                <option key={value} value={value}>
                                                    {label}
                                                </option>
                                            ))}
                                        </select>
                                        <FormErrors errors={errors} field={name} />
                                    </div>
                                );
                            }

                            return (
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
                                            placeholder={placeholder || ""}
                                        />
                                    ) : (
                                        <input
                                            type={type}
                                            id={name}
                                            name={name}
                                            value={form[name]}
                                            onChange={handleChange}
                                            className={errors?.[name] ? "input-error" : ""}
                                            placeholder={placeholder || ""}
                                        />
                                    )}
                                    <FormErrors errors={errors} field={name} />
                                </div>
                            );
                        })}
                    </div>

                    {form.tipoPosse && form.tipoPosse in camposPorPosse && (
                        <div className="form-row">
                            {camposPorPosse[form.tipoPosse].map(({ name, label, type = "text", mask }) => (
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
                    )}

                    <div className="modal-buttons">
                        <button type="submit" className="btn-submit">
                            Cadastrar
                        </button>
                        <button
                            type="button"
                            className="btn-cancel"
                            onClick={() => {
                                onClose();
                                setErrors(null);
                            }}
                        >
                            Cancelar
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default CadastroArmasModal;