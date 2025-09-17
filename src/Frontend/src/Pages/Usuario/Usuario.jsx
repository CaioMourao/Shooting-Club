import React, { useContext, useState } from "react";
import Sidebar from "../../Components/Layout/Sidebar/Sidebar";
import { SidebarContext } from "../../Context/SidebarContext/SidebarContext";
import CadastroUsuarioModal from "../../Components/Modal/Cadastro/CadastroUsuarioModal/CadastroUsuarioModal";
import EditarUsuarioModal from "../../Components/Modal/Editar/EditarUsuarioModal/EditarUsuarioModal";
import InfoUsuarioModal from "../../Components/Info/InfoUsuarioModal/InfoUsuarioModal";
import DeletarUsuarioModal from "../../Components/Modal/Deletar/DeletarUsuarioModal/DeletarUsuarioModal";
import { filtrarUsuarios } from "../../Services/UserService/UserService";
import DataGrid from "../../Components/DataGrid/DataGrid";
import SearchBar from "../../Components/SearchBar/SearchBar";

import { FiFilter, FiCheckSquare, FiEye, FiEdit, FiTrash2 } from "react-icons/fi";

import "./Usuario.css";

const INITIAL_FORM_STATE = {
  nome: "",
  cpf: "",
  nascimento: "",
  pais: "",
  estado: "",
  bairro: "",
  rua: "",
  numero: "",
  filiacao: "",
  dataFiliacao: "",
  ultimaRenovacao: "",
  numeroCr: "",
  vencimentoCr: "",
};

const Usuario = () => {
  const { sidebarOpen, setSidebarOpen } = useContext(SidebarContext);

  const [searchTerm, setSearchTerm] = useState("");
  const [searchField, setSearchField] = useState("nome");
  const [proximoExpiracao, setProximoExpiracao] = useState(false);
  const [buscaRealizada, setBuscaRealizada] = useState(false);

  const [modalCadastroOpen, setModalCadastroOpen] = useState(false);
  const [modalEditarOpen, setModalEditarOpen] = useState(false);
  const [modalInfoOpen, setModalInfoOpen] = useState(false);
  const [modalDeletarOpen, setModalDeletarOpen] = useState(false);

  const [usuarios, setUsuarios] = useState([]);
  const [usuarioSelecionadoEditar, setUsuarioSelecionadoEditar] = useState(null);
  const [usuarioSelecionadoInfo, setUsuarioSelecionadoInfo] = useState(null);
  const [usuarioSelecionadoDeletar, setUsuarioSelecionadoDeletar] = useState(null);

  const [form, setForm] = useState(INITIAL_FORM_STATE);

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleFormReset = () => {
    setForm(INITIAL_FORM_STATE);
  };

  const handleSubmitCadastro = (e) => {
    e.preventDefault();
    setModalCadastroOpen(false);
    handleFormReset();
  };

  const buscarUsuarios = async () => {
    try {
      const filtro = {
        nome: searchField === "nome" ? searchTerm : "",
        cpf: searchField === "cpf" ? searchTerm : "",
        email: searchField === "email" ? searchTerm : "",
        proximoExpiracao,
      };

      const response = await filtrarUsuarios(filtro);
      setUsuarios(response.usuarios || []);
    } catch (error) {
      setUsuarios([]);
    } finally {
      setBuscaRealizada(true);
    }
  };

  const abrirModalEditar = (usuarioId) => {
    setUsuarioSelecionadoEditar(usuarioId);
    setModalEditarOpen(true);
  };

  const abrirModalInfo = (usuarioId) => {
    setUsuarioSelecionadoInfo(usuarioId);
    setModalInfoOpen(true);
  };

  const abrirModalDeletar = (usuario) => {
    setUsuarioSelecionadoDeletar(usuario);
    setModalDeletarOpen(true);
  };

  const fecharModalEditar = () => {
    setModalEditarOpen(false);
    setUsuarioSelecionadoEditar(null);
  };

  const fecharModalInfo = () => {
    setModalInfoOpen(false);
    setUsuarioSelecionadoInfo(null);
  };

  const fecharModalDeletar = () => {
    setModalDeletarOpen(false);
    setUsuarioSelecionadoDeletar(null);
  };

  return (
    <div className="usuario-page">
      <div className="usuario-layout">
        <main className="usuario-content" style={{ marginLeft: sidebarOpen ? 250 : 70 }}>
          <header className="header-usuario">
            <div className="titulo-descricao">
              <h2>Cadastro do Usuário</h2>
              <p className="descricao-usuario">
                Aqui você pode cadastrar usuário e editar informações sobre ele
              </p>
            </div>

            <div className="botoes-superiores-inline">
              <button className="btn-pesquisar" onClick={buscarUsuarios}>
                Pesquisar
              </button>
              <button className="btn-cadastrar" onClick={() => setModalCadastroOpen(true)}>
                Cadastrar
              </button>
            </div>
          </header>

          <section className="filtros-container">
            <FiFilter className="filter-icon" />

            <select
              value={searchField}
              onChange={(e) => setSearchField(e.target.value)}
              className="search-select"
            >
              <option value="nome">Nome</option>
              <option value="cpf">CPF</option>
              <option value="email">Email</option>
            </select>

            <label className="search-checkbox-label">
              <input
                type="checkbox"
                checked={proximoExpiracao}
                onChange={() => setProximoExpiracao((v) => !v)}
              />
              Próxima Expiração
              <FiCheckSquare className="checkbox-icon" />
            </label>
          </section>

          <SearchBar
            mask={searchField === "cpf" ? "999.999.999-99" : undefined}
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            onSearch={buscarUsuarios}
            placeholder={`Pesquisar por ${searchField}...`}
          />

          {buscaRealizada && (
            <section className="table-wrapper">
              <DataGrid
                columns={[
                  { header: "Nome", accessor: "nome" },
                  { header: "Email", accessor: "email" },
                  { header: "CPF", accessor: "cpf" },
                ]}
                data={usuarios}
                actions={(usuario) => (
                  <>
                    <button className="btn-visualizar" onClick={() => abrirModalInfo(usuario.id)}>
                      <FiEye />
                    </button>
                    <button className="btn-editar" onClick={() => abrirModalEditar(usuario.id)}>
                      <FiEdit />
                    </button>
                    <button className="btn-deletar" onClick={() => abrirModalDeletar(usuario)}>
                      <FiTrash2 />
                    </button>
                  </>
                )}
                emptyMessage="Nenhum usuário encontrado."
              />
            </section>
          )}

          <CadastroUsuarioModal
            isOpen={modalCadastroOpen}
            onClose={() => setModalCadastroOpen(false)}
            form={form}
            handleChange={handleFormChange}
            handleSubmit={handleSubmitCadastro}
            onSuccess={buscarUsuarios}
          />

          <EditarUsuarioModal
            isOpen={modalEditarOpen}
            onClose={fecharModalEditar}
            usuarioId={usuarioSelecionadoEditar}
            onSuccess={buscarUsuarios}
          />

          <InfoUsuarioModal
            isOpen={modalInfoOpen}
            onClose={fecharModalInfo}
            usuarioId={usuarioSelecionadoInfo}
          />

          <DeletarUsuarioModal
            isOpen={modalDeletarOpen}
            onClose={fecharModalDeletar}
            usuario={usuarioSelecionadoDeletar}
            onConfirm={buscarUsuarios}
          />
        </main>
      </div>
    </div>
  );
};

export default Usuario;
