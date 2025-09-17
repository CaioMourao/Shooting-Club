import React, { useContext } from "react";
import { SidebarContext } from "../../Context/SidebarContext/SidebarContext";
import { AiOutlineClockCircle } from "react-icons/ai";
import "./Development.css";

const Development = () => {
    const { sidebarOpen, setSidebarOpen } = useContext(SidebarContext);

    return (
        <div className="funcionalidade-page">
            <div className="funcionalidade-layout">
                <main
                    className="funcionalidade-content"
                    style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}
                >
                    <div className="mensagem-container">
                        <h2>Essa funcionalidade será implementada nas próximas versões</h2>
                        <AiOutlineClockCircle size={60} color="#555" style={{ marginTop: 20 }} />
                    </div>
                </main>
            </div>
        </div>
    );
};

export default Development;
