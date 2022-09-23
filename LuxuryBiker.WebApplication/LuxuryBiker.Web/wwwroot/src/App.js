import React, { useState, useEffect } from 'react';
import Axios from 'axios';
import {
	BrowserRouter as Router,
	Route,
	Routes
} from 'react-router-dom';
import {
	setToken,
	getToken,
	initAxiosInterceptors,
    deleteToken
} from './helpers/auth-helpers';

import Login from './views/Login';
import Signup from './views/Signup';
import Layout from './components/Layout/Layout';
import Home from './views/Home';
import Loading from "./components/Loading";

initAxiosInterceptors();

export default function App () {
    const [usuario, setUsuario] = useState(null);
    const [cargandoUsuario, setCargandoUsuario] = useState(true);

    useEffect (() => {
        const cargaUsuario = async () => {
            if (!getToken()) {
               setCargandoUsuario(false);
                return;
            }
            
            try {
                const { data: usuario } = await Axios.get('/Usuarios/Whoami')
                setUsuario(usuario);
                setCargandoUsuario(false);
            } catch (error) {
                console.log(error)
            }
        }

        cargaUsuario();
    }, []);

    const login = async (username, password, rememberme) => {
        const { data: usuario } = await Axios.post('/Usuarios/Login',
            {
                UserName: username,
                PasswordHash: password,
                rememberme: rememberme
            }
        );
        setUsuario(usuario);
        setToken(usuario.token);
    }

    const signup = async (usuario) => {
        const { data } = await Axios.post("/api/usuarios/signup", usuario);
        setUsuario(data);
        setToken(data.token);
    };

	const logout = () => {
		setUsuario(null);
		deleteToken();
	};

  if (cargandoUsuario) {
    return (
        <Loading />
    );
  }

    return (
      <Router>
        {usuario ? (
          // si el usuario esta logueado se renderiza layout y habilitan rutas
          <LoginRoutes
              usuario={usuario}
              logout={logout}
          />
        ) : (
          // si el usuario no esta logueado solo se habilitan rutas de registro y login
          <LogoutRoutes login={login} signup={signup} />
        )}
		  </Router>
	);
}

const LoginRoutes = ({ usuario, logout }) => {
  return (
    <Routes>
      <Route
        path="/"
        element={<Layout usuario={usuario} logout={logout} replace><Home /></Layout>}
      />
    </Routes>
  );
};
  
const LogoutRoutes = ({ login, signup }) => {
    return (
        <Routes>
            <Route
                path="/signup"
                element={<Signup signup={signup} />}
            />
            <Route path='/'
                element={<Login loggin={login} />}
            />
        </Routes>
    );
};