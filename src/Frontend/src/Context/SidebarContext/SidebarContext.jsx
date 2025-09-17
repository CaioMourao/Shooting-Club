import React, { createContext, useState, useEffect } from "react";

export const SidebarContext = createContext();

export const SidebarProvider = ({ children }) => {
  const [sidebarOpen, setSidebarOpen] = useState(true);

  useEffect(() => {
    const handleResize = () => {
      if (window.innerWidth < 768) {
        setSidebarOpen(false);
      } else {
        setSidebarOpen(true);
      }
    };

    handleResize();
    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  return (
    <SidebarContext.Provider value={{ sidebarOpen, setSidebarOpen }}>
      {children}
    </SidebarContext.Provider>
  );
};
