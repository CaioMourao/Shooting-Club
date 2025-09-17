import React, { useState, useContext } from "react";
import Topbar from "../../../Layout/Topbar/Topbar";
import Sidebar from "../../../Layout/Sidebar/Sidebar";
import { SidebarContext } from "../../../../Context/SidebarContext/SidebarContext";
import "./Itens.css";

const Item = () => {
  const {sidebarOpen, setSidebarOpen} = useContext(SidebarContext);
  const [form, setForm] = useState({
    tipo: "",
    quantidade: "",
    nome: "",
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
  };

  return (
    <div className="item-page">
      <Topbar toggleSidebar={() => setSidebarOpen((prev) => !prev)} sidebarOpen={sidebarOpen} />
      <div className="item-layout">
        <Sidebar sidebarOpen={sidebarOpen} />
        <div className="item-content" style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}>
          <h2>Cadastro de item no acervo</h2>
          <form className="item-form" onSubmit={handleSubmit}>
            <div className="form-row full">
              <label>Tipo de item</label>
              <input type="text" name="tipo" placeholder="Ex: munição" value={form.tipo} onChange={handleChange} required />
            </div>

            <div className="form-row full">
              <label>Quantidade de itens</label>
              <input type="number" name="quantidade" placeholder="Ex: 5" value={form.quantidade} onChange={handleChange} required />
            </div>

            <div className="form-row full">
              <label>Nome</label>
              <input type="text" name="nome" placeholder="Ex: Munição de calibre 22" value={form.nome} onChange={handleChange} required />
            </div>

            <div className="form-row full">
              <button type="submit" className="btn-cadastrar">Cadastrar ✍️</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Item;
