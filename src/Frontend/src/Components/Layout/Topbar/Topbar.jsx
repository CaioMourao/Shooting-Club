import React from "react";
import { FaBars } from "react-icons/fa";
import "./Topbar.css";

const Topbar = ({ toggleSidebar, sidebarOpen }) => {
  return (
    <header className={`topbar ${!sidebarOpen ? "collapsed" : ""}`}>
      <button className="menu-button" onClick={toggleSidebar}>
        <FaBars />
      </button>
    </header>
  );
};

export default Topbar;
