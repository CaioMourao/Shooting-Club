import { useContext } from "react";
import { AuthContext } from "../../Context/AuthContext/AuthContext";

export function useAuth() {
  return useContext(AuthContext);
}
