import { apiFetch } from "../Api/Api";

export async function getDashboardData() {
  return await apiFetch("/api/dashboard", {
    method: "GET",
  });
}
