import { useState } from "react";
import Topbar from "../../../Components/Layout/Topbar/Topbar";
import Sidebar from "../../../Components/Layout/Sidebar/Sidebar";
import "./Vencimentos.css";

const Vencimentos = () => {
    const [sidebarOpen, setSidebarOpen] = useState(true);

    return (
        <div className="vencimentos-page">
            <Topbar toggleSidebar={() => setSidebarOpen(!sidebarOpen)} sidebarOpen={sidebarOpen} />
            <div className="vencimentos-layout">
                <Sidebar sidebarOpen={sidebarOpen} />
                <div className={`vencimentos-content ${sidebarOpen ? "expanded" : "collapsed"}`}>
                    <h2>KKKKKK</h2>
                    <p>teste agora sim</p>
                </div>
            </div>

        </div>

    );
}; export default Vencimentos;