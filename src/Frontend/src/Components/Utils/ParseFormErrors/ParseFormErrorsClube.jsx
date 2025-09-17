export const ParseFormErrorsClube = (error) => {
    const fieldErrors = {};

    const addFieldError = (field, msg) => {
        fieldErrors[field] = (fieldErrors[field] || []).concat(msg.trim());
    };

    const removeAccents = (str) =>
        str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");

    try {
        let errorsArray = [];

        if (error?.response?.data?.errors) {
            errorsArray = error.response.data.errors;
        } else if (error?.errors) {
            errorsArray = error.errors;
        } else if (typeof error.message === "string") {
            try {
                const parsedMessage = JSON.parse(error.message);
                if (parsedMessage.errors) {
                    errorsArray = parsedMessage.errors;
                }
            } catch {
            }
        }

        if (!Array.isArray(errorsArray) || errorsArray.length === 0) {
            addFieldError("general", "Erro inesperado ao processar a resposta do servidor.");
            return fieldErrors;
        }

        errorsArray.forEach((msg) => {
            const normalized = removeAccents(msg.toLowerCase().trim());

            if (normalized.includes("nome")) {
                addFieldError("nome", msg);
            } else if (normalized.includes("cnpj")) {
                addFieldError("cnpj", msg);
            } else if (
                normalized.includes("certificado de registro") ||
                normalized.includes("certificado") ||
                normalized.includes("registro")
            ) {
                addFieldError("certificadoRegistro", msg);
            } else if (normalized.includes("pais")) {
                addFieldError("enderecoPais", msg);
            } else if (normalized.includes("estado")) {
                addFieldError("enderecoEstado", msg);
            } else if (normalized.includes("cidade")) {
                addFieldError("enderecoCidade", msg);
            } else if (normalized.includes("bairro")) {
                addFieldError("enderecoBairro", msg);
            } else if (normalized.includes("rua")) {
                addFieldError("enderecoRua", msg);
            } else if (
                normalized.includes("numero") &&
                (normalized.includes("endereco") || normalized.includes("número"))
            ) {
                addFieldError("enderecoNumero", msg);
            } else {
                addFieldError("general", msg);
            }
        });
    } catch {
        addFieldError("general", "Erro inesperado ao processar a resposta do servidor.");
    }

    return fieldErrors;
};
