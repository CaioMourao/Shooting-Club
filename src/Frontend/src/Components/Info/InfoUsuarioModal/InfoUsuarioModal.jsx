import React, { useEffect, useState } from "react";
import { getUsuarioById } from "../../../Services/UserService/UserService";
import "./InfoUsuarioModal.css";

const InfoUsuarioModal = ({ isOpen, onClose, usuarioId }) => {
  const [usuario, setUsuario] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!isOpen || !usuarioId) {
      setUsuario(null);
      setError(null);
      return;
    }

    const fetchUsuario = async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await getUsuarioById(usuarioId);
        setUsuario(data);
      } catch (err) {
        setError("Erro ao carregar dados do usuário.");
      } finally {
        setLoading(false);
      }
    };

    fetchUsuario();
  }, [isOpen, usuarioId]);

  const formatarData = (dataString) => {
    if (!dataString) return "";
    const dt = new Date(dataString);
    if (isNaN(dt)) return dataString;
    return dt.toLocaleDateString("pt-BR");
  };

  if (!isOpen) return null;

  return (
    <div
      className="modal-overlay"
      onClick={(e) => {
        if (e.target.classList.contains("modal-overlay")) onClose();
      }}
    >
      <div className="modal-card">
        <header className="modal-header">
          <h2>Perfil do Usuário</h2>
          <button
            className="btn-close"
            onClick={onClose}
            aria-label="Fechar modal"
          >
            ×
          </button>
        </header>

        <div className="modal-content">
          {loading && <p>Carregando...</p>}
          {error && <p className="error">{error}</p>}
          {!loading && !error && usuario && (
            <>
              <div className="profile-row">
                <div className="profile-label">Nome:</div>
                <div className="profile-value">{usuario.nome}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">E-mail:</div>
                <div className="profile-value">{usuario.email}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">CPF:</div>
                <div className="profile-value">{usuario.cpf}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">Data de Nascimento:</div>
                <div className="profile-value">
                  {formatarData(usuario.dataNascimento)}
                </div>
              </div>

              <hr />

              <h3>Endereço</h3>
              <div className="profile-row">
                <div className="profile-label">País:</div>
                <div className="profile-value">{usuario.enderecoPais}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">Estado:</div>
                <div className="profile-value">{usuario.enderecoEstado}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">Cidade:</div>
                <div className="profile-value">{usuario.enderecoCidade}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">Bairro:</div>
                <div className="profile-value">{usuario.enderecoBairro}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">Rua:</div>
                <div className="profile-value">{usuario.enderecoRua}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">Número:</div>
                <div className="profile-value">{usuario.enderecoNumero}</div>
              </div>

              <hr />

              <h3>Informações CR e Filiação</h3>
              <div className="profile-row">
                <div className="profile-label">Número CR:</div>
                <div className="profile-value">{usuario.cr}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">Data Vencimento CR:</div>
                <div className="profile-value">
                  {formatarData(usuario.dataVencimentoCR)}
                </div>
              </div>
              <div className="profile-row">
                <div className="profile-label">SFPC Vinculação:</div>
                <div className="profile-value">{usuario.sfpcVinculacao}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">Número Filiação:</div>
                <div className="profile-value">{usuario.numeroFiliacao}</div>
              </div>
              <div className="profile-row">
                <div className="profile-label">Data Filiação:</div>
                <div className="profile-value">
                  {formatarData(usuario.dataFiliacao)}
                </div>
              </div>
              <div className="profile-row">
                <div className="profile-label">Data Renovação Filiação:</div>
                <div className="profile-value">
                  {formatarData(usuario.dataRenovacaoFiliacao)}
                </div>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default InfoUsuarioModal;
