import React, { useEffect, useState, useContext } from "react";
import { getClube } from "../../Services/ClubeService/ClubeService";
import { SidebarContext } from "../../Context/SidebarContext/SidebarContext";
import EditarPerfilClubeModal from "../../Components/Modal/Editar/EditarPerfilClubeModal/EditarPerfilClubeModal";
import "./PerfilClube.css";

const PerfilClube = () => {
    const { sidebarOpen } = useContext(SidebarContext);
    const [clube, setClube] = useState(null);
    const [modalAberto, setModalAberto] = useState(false);

    const carregarClube = async () => {
        try {
            const dados = await getClube();
            setClube(dados);
        } catch (error) {
            alert.error("Erro ao carregar dados do clube:", error);
        }
    };

    useEffect(() => {
        carregarClube();
    }, []);

    if (!clube) return <div>Carregando...</div>;

    return (
        <div className="perfil-clube-page">
            <div
                className="perfil-clube-layout"
                style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}
            >
                <div className="perfil-clube-card">
                    <h2>Informações do Clube</h2>

                    <div className="info-grid">
                        <div className="info-item">
                            <strong>Nome:</strong>
                            <span>{clube.nome}</span>
                        </div>
                        <div className="info-item">
                            <strong>CNPJ:</strong>
                            <span>{clube.cnpj}</span>
                        </div>
                        <div className="info-item">
                            <strong>Certificado:</strong>
                            <span>{clube.certificadoRegistro}</span>
                        </div>
                        <div className="info-item">
                            <strong>País:</strong>
                            <span>{clube.enderecoPais}</span>
                        </div>
                        <div className="info-item">
                            <strong>Estado:</strong>
                            <span>{clube.enderecoEstado}</span>
                        </div>
                        <div className="info-item">
                            <strong>Cidade:</strong>
                            <span>{clube.enderecoCidade}</span>
                        </div>
                        <div className="info-item">
                            <strong>Bairro:</strong>
                            <span>{clube.enderecoBairro}</span>
                        </div>
                        <div className="info-item">
                            <strong>Rua:</strong>
                            <span>{clube.enderecoRua}</span>
                        </div>
                        <div className="info-item">
                            <strong>Número:</strong>
                            <span>{clube.enderecoNumero}</span>
                        </div>
                    </div>

                    <div className="acoes">
                        <button
                            className="editar-btn"
                            onClick={() => setModalAberto(true)}
                        >
                            Editar Clube
                        </button>
                    </div>
                </div>
            </div>

            <EditarPerfilClubeModal
                isOpen={modalAberto}
                onClose={() => setModalAberto(false)}
                dadosIniciais={clube}
                onSuccess={() => {
                    carregarClube(); 
                    setModalAberto(false); 
                }}
            />
        </div>
    );
};

export default PerfilClube;
