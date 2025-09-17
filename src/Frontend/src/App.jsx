import { BrowserRouter, Routes, Route } from "react-router-dom";
import appRoutes from "./Routes/AppRoutes";
import Login from "./Pages/Login/Login";
import ProtectedRoute from "./Components/ProtectedRoute/ProtectedRoute";
import Layout from "./Components/Layout/Layout";
import { SidebarProvider } from "./Context/SidebarContext/SidebarContext"; 

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Login />} />

                <Route element={<ProtectedRoute />}>
                    <Route element={<SidebarProvider><Layout /></SidebarProvider>}>
                        {appRoutes.map(({ path, element }) => (
                            <Route key={path} path={path} element={element} />
                        ))}
                    </Route>
                </Route>

                <Route path="*" element={<h1>404 - Página Não Encontrada</h1>} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;