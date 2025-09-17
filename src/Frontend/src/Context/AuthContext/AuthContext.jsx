import React, { createContext, useState, useContext, useEffect } from "react";
import { login as loginService } from "../../Services/AuthService/AuthService";

export const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [token, setToken] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const storedToken = localStorage.getItem("token");
    if (storedToken) {
      setToken(storedToken);
      setIsAuthenticated(true);
    }
    setIsLoading(false);
  }, []);

  const login = async (email, password) => {
    try {
      const response = await loginService(email, password);
      const { accessToken, refreshToken } = response.tokens;

      localStorage.setItem("token", accessToken);
      localStorage.setItem("refreshToken", refreshToken);
      setToken(accessToken);
      setIsAuthenticated(true);

      return response;
    } catch (error) {
      logout();
      throw error;
    }
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
    setToken(null);
    setIsAuthenticated(false);
  };

  const value = { isAuthenticated, token, isLoading, login, logout };

  if (isLoading) {
    return <div>Carregando...</div>;
  }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  return useContext(AuthContext);
}
