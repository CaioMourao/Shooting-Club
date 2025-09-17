import React, { useContext, useEffect, useState } from "react";
import { FaUserCircle } from "react-icons/fa"; 
import "./Perfil.css";
import { SidebarContext } from "../../Context/SidebarContext/SidebarContext";
import { getUsuarioProfile } from "../../Services/UserService/UserService";
import EditarPerfilModal from "../../Components/Modal/Editar/EditarPerfilModal/EditarPerfilModal";
import ChangePasswordModal from "../../Components/Modal/ChangePasswordModal/ChangePasswordModal";

const Perfil = () => {
  const { sidebarOpen, setSidebarOpen } = useContext(SidebarContext);
  const [usuario, setUsuario] = useState(null);
  const [loading, setLoading] = useState(true);
  const [modalAberto, setModalAberto] = useState(false);
  const [modalSenhaAberto, setModalSenhaAberto] = useState(false);

  useEffect(() => {
    async function fetchProfile() {
      try {
        const data = await getUsuarioProfile();
        setUsuario(data);
      } catch (error) {
        alert(
          `Erro ao buscar perfil: ${error.message || JSON.stringify(error) || "Erro desconhecido"}`
        );
      } finally {
        setLoading(false);
      }
    }
    fetchProfile();
  }, []);

  const handleAtualizarPerfil = (dadosAtualizados) => {
    setUsuario((prev) => ({
      ...prev,
      ...dadosAtualizados,
    }));
  };

  return (
    <div className="perfil-page">
      <div className="perfil-layout">
        <div
          className="perfil-content"
          style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}
        >
          <div className="perfil-card-container">
            <div className="voltar-btn" onClick={() => window.history.back()}>
              ←
            </div>

            <div className="perfil-header">
              <div className="perfil-info">
                <div className="perfil-avatar">
                  <FaUserCircle size={64} color="#555" />
                </div>
                <div>
                  <h3>{usuario?.nome || "Usuário"}</h3>
                  <p>Atirador Esportivo</p>
                  <p className="localizacao">
                    {usuario ? `${usuario.enderecoCidade} - ${usuario.enderecoEstado}` : ""}
                  </p>
                </div>
              </div>
              <div className="perfil-acoes">
                <button
                  className="editar-btn"
                  onClick={() => setModalAberto(true)}
                  disabled={!usuario}
                >
                  Editar perfil
                </button>
                <button
                  className="mudar-senha-btn"
                  onClick={() => setModalSenhaAberto(true)}
                  disabled={!usuario}
                >
                  Mudar senha
                </button>
              </div>
            </div>

            <div className="perfil-detalhes">
              <h4>Informações de contato</h4>
              <table>
                <tbody>
                  <tr>
                    <td>Nome</td>
                    <td>{usuario?.nome || "-"}</td>
                  </tr>
                  <tr>
                    <td>E-mail</td>
                    <td>{usuario?.email || "-"}</td>
                  </tr>
                  <tr>
                    <td>Nº de filiação ao clube</td>
                    <td>{usuario?.numeroFiliacao || "N/A"}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>

      <EditarPerfilModal
  isOpen={modalAberto}
  onClose={() => setModalAberto(false)}
  dadosIniciais={usuario}
  onSuccess={(dadosAtualizadosDoModal) => {
    setUsuario((prev) => ({
      ...prev,
      ...dadosAtualizadosDoModal,
    }));
    setModalAberto(false);
  }}
/>

      <ChangePasswordModal
        isOpen={modalSenhaAberto}
        onClose={() => setModalSenhaAberto(false)}
      />
    </div>
  );
};

export default Perfil;
