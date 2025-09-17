import React, { useState, useContext } from "react";
import "./Sidebar.css";
import { Link, useNavigate } from "react-router-dom";
import {
    FaUser,
    FaSignInAlt,
    FaCrosshairs,
    FaBuilding,
    FaUsers,
    FaCheckCircle,
    FaBook,
    FaExchangeAlt,
    FaBox,
    FaUserCircle,
    FaAngleDown,
    FaChartLine,
    FaCalendarAlt,
} from "react-icons/fa";
import { SidebarContext } from "../../../Context/SidebarContext/SidebarContext"; // Continua importando o contexto
import { useAuth } from "../../../Hooks/useAuth/useAuth";

const menuItems = [
    { icon: <FaUser />, text: "Usuários", path: "/usuario" },
    { icon: <FaCrosshairs />, text: "Armas", path: "/armas" },
    { icon: <FaBuilding />, text: "Clube", path: "/clube" },
    { icon: <FaUsers />, text: "Associados", path: "/associados" },
    { icon: <FaCheckCircle />, text: "Habitualidades", path: "/habitualidades" },
    { icon: <FaBook />, text: "Acervo", path: "/acervo" },
    { icon: <FaExchangeAlt />, text: "Empréstimo de armas", path: "/emprestimo" },
    { icon: <FaBox />, text: "Cadastro de itens", path: "/itens" },
    { icon: <FaUserCircle />, text: "Perfil", path: "/perfil" },
];

const Sidebar = () => {
    const { sidebarOpen } = useContext(SidebarContext); 
    const [submenuOpen, setSubmenuOpen] = useState(false);
    const { logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate("/");
    };

    return (
        <div className={`sidebar ${sidebarOpen ? "open" : "collapsed"}`}> 
            <div className="sidebar-header">
                <Link
                    to="/home"
                    className="sidebar-logo-link"
                    style={{
                        display: "flex",
                        alignItems: "center",
                        gap: "1rem",
                        textDecoration: "none",
                        color: "inherit",
                    }}
                >
                    <img src="/logo.jpeg" alt="Logo" className="sidebar-logo" />
                    {sidebarOpen && (
                        <h1 className="sidebar-title">
                            CLUBE DE TIRO ESPORTIVO DE CRATEÚS
                        </h1>
                    )}
                </Link>
            </div>

            <ul className="sidebar-menu">
                {menuItems.map((item, idx) => (
                    <li key={idx} className="sidebar-item">
                        <Link to={item.path} className="sidebar-link">
                            {item.icon}
                            {sidebarOpen && <span className="sidebar-text">{item.text}</span>}
                        </Link>
                    </li>
                ))}

                <li className="has-submenu">
                    <div
                        className="submenu-header"
                        onClick={() => setSubmenuOpen(!submenuOpen)}
                        style={{ userSelect: "none" }}
                    >
                        <FaBook />
                        {sidebarOpen && <span>Registros anuais</span>}
                        <FaAngleDown className={`arrow-icon ${submenuOpen ? "rotated" : ""}`} />
                    </div>

                    {submenuOpen && (
                        <ul className="submenu">
                            <li>
                                <Link to="/r_habitualidades" className="sidebar-link">
                                    <FaChartLine />
                                    {sidebarOpen && <span>Relatório de Habitualidades</span>}
                                </Link>
                            </li>
                            <li>
                                <Link to="/vencimentos" className="sidebar-link">
                                    <FaCalendarAlt />
                                    {sidebarOpen && <span>Relatório de Vencimentos</span>}
                                </Link>
                            </li>
                        </ul>
                    )}
                </li>

                <li className="sidebar-item logout-item">
                    <button className="sidebar-link logout-button" onClick={handleLogout}>
                        <FaSignInAlt />
                        {sidebarOpen && <span className="sidebar-text">Sair</span>}
                    </button>
                </li>
            </ul>
        </div>
    );
};

export default Sidebar;