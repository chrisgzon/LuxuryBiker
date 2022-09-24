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
import Register from './views/Register';
import Layout from './components/Layout/Layout';
import Home from './views/Home';
import Swal from 'sweetalert2';

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
                Swal.fire({
                  title: 'Cargando informacion ...',
                  allowOutsideClick: false,
                  allowEscapeKey: false,
                  allowEnterKey: false,
                  showConfirmButton: false
                })
                const { data: response } = await (await Axios.get('/Usuarios/Whoami'))
                setCargandoUsuario(false);
                Swal.close();
                if (!response.error) {
                  setUsuario(response.result);
                }
            } catch (error) {
                console.log(error)
            }
        }

        cargaUsuario();
    }, []);

    const login = async (username, password, rememberme) => {
        await Axios.post('/Usuarios/Login',
            {
                UserName: username,
                PasswordHash: password,
                rememberme: rememberme
            }
        ).then((response => {
          if (response.data.error) {
            throw response;
          }
          setUsuario(response.data.result);
          setToken(response.data.result.token);
        }));
    }

    const signup = async (usuario) => {
        await Axios.post("/Usuarios/register", usuario)
        .then((response)=>{
            throw response;
        });
    };

	const logout = () => {
		setUsuario(null);
		deleteToken();
	};

  if (cargandoUsuario) {
    return(
      <div></div>
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
                path="/register"
                element={<Register signup={signup} />}
            />
            <Route path='/'
                element={<Login loggin={login} />}
            />
        </Routes>
    );
};