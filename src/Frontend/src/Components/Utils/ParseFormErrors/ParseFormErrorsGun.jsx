export const ParseFormErrorsGun = (error) => {
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

            if (normalized.includes("cpf_proprietario") || normalized.includes("cpf")) {
                addFieldError("cpf_proprietario", msg);
            } else if (normalized.includes("tipo de arma") || normalized.includes("tipo")) {
                addFieldError("tipo", msg);
            } else if (normalized.includes("marca")) {
                addFieldError("marca", msg);
            } else if (normalized.includes("calibre")) {
                addFieldError("calibre", msg);
            } else if (
                normalized.includes("numero de serie") ||
                normalized.includes("numero serie") ||
                normalized.includes("número de série")
            ) {
                addFieldError("numeroSerie", msg);
            } else if (normalized.includes("tipo de posse") || normalized.includes("posse")) {
                addFieldError("tipoPosse", msg);
            } else if (
                normalized.includes("numero sigma") ||
                normalized.includes("número sigma")
            ) {
                addFieldError("numeroSigma", msg);
            } else if (
                normalized.includes("data de expedicao do craf") ||
                normalized.includes("data de expedição do craf") ||
                normalized.includes("data expedicao craf") ||
                normalized.includes("data expedição craf") ||
                normalized.includes("expedicao do craf")
            ) {
                addFieldError("dataExpedicaoCRAF", msg);
            } else if (
                normalized.includes("data de validade do certificado") ||
                normalized.includes("validade do certificado") ||
                normalized.includes("certificado esta invalido") ||
                normalized.includes("a data de validade do certificado esta invalido")
            ) {
                addFieldError("validadeCertificacao", msg);
            } else if (
                normalized.includes("data de validade do craf") ||
                normalized.includes("data validade do craf") ||
                normalized.includes("validade craf") ||
                normalized.includes("validade do craf")
            ) {
                addFieldError("validadeCRAF", msg);
            } else if (normalized.includes("local registro")) {
                addFieldError("localRegistro", msg);
            } else if (
                normalized.includes("data de validade da gte") ||
                normalized.includes("validade gte") ||
                normalized.includes("validade da gte")
            ) {
                addFieldError("validadeGTE", msg);
            } else if (
                normalized.includes("numero registro pf") ||
                normalized.includes("número registro pf")
            ) {
                addFieldError("numeroRegistroPF", msg);
            } else if (
                normalized.includes("data da certificacao") ||
                normalized.includes("data da certificação") ||
                normalized.includes("data certificacao") ||
                normalized.includes("data de certificacao") ||
                normalized.includes("data de certificação") ||
                normalized.includes("certificacao foi realizada") ||
                normalized.includes("'data certificacao' deve ser definido")
            ) {
                addFieldError("dataCertificacao", msg);
            } else if (
                normalized.includes("numero sinarm") ||
                normalized.includes("número sinarm")
            ) {
                addFieldError("numeroSinarm", msg);
            } else if (
                normalized.includes("numero nota fiscal") ||
                normalized.includes("número nota fiscal")
            ) {
                addFieldError("numeroNotaFiscal", msg);
            } else if (
                normalized.includes("data de validade") ||
                normalized.includes("validade esta invalida") ||
                normalized.includes("data validade")
            ) {
                addFieldError("dataValidadePF", msg);
            } else if (
                normalized.includes("numero certificado") ||
                normalized.includes("número certificado")
            ) {
                addFieldError("numeroCertificado", msg);
            } else {
                addFieldError("general", msg);
            }
        });
    } catch {
        addFieldError("general", "Erro inesperado ao processar a resposta do servidor.");
    }

    return fieldErrors;
};
