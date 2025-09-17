import React from "react";
import { FiSearch } from "react-icons/fi";
import InputMask from "react-input-mask";
import "./SearchBar.css";

const SearchBar = ({
  type = "text",
  mask,
  value,
  onChange,
  onSearch,
  placeholder,
}) => {
  return (
    <div className="search-bar-container">
      <div className="search-input-group">
        <FiSearch className="search-icon" />
        {mask ? (
          <InputMask
            mask={mask}
            maskChar=""
            placeholder={placeholder}
            className="search-input"
            value={value}
            onChange={onChange}
            onKeyDown={(e) => {
              if (e.key === "Enter") onSearch();
            }}
          />
        ) : (
          <input
            type={type}
            placeholder={placeholder}
            className="search-input"
            value={value}
            onChange={onChange}
            onKeyDown={(e) => {
              if (e.key === "Enter") onSearch();
            }}
          />
        )}
      </div>
    </div>
  );
};

export default SearchBar;
