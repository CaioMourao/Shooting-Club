import React, { useEffect, useState } from "react";
import { deletarUsuario } from "../../../../Services/UserService/UserService";
import { useAuth } from "../../../../Context/AuthContext/AuthContext"; 
import "./DeletarUsuarioModal.css";

const DeletarUsuarioModal = ({ isOpen, onClose, usuario, onConfirm }) => {
    if (!isOpen || !usuario) return null;

    const handleExcluir = async () => {
        try {
            await deletarUsuario(usuario.id);
            alert(`Usuário ${usuario.nome} excluído com sucesso!`);
            onConfirm();
            onClose();
        } catch (error) {
            alert(`Erro ao excluir usuário: ${error.message}`);
        }
    };

    return (
        <div className="usuario-modal-overlay">
            <div className="usuario-modal-box">
                <h3 className="usuario-modal-title">Confirmar Exclusão</h3>
                <p className="usuario-modal-text">
                    Tem certeza que deseja excluir o usuário{" "}
                    <strong>{usuario.nome}</strong>?
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

export default DeletarUsuarioModal;