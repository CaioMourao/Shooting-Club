import Home from "../Pages/Home/Home";
import Clube from "../Pages/Clube/Clube";
import Usuario from "../Pages/Usuario/Usuario";
import Armas from "../Pages/Armas/Armas";
import Associados from "../Components/Modal/Cadastro/Associados/Associados";
import Habitualidades from "../Components/Modal/Cadastro/Habitualidades/Habitualidades";
import Acervo from "../Components/Modal/Cadastro/Acervo/Acervo";
import Emprestimo from "../Pages/Emprestimo/Emprestimo";
import Itens from "../Components/Modal/Cadastro/Itens/Itens";
import Perfil from "../Pages/Perfil/Perfil";
import R_Habitualidades from "../Pages/Relatorio/R_Habitualidades/R_Habitualidades";
import Vencimentos from "../Pages/Relatorio/Vencimentos/Vencimentos";
import Development from "../Pages/Development/Development";
import PerfilClube from "../Pages/Clube/PerfilClube";


const AppRoutes = [
  { path: "/home", element: <Home /> },
  { path: "/clube", element: <Clube /> },
  { path: "/usuario", element: <Usuario /> },
  { path: "/armas", element: <Armas /> },
    { path: "/associados", element: <Development /> },
    { path: "/habitualidades", element: <Development /> },
    { path: "/acervo", element: <Development /> },
    { path: "/emprestimo", element: <Development /> },
    { path: "/itens", element: <Development /> },
    { path: "/perfil", element: <Perfil /> },
    { path: "/r_habitualidades", element: <Development /> },
    { path: "/vencimentos", element: <Development /> },
    { path: "/perfilclube", element: <PerfilClube /> },
];

export default AppRoutes;
