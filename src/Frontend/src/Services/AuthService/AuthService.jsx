import { apiFetch } from "../Api/Api";

export function login(email, senha) {
  return apiFetch("/api/login", {
    method: "POST",
    body: JSON.stringify({ email, senha }),
  });
}

export function logout() {
  localStorage.removeItem("token");
  localStorage.removeItem("refreshToken");
}

export async function refreshToken(refreshToken) {
  const response = await fetch("/api/token/refresh-token", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ refreshToken }),
  });

  if (!response.ok) {
    throw new Error("Sessão expirada.");
  }

  return await response.json();
}
