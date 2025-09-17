import React, { useState, useContext } from "react";
import { SidebarContext } from "../../Context/SidebarContext/SidebarContext";
import CadastroArmasModal from "../../Components/Modal/Cadastro/CadastroArmasModal/CadastroArmasModal";
import EditarArmasModal from "../../Components/Modal/Editar/EditarArmasModal/EditarArmasModal";
import DeletarArmasModal from "../../Components/Modal/Deletar/DeletarArmasModal/DeletarArmasModal";
import SearchBar from "../../Components/SearchBar/SearchBar";
import DataGrid from "../../Components/DataGrid/DataGrid";

import { filtrarArmas, buscarArmaPorId } from "../../Services/ArmasService/ArmasService";

import {
  FiFilter,
  FiCheckSquare,
  FiEdit,
  FiTrash2,
} from "react-icons/fi";

import "./Armas.css";

const Armas = () => {
  const { sidebarOpen } = useContext(SidebarContext);

  const [armas, setArmas] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [campoFiltro, setCampoFiltro] = useState("tipo");
  const [tipoPosse, setTipoPosse] = useState("");
  const [soArmasDoClube, setSoArmasDoClube] = useState(false);
  const [proximoExpiracao, setProximoExpiracao] = useState(false);
  const [pesquisou, setPesquisou] = useState(false);

  const [modalOpen, setModalOpen] = useState(false);
  const [editarModalOpen, setEditarModalOpen] = useState(false);
  const [armaSelecionada, setArmaSelecionada] = useState(null);
  const [armaParaExcluir, setArmaParaExcluir] = useState(null);
  const [excluirModalOpen, setExcluirModalOpen] = useState(false);

  const [form, setForm] = useState({
    tipo: "",
    marca: "",
    calibre: "",
    numeroSerie: "",
    tipoPosse: "",
    cpf_proprietario: "",
  });

  const formatarTipoPosse = (valor) => {
    if (typeof valor === "number") {
      switch (valor) {
        case 0: return "Exército";
        case 1: return "Polícia Federal";
        case 2: return "Porte Pessoal";
        default: return "";
      }
    }
    if (typeof valor === "string") {
      switch (valor.toLowerCase().replace(/\s/g, "")) {
        case "exercito": return "Exército";
        case "policiafederal": return "Polícia Federal";
        case "portepessoal": return "Porte Pessoal";
        default: return valor;
      }
    }
    return "";
  };

  const buscarArmas = async () => {
    try {
      const filtros = {
        tipoPosse: tipoPosse === "" ? null : Number(tipoPosse),
        tipo: campoFiltro === "tipo" ? searchTerm : null,
        marca: campoFiltro === "marca" ? searchTerm : null,
        calibre: campoFiltro === "calibre" ? searchTerm : null,
        numeroSerie: campoFiltro === "numeroSerie" ? searchTerm : null,
        soArmasDoClube,
        proximoExpiracao,
      };

      const response = await filtrarArmas(filtros);

      const armasCorrigidas = (response.armas || []).map((arma) => ({
        ...arma,
        tipoPosse: formatarTipoPosse(arma.tipoPosse),
      }));

      setArmas(armasCorrigidas);
      setPesquisou(true);
    } catch (error) {
      setArmas([]);
      setPesquisou(true);
    }
  };

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setModalOpen(false);
    setForm({
      tipo: "",
      marca: "",
      calibre: "",
      numeroSerie: "",
      tipoPosse: "",
      cpf_proprietario: "",
    });
    buscarArmas();
  };

  const abrirModalEdicao = async (arma) => {
    try {
      const dadosCompletos = await buscarArmaPorId(arma.id);
      const tipoPosseCorrigido = formatarTipoPosse(dadosCompletos.tipoPosse);

      setArmaSelecionada({
        ...dadosCompletos,
        tipoPosse: tipoPosseCorrigido,
      });

      setEditarModalOpen(true);
    } catch (error) {
      alert("Erro ao carregar dados da arma para edição");
    }
  };

  const abrirModalExclusao = (arma) => {
    setArmaParaExcluir(arma);
    setExcluirModalOpen(true);
  };

  const columns = [
    { header: "Tipo de Arma", accessor: "tipo" },
    { header: "Marca", accessor: "marca" },
    { header: "Calibre", accessor: "calibre" },
    {
      header: "Número de Série",
      accessor: "numeroSerie",
      render: (value, item) => value || item.numeroGTE || item.gte || "",
    },
    {
      header: "Tipo de Posse",
      accessor: "tipoPosse",
    },
  ];

  const renderActions = (arma) => (
    <>
      <button
        className="btn-editar-armas"
        title="Editar"
        onClick={() => abrirModalEdicao(arma)}
      >
        <FiEdit />
      </button>
      <button
        className="btn-excluir-armas"
        title="Excluir"
        onClick={() => abrirModalExclusao(arma)}
      >
        <FiTrash2 />
      </button>
    </>
  );

  return (
    <div className="armas-page">
      <div className="armas-layout">
        <div
          className="armas-content"
          style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}
        >
          <div className="header-armas">
            <div className="titulo-descricao">
              <h2>Cadastro de Armas</h2>
              <p className="descricao-armas">
                Aqui você pode cadastrar armas e editar informações sobre elas
              </p>
            </div>

            <div className="botoes-superiores-inline">
              <button className="btn-pesquisar" onClick={buscarArmas}>
                Pesquisar
              </button>
              <button
                className="btn-cadastrar"
                title="Cadastrar"
                onClick={() => setModalOpen(true)}
              >
                Cadastrar
              </button>
            </div>
          </div>

          <div className="filtros-container">
            <FiFilter className="filter-icon" />

            <select
              className="search-select"
              value={campoFiltro}
              onChange={(e) => setCampoFiltro(e.target.value)}
            >
              <option value="tipo">Nome da Arma</option>
              <option value="marca">Marca</option>
              <option value="calibre">Calibre</option>
              <option value="numeroSerie">Número de Série</option>
            </select>

            <select
              className="search-select"
              value={tipoPosse}
              onChange={(e) => setTipoPosse(e.target.value)}
            >
              <option value="">Tipo de Posse</option>
              <option value="0">Exército</option>
              <option value="1">Polícia Federal</option>
              <option value="2">Porte Pessoal</option>
            </select>

            <label className="search-checkbox-label">
              <input
                type="checkbox"
                checked={soArmasDoClube}
                onChange={() => setSoArmasDoClube(!soArmasDoClube)}
              />
              Armas do clube
              <FiCheckSquare className="checkbox-icon" />
            </label>

            <label className="search-checkbox-label">
              <input
                type="checkbox"
                checked={proximoExpiracao}
                onChange={() => setProximoExpiracao(!proximoExpiracao)}
              />
              Próxima Expiração
              <FiCheckSquare className="checkbox-icon" />
            </label>
          </div>

          <SearchBar
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            onSearch={buscarArmas}
            placeholder="Pesquisar arma..."
          />

          {pesquisou && (
            <div className="table-wrapper">
              <DataGrid
                columns={columns}
                data={armas}
                actions={renderActions}
                emptyMessage="Nenhuma arma encontrada."
              />
            </div>
          )}

          <CadastroArmasModal
            isOpen={modalOpen}
            onClose={() => setModalOpen(false)}
            form={form}
            handleChange={handleChange}
            handleSubmit={handleSubmit}
            setForm={setForm}
          />

          <EditarArmasModal
            isOpen={editarModalOpen}
            onClose={() => setEditarModalOpen(false)}
            form={
              armaSelecionada || {
                id: null,
                tipo: "",
                marca: "",
                calibre: "",
                numeroSerie: "",
                tipoPosse: "",
                cpf_proprietario: "",
              }
            }
            handleChange={(e) =>
              setArmaSelecionada({
                ...armaSelecionada,
                [e.target.name]: e.target.value,
              })
            }
            onSubmit={() => {
              setEditarModalOpen(false);
              buscarArmas();
            }}
          />

          <DeletarArmasModal
            isOpen={excluirModalOpen}
            onClose={() => setExcluirModalOpen(false)}
            arma={armaParaExcluir}
            onConfirm={() => {
              setExcluirModalOpen(false);
              buscarArmas();
            }}
          />
        </div>
      </div>
    </div>
  );
};

export default Armas;
