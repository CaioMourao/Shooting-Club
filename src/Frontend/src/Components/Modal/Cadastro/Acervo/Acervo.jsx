import React, { useState, useContext} from "react";
import Topbar from "../../../Layout/Topbar/Topbar";
import Sidebar from "../../../Layout/Sidebar/Sidebar";
import { SidebarContext } from "../../../../Context/SidebarContext/SidebarContext";
import "./Acervo.css";

const Acervo = () => {
  const {sidebarOpen, setSidebarOpen} = useContext(SidebarContext);
  const [searchTerm, setSearchTerm] = useState("");

  const acervo = [
    { tipo: "Arma", quantidade: 2, nome: "Ak-47" },
    { tipo: "Munição", quantidade: 50, nome: "7,62 x 51mm" },
    { tipo: "Arma", quantidade: 2, nome: "Ak-47" },
    { tipo: "Munição", quantidade: 50, nome: "7,62 x 51mm" },
  ];

  const filteredAcervo = acervo.filter((item) =>
    item.nome.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="acervo-page">
      <Topbar toggleSidebar={() => setSidebarOpen(!sidebarOpen)} sidebarOpen={sidebarOpen} />
      <div className="acervo-layout">
        <Sidebar sidebarOpen={sidebarOpen} />
        <div className="acervo-content" style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}>
          <div className="header-acervo">
            <h2>Acompanhamento dos acervo</h2>
            <button className="btn-cadastrar">
              Cadastrar
            </button>
          </div>

          <div className="search-bar-container">
            <span className="search-icon">🔍</span>
            <input
              type="text"
              placeholder="Pesquisar item..."
              className="search-input"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>

          <table className="acervo-table">
            <thead>
              <tr>
                <th>Tipo de item</th>
                <th>Quantidade</th>
                <th>Nome</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {filteredAcervo.map((item, index) => (
                <tr key={index}>
                  <td>{item.tipo}</td>
                  <td>{item.quantidade}</td>
                  <td>{item.nome}</td>
                  <td>
                    <button className="btn-editar" title="Editar">📝</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default Acervo;
