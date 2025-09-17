import React from "react";
import "./DataGrid.css";

const DataGrid = ({
  columns = [],    
  data = [],       
  actions = null,  
  emptyMessage = "Nenhum registro encontrado", 
}) => {
  return (
    <div className="datagrid-wrapper">
      <table className="datagrid-table">
        <thead>
          <tr>
            {columns.map((col) => (
              <th key={col.accessor}>{col.header}</th>
            ))}
            {actions && <th></th>}
          </tr>
        </thead>
        <tbody>
          {data.length === 0 ? (
            <tr>
              <td colSpan={columns.length + (actions ? 1 : 0)} style={{ textAlign: "center" }}>
                {emptyMessage}
              </td>
            </tr>
          ) : (
            data.map((item, index) => (
              <tr key={index}>
                {columns.map((col) => (
                  <td key={col.accessor}>
                    {col.render
                      ? col.render(item[col.accessor], item)
                      : item[col.accessor]}
                  </td>
                ))}
                {actions && (
                  <td className="datagrid-actions">
                    {actions(item)}
                  </td>
                )}
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
};

export default DataGrid;
