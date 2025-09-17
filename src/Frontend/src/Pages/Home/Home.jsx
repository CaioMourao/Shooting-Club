import React, { useContext, useEffect, useState } from "react";
import { SidebarContext } from "../../Context/SidebarContext/SidebarContext";
import { motion } from "framer-motion";
import { FaRegClock, FaBoxes, FaUser } from "react-icons/fa";
import { getDashboardData } from "../../Services/DashboardService/DashboardService";
import "./Home.css";

const Home = () => {
  const { sidebarOpen, setSidebarOpen } = useContext(SidebarContext);

  const [dashboard, setDashboard] = useState({
    quantArmasAtrasadas: 0,
    quantTotalArmas: 0,
    quantUsuariosNoClube: 0,
  });

  const [armaVencendo, setArmaVencendo] = useState([]);

  useEffect(() => {
    async function fetchDashboard() {
      try {
        const data = await getDashboardData();
        setDashboard({
          quantArmasAtrasadas: data.quantArmasAtrasadas,
          quantTotalArmas: data.quantTotalArmas,
          quantUsuariosNoClube: data.quantUsuariosNoClube,
        });

        if (data.armasVencendo) {
          setArmaVencendo(data.armasVencendo);
        }
      } catch (error) {
      }
    }

    fetchDashboard();
  }, []);

  return (
    <div className="home-page">
      <div className="home-layout">
        <div
          className="home-content"
          style={{ marginLeft: sidebarOpen ? "250px" : "70px" }}
        >
          <motion.div
            className="home-header"
            initial={{ opacity: 0, y: -30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6 }}
          >
            <img src="/logo.jpeg" alt="Logo" className="sidebar-logo" />
            <div>
              <h2>Clube de Tiro Esportivo de Crateús</h2>
              <p>
                Bem-vindo ao sistema de gestão do nosso clube! Aqui você
                acompanha armas, usuários e vencimentos de forma prática.
              </p>
            </div>
          </motion.div>

          <h2 className="VisaoH2">Visão Geral</h2>

          <div className="cards-container">
            <motion.div
              className="card destaque"
              initial={{ opacity: 0, y: -20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.4 }}
            >
              <FaRegClock size={40} color="#ff6b6b" />
              <h3>Armas Vencendo</h3>
              <p>{dashboard.quantArmasAtrasadas}</p>
            </motion.div>

            <motion.div
              className="card"
              initial={{ opacity: 0, y: -20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.5 }}
            >
              <FaBoxes size={40} color="#4dabf7" />
              <h3>Total de Armas</h3>
              <p>{dashboard.quantTotalArmas}</p>
            </motion.div>

            <motion.div
              className="card"
              initial={{ opacity: 0, y: -20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.7 }}
            >
              <FaUser size={40} color="#51cf66" />
              <h3>Usuários Ativos</h3>
              <p>{dashboard.quantUsuariosNoClube}</p>
            </motion.div>
          </div>

          <motion.div
            className="lembretes-container"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ delay: 0.4 }}
          >
            <h3>Vencimentos nos próximos dias</h3>
            <ul className="lembretes-lista">
              {armaVencendo.length > 0 ? (
                armaVencendo.map((arma) => (
                  <li key={arma.id}>
                    <strong>{arma.nome}</strong> - Vence em {arma.validade}
                  </li>
                ))
              ) : (
                <li>Nenhuma arma vencendo nos próximos dias.</li>
              )}
            </ul>
          </motion.div>
        </div>
      </div>
    </div>
  );
};

export default Home;
