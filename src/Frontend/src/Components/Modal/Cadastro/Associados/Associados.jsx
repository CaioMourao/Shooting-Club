import React, { useState, useContext } from "react";
import Topbar from "../../../Layout/Topbar/Topbar";
import Sidebar from "../../../Layout/Sidebar/Sidebar";
import { SidebarContext } from "../../../../Context/SidebarContext/SidebarContext";
import "./Associados.css";

const Associados = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const { sidebarOpen, setSidebarOpen } = useContext(SidebarContext);

  const associados = [
    {
      nome: "Leonardo Oliveira de Araújo",
      cpf: "042.666.777-88",
      filiacao: "88888888888888888888",
    },
  ];

  const filteredAssociados = associados.filter((item) =>
    item.nome.toLowerCase().includes(searchTerm.toLowerCase()) ||
    item.cpf.toLowerCase().includes(searchTerm.toLowerCase()) ||
    item.filiacao.toLowerCase().includes(searchTerm.toLowerCase()) 
  );

  return (
    <div className="associados-page">
      <Topbar toggleSidebar={() => setSidebarOpen(!sidebarOpen)} sidebarOpen={sidebarOpen} />
      <div className="associados-layout">
        <Sidebar sidebarOpen={sidebarOpen} />
        <div className="associados-content" style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}>
          <div className="header-associados">
            <h2>Acompanhamento dos associados</h2>
            <button className="btn-cadastrar">Cadastrar</button>
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

          <table className="associados-table">
            <thead>
              <tr>
                <th>Nome</th>
                <th>CPF</th>
                <th>Nº de filiação</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {filteredAssociados.map((assoc, index) => (
                <tr key={index}>
                   <td>{assoc.nome}</td>
                   <td>{assoc.cpf}</td>
                   <td>{assoc.filiacao}</td>
                   <td>
                     <button className="btn-editar" title="Editar">
                       📝
                     </button>
                   </td>
                 </tr>
               ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  )
};

export default Associados;
