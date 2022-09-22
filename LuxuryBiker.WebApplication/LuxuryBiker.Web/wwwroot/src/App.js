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
    const [alert, setAlert] = useState(null);

    useEffect (() => {
        const cargaUsuario = async () => {
            if (!getToken()) {
                return;
            }
            
            try {
                const { data: usuario } = await Axios.get('/Usuarios/Whoami')
                setUsuario(usuario);
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

    const mostrarAlert = (mensaje) => {
		setAlert(mensaje);
	};

    const ocultarAlert = () => {
        setAlert(null);
    }

	const logout = () => {
		setUsuario(null);
		deleteToken();
	};

    return (
        <Routes>
            <Alert mensaje={alert} typeAlert={""} ocultarAlert={ocultarAlert}/>
			{usuario ? (
                // si el usuario esta logueado se renderiza layout
				 <div id='wrapper'>
                    <Sidebar usuario={usuario} />
                    <div id="content-wrapper" className="d-flex flex-column">
                        {/* Main Content */}
                        <div id="content">
                            <Nav usuario={usuario} logout={logout} />
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
                // si el usuario no esta logueado renderiza el login
				<Login loggin={login} />
			)}
		</Routes>
	);
}