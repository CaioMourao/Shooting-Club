import React from "react";
import { deletarArma } from "../../../../Services/ArmasService/ArmasService";
import "./DeletarArmasModal.css";

const DeletarArmasModal = ({ isOpen, onClose, arma, onConfirm }) => {
  if (!isOpen || !arma) return null;

  const handleExcluir = async () => {
    try {
      await deletarArma(arma.id);
      alert(`Arma do tipo ${arma.tipo} excluída com sucesso!`);
      onConfirm();
      onClose();
    } catch (error) {
      alert("Erro ao excluir arma.");
    }
  };

  return (
    <div className="usuario-modal-overlay">
      <div className="usuario-modal-box">
        <h3 className="usuario-modal-title">Confirmar Exclusão</h3>
        <p className="usuario-modal-text">
          Tem certeza que deseja excluir a arma de tipo <strong>{arma.tipo}</strong>?
          Esta ação não poderá ser desfeita.
        </p>
        <div className="usuario-modal-buttons">
          <button className="btnUsuario-excluir" onClick={handleExcluir}>
            Excluir
          </button>
          <button className="btnUsuario-cancelar" onClick={onClose}>
            Cancelar
          </button>
        </div>
      </div>
    </div>
  );
};

export default DeletarArmasModal;
