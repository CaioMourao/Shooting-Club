import { Outlet } from "react-router-dom";
import { SidebarContext } from "../../Context/SidebarContext/SidebarContext"; // Apenas importe SidebarContext
import Sidebar from "./Sidebar/Sidebar";
import Topbar from "./Topbar/Topbar";
import { useContext } from "react"; 
const Layout = () => {
    const { sidebarOpen, setSidebarOpen } = useContext(SidebarContext);

    const toggleSidebar = () => {
        setSidebarOpen(!sidebarOpen);
    };

    return (
        <div className="app-container">
            <Sidebar sidebarOpen={sidebarOpen} />
            <main className="main-content">
                <Topbar toggleSidebar={toggleSidebar} sidebarOpen={sidebarOpen} />
                <div className="page-content">
                    <Outlet />
                </div>
            </main>
        </div>
    );
};

export default Layout;