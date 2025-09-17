import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../Context/AuthContext/AuthContext";

const ProtectedRoute = () => {
  const { isAuthenticated, isLoading } = useAuth();


  if (isLoading) {
    return <div>Carregando...</div>;
  }

  if (!isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
