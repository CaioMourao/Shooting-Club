import { apiFetch } from "../Api/Api";

export function cadastrarClube(dadosClube) {
  return apiFetch("/api/clube", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(dadosClube),
  });
}

export function getClube() {
  return apiFetch("/api/clube", {
    method: "GET",
  });
}

export function editarClube(dadosClube) {
  return apiFetch("/api/clube", {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(dadosClube),
  });
}
