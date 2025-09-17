import React from "react";
import "./FormErrors.css";

const FormErrors = ({ errors, field }) => {
  if (!errors) return null;

  if (field && errors[field]) {
    const messages = Array.isArray(errors[field]) ? errors[field] : [errors[field]];
    return (
      <div className="field-error">
        <ul>
          {messages.map((msg, idx) => (
            <li key={idx}>{msg}</li>
          ))}
        </ul>
      </div>
    );
  }

  if (!field && errors.general && errors.general.length > 0) {
    return (
      <div className="form-errors">
        <ul>
          {errors.general.map((msg, idx) => (
            <li key={idx}>{msg}</li>
          ))}
        </ul>
      </div>
    );
  }

  return null;
};

export default FormErrors;
