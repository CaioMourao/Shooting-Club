import React, { useState, useContext } from "react";
import Topbar from "../../../Layout/Topbar/Topbar";
import Sidebar from "../../../Layout/Sidebar/Sidebar";
import { SidebarContext } from "../../../../Context/SidebarContext/SidebarContext";
import "./Habitualidades.css";

const Habitualidades = () => {
  const {sidebarOpen, setSidebarOpen} = useContext(SidebarContext);
  const [form, setForm] = useState({
    tipo: "",
    arma: "",
    nome: "",
    tipoMunicao: "",
    quantidadeMunicao: "",
    dataInicio: "",
    dataFinalizacao: "",
    possuiArma: "",
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
  };

  return (
    <div className="habitualidades-page">
      <Topbar toggleSidebar={() => setSidebarOpen((prev) => !prev)} sidebarOpen={sidebarOpen} />
      <div className="habitualidades-layout">
        <Sidebar sidebarOpen={sidebarOpen} />

        <div className="habitualidades-content" style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}>
          <h2>Habitualidades</h2>
          <p>Aqui você pode registrar as atividades realizadas com armas, como provas e treinos.</p>

          <form className="habitualidades-form" onSubmit={handleSubmit}>
            <div className="form-row full">
              <label>Tipo</label>
              <input type="text" name="tipo" placeholder="Ex: Prova ou treino" value={form.tipo} onChange={handleChange} required />
            </div>

            <div className="form-row full">
              <label>Arma utilizada</label>
              <input type="text" name="arma" placeholder="Ex: pistola" value={form.arma} onChange={handleChange} required />
            </div>

            <div className="form-row full">
              <label>Nome</label>
              <input type="text" name="nome" placeholder="Ex: glock" value={form.nome} onChange={handleChange} required />
            </div>

            <div className="form-row full">
              <label>Tipo de munição</label>
              <input type="text" name="tipoMunicao" placeholder="Ex: 9mm" value={form.tipoMunicao} onChange={handleChange} required />
            </div>

            <div className="form-row full">
              <label>Quantidade de munições utilizadas</label>
              <input type="number" name="quantidadeMunicao" placeholder="Ex: 90" value={form.quantidadeMunicao} onChange={handleChange} required />
            </div>

            <div className="form-row full">
              <label>Data de início</label>
              <input type="date" name="dataInicio" value={form.dataInicio} onChange={handleChange} required />
            </div>

            <div className="form-row full">
              <label>Data de finalização</label>
              <input type="date" name="dataFinalizacao" value={form.dataFinalizacao} onChange={handleChange} required />
            </div>

            <div className="form-row full">
              <label>Possui arma</label>
              <select name="possuiArma" value={form.possuiArma} onChange={handleChange} required>
                <option value="">Possui arma?</option>
                <option value="sim">Sim</option>
                <option value="nao">Não</option>
              </select>
            </div>

            <div className="form-row full">
              <button type="submit" className="btn-cadastrar">Cadastrar 🖋️</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Habitualidades;
