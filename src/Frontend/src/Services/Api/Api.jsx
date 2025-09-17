import { refreshToken as refreshTokenService } from "../../Services/AuthService/AuthService";

export async function apiFetch(url, options = {}) {
  let token = localStorage.getItem("token");
  const refreshToken = localStorage.getItem("refreshToken");

  let headers = {
    "Content-Type": "application/json",
    ...(token && { Authorization: `Bearer ${token}` }),
    ...options.headers,
  };

  let response = await fetch(url, { ...options, headers });

  if (response.status === 401 && refreshToken) {
    try {
      const newTokens = await refreshTokenService(refreshToken);
      token = newTokens.accessToken;
      localStorage.setItem("token", token);
      headers = {
        ...headers,
        Authorization: `Bearer ${token}`,
      };
      response = await fetch(url, { ...options, headers });
    } catch (err) {
      localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
      throw new Error("Sessão expirada. Faça login novamente.");
    }
  }

  if (!response.ok) {
    let errorMessage = "Erro desconhecido";
    try {
      const data = await response.json();
      errorMessage = data.message || JSON.stringify(data);
    } catch {
      errorMessage = await response.text();
    }
    throw new Error(errorMessage);
  }

  const text = await response.text();
  return text ? JSON.parse(text) : null;
}
