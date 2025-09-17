import { useContext, useState } from "react";
import Topbar from "../../../Components/Layout/Topbar/Topbar";
import Sidebar from "../../../Components/Layout/Sidebar/Sidebar";;
import "./R_Habitualidades.css";
import { SidebarContext } from "../../../Context/SidebarContext/SidebarContext";

const R_Habitualidades = () => {
    const {sidebarOpen, setSidebarOpen} = useContext(SidebarContext);
    const [searchTerm, setSearchTerm] = useState("");
    const r_habitualidades = [
        { tipo: "Arma", quantidade: 2, nome: "Ak-47" },
        { tipo: "Munição", quantidade: 50, nome: "7,62 x 51mm" },
        { tipo: "Arma", quantidade: 2, nome: "Ak-47" },
        { tipo: "Munição", quantidade: 50, nome: "7,62 x 51mm" },
    ];

    const filteredR_Habitualidades = r_habitualidades.filter((item) =>
        item.nome.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="r-habi-page">
            <Topbar toggleSidebar={() => setSidebarOpen(!sidebarOpen)} sidebarOpen={sidebarOpen} />
            <div className="r-habi-layout">
                <Sidebar sidebarOpen={sidebarOpen} />
                <div className={`r-habi-content ${sidebarOpen ? "expanded" : "collapsed"}`}>
                    <h2>Registro de Habitualidades</h2>
                    <p>Aqui você pode ver as Habitualidades realizadas</p>

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

                    <table className="r-habi-table">
                        <thead>
                            <tr>
                                <th>Tipo(prova/treino)</th>
                                <th>Arma</th>
                                <th>Data da Realização</th>
                                <th>Posse</th>
                            </tr>
                        </thead>
                        <tbody>
                            {filteredR_Habitualidades.map((item, index) => (
                                <tr key={index}>
                                    <td>{item.tipo}</td>
                                    <td>{item.quantidade}</td>
                                    <td>{item.nome}</td>
                                    <td> --- ---</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}; export default R_Habitualidades;