import React, { useState, useEffect } from 'react';
import Axios from 'axios';
import {
	BrowserRouter as Routes,
	Route,
	Switch,
} from 'react-router-dom';
import {
	setToken,
	getToken,
	initAxiosInterceptors,
    deleteToken
} from './helpers/auth-helpers';
import Nav from './components/Layout/_Nav';
import Sidebar from './components/Layout/_Sidebar';
import Footer from './components/Layout/_Footer';
import Main from './components/Main';
import Alert from "./components/Alert";

import Login from './Login';
// import Signup from './view/Signup';
// import Upload from './view/Upload';

initAxiosInterceptors();

export default function App () {
    const [usuario, setUsuario] = useState(null);
    const [cargandoUsuario, setCargandoUsuario] = useState(true);
    const [alert, setAlert] = useState(null);

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

    const login = async (username, password) => {
        const { data: usuario } = await Axios.post('/Usuarios/Login',
            {
                UserName: username,
                PasswordHash: password
            }
        );
        setUsuario(usuario);
        setToken(usuario.token);
    }

    const signup = async (usuario) => {
		const { data } = await axios.post(
			'/api/usuarios/signup',
			usuario
		);

		setUsuario(usuario);
		setToken(usuario.token);
	};

    const mostrarAlert = (mensaje) => {
		setAlert(mensaje);
	};

	const logout = () => {
		setUsuario(null);
		deleteToken();
	};

    if (cargandoUsuario) {
		return (
			<Main center>
			</Main>
		);
	}

    return (
        <Routes>
			{usuario ? (
				 <div id='wrapper'>
                    <Sidebar />
                    <div id="content-wrapper" className="d-flex flex-column">
                        {/* Main Content */}
                        <div id="content">
                            <Nav />
                            {/* Begin Page Content  */}
                            <div className="container-fluid">
                                <Main center />
                            </div>
                        </div>
                        {/* End of Main Content  */}
                        <Footer />
                    </div> 
                </div>
			) : (
				<Login loggin={login} />
			)}
		</Routes>
	);

}