import { apiFetch } from "../Api/Api";

export function cadastrarUsuario(dadosUsuario) {
  return apiFetch("/api/usuario", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(dadosUsuario),
  });
}

export function filtrarUsuarios(filtro) {
  return apiFetch("/api/usuario/filter", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(filtro),
  });
}

export function getUsuarioProfile() {
  return apiFetch("/api/usuario", {
    method: "GET",
  });
}

export function getUsuarioById(id) {
  return apiFetch(`/api/usuario/${id}`, {
    method: "GET",
  });
}

export function atualizarUsuario(usuario) {
  return apiFetch(`/api/usuario/${usuario.id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(usuario),
  });
}

export function deletarUsuario(id) {
  return apiFetch(`/api/usuario/${id}`, {
    method: "DELETE",
  });
}

export function atualizarUsuarioProfile(dadosAtualizados) {
  return apiFetch("/api/usuario", {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(dadosAtualizados),
  });
}

export function changeSenha(dados) {
  return apiFetch("/api/usuario/change-password", {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(dados),
  });
}
