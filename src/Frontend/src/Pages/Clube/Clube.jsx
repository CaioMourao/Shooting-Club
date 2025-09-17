import React, { useState, useEffect, useContext } from "react";
import { SidebarContext } from "../../Context/SidebarContext/SidebarContext";
import { ParseFormErrorsClube } from "../../Components/Utils/ParseFormErrors/ParseFormErrorsClube";
import { cadastrarClube, getClube } from "../../Services/ClubeService/ClubeService";
import { useNavigate } from "react-router-dom";
import "./Clube.css";

const Clube = () => {
    const { sidebarOpen } = useContext(SidebarContext);
    const navigate = useNavigate();
    const [form, setForm] = useState({
        nome: "",
        cnpj: "",
        certificadoRegistro: "",
        enderecoPais: "",
        enderecoEstado: "",
        enderecoCidade: "",
        enderecoBairro: "",
        enderecoRua: "",
        enderecoNumero: "",
    });
    const [errors, setErrors] = useState({});

    useEffect(() => {
        async function carregarClube() {
            try {
                const clube = await getClube();
                if (clube && Object.keys(clube).length > 0) {
                    navigate("/perfilclube");
                }
            } catch (error) {
                console.error("Erro ao buscar clube:", error);
            }
        }
        carregarClube();
    }, [navigate]);

    const formatarCNPJ = (value) => {
        const numeros = value.replace(/\D/g, "");
        return numeros
            .replace(/^(\d{2})(\d)/, "$1.$2")
            .replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3")
            .replace(/\.(\d{3})(\d)/, ".$1/$2")
            .replace(/(\d{4})(\d)/, "$1-$2")
            .substring(0, 18);
    };

    const handleChange = (e) => {
        const { name, value } = e.target;

        if (errors[name]) {
            setErrors((prev) => ({ ...prev, [name]: null }));
        }

        if (name === "cnpj") {
            const somenteNumeros = value.replace(/\D/g, "");
            const cnpjFormatado = formatarCNPJ(somenteNumeros);
            setForm({ ...form, [name]: cnpjFormatado });
            return;
        }

        if (name === "enderecoNumero") {
            const numerosApenas = value.replace(/\D/g, "");
            setForm({ ...form, [name]: numerosApenas });
            return;
        }

        const camposTexto = [
            "nome", "enderecoPais", "enderecoEstado", "enderecoCidade", "enderecoBairro"
        ];
        if (camposTexto.includes(name)) {
            const somenteLetras = value.replace(/[0-9]/g, "");
            setForm({ ...form, [name]: somenteLetras });
            return;
        }

        setForm({ ...form, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const newErrors = {};
        if (!form.nome) newErrors.nome = "O nome do clube é obrigatório.";
        if (!form.cnpj) newErrors.cnpj = "O CNPJ é obrigatório.";
        if (!form.certificadoRegistro) newErrors.certificadoRegistro = "O certificado é obrigatório.";
        if (!form.enderecoPais) newErrors.enderecoPais = "O país é obrigatório.";
        if (!form.enderecoEstado) newErrors.enderecoEstado = "O estado é obrigatório.";
        if (!form.enderecoCidade) newErrors.enderecoCidade = "A cidade é obrigatória.";
        if (!form.enderecoBairro) newErrors.enderecoBairro = "O bairro é obrigatório.";
        if (!form.enderecoRua) newErrors.enderecoRua = "A rua é obrigatória.";
        if (!form.enderecoNumero) newErrors.enderecoNumero = "O número é obrigatório.";

        if (Object.keys(newErrors).length > 0) {
            setErrors(newErrors);
            return;
        }

        try {
            await cadastrarClube(form);
            alert("Clube cadastrado com sucesso!");
            setErrors({});
            navigate("/perfilclube");
        } catch (error) {
            const parsed = ParseFormErrorsClube(error);
            setErrors(parsed);
        }
    };

    return (
        <div className="habitualidades-page">
            <div className="habitualidades-layout">
                <div className="habitualidades-content" style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}>
                    <h2>Cadastrar Clube</h2>
                    <p>Formulário para cadastro de informações do clube</p>

                    <form className="habitualidades-form" onSubmit={handleSubmit}>
                        {[
                            { name: "nome", label: "Nome do Clube", placeholder: "Ex: Clube de Tiro XYZ" },
                            { name: "cnpj", label: "CNPJ", placeholder: "Ex: 00.000.000/0001-00" },
                            { name: "certificadoRegistro", label: "Certificado de Registro", placeholder: "Digite o número do certificado" },
                            { name: "enderecoPais", label: "País", placeholder: "Ex: Brasil" },
                            { name: "enderecoEstado", label: "Estado", placeholder: "Ex: CE" },
                            { name: "enderecoCidade", label: "Cidade", placeholder: "Ex: Crateús" },
                            { name: "enderecoBairro", label: "Bairro", placeholder: "Ex: Centro" },
                            { name: "enderecoRua", label: "Rua", placeholder: "Ex: Rua Principal" },
                            { name: "enderecoNumero", label: "Número", placeholder: "Ex: 123" },
                        ].map(({ name, label, placeholder }) => (
                            <div key={name} className="form-row full">
                                <label>{label}</label>
                                <input
                                    type="text"
                                    name={name}
                                    placeholder={placeholder}
                                    value={form[name]}
                                    onChange={handleChange}
                                    className={errors[name] ? "input-error" : ""}
                                />
                                {errors[name] && <small className="error-message">{errors[name]}</small>}
                            </div>
                        ))}

                        <div className="form-row full">
                            <button type="submit" className="btn-cadastrar">Cadastrar</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default Clube;
