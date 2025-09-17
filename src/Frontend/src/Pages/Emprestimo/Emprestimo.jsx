import React, { useContext, useState } from "react";
import Sidebar from "../../Components/Layout/Sidebar/Sidebar";
import Topbar from "../../Components/Layout/Topbar/Topbar";
import "./Emprestimo.css";
import { SidebarContext } from "../../Context/SidebarContext/SidebarContext";

const Emprestimo = () => {
  const {sidebarOpen, setSidebarOpen} = useContext(SidebarContext);

  const [form, setForm] = useState({
    diretorEmpresta: "",
    cpfDiretorEmpresta: "",
    diretorRecebe: "",
    cpfDiretorRecebe: "",
    armaSelecionada: "",
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
  };

  return (
    <div className="emprestimo-page">
      <Topbar toggleSidebar={() => setSidebarOpen((prev) => !prev)} sidebarOpen={sidebarOpen} />
      <div className="emprestimo-layout">
        <Sidebar sidebarOpen={sidebarOpen} />

        <div className="emprestimo-content" style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}>
          <h2>Empréstimo de armas</h2>

          <form className="emprestimo-form" onSubmit={handleSubmit}>
            <div className="form-row full">
              <label>Nome do atirador que irá emprestar</label>
              <input
                type="text"
                name="diretorEmpresta"
                placeholder="Ex: Marcos"
                value={form.diretorEmpresta}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-row full">
              <label>CPF do atirador</label>
              <input
                type="text"
                name="cpfDiretorEmpresta"
                placeholder="Ex: 00000000000"
                value={form.cpfDiretorEmpresta}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-row full">
              <label>Nome do atirador que irá receber</label>
              <input
                type="text"
                name="diretorRecebe"
                placeholder="Ex: Leonardo"
                value={form.diretorRecebe}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-row full">
              <label>CPF do recebedor</label>
              <input
                type="text"
                name="cpfDiretorRecebe"
                placeholder="Ex: 00000000000"
                value={form.cpfDiretorRecebe}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-row full">
              <label>Armas para empréstimo</label>
              <select
                name="armaSelecionada"
                value={form.armaSelecionada}
                onChange={handleChange}
                required
              >
                <option value="">Armas disponíveis</option>
                <option value="Glock G25">Glock G25</option>
                <option value="Taurus TH9">Taurus TH9</option>
                <option value="Imbel .40">Imbel .40</option>
                
              </select>
            </div>

            <div className="form-row full">
              <button type="submit" className="btn-oficializar">Oficializar empréstimo</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Emprestimo;
