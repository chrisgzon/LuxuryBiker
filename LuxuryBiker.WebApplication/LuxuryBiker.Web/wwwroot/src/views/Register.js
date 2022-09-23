import React, { useState } from "react";
import { Link } from 'react-router-dom';
import $ from 'jquery';

export default function Register ({signup}) {
    const [usuario, setUsuario] = useState({
        UserName: "",
        Nombres: "",
        Apellidos: "",
        Identificacion: "",
        PasswordHash: "",
        repeatPassword: ""
    });
    const [error, setError] = useState(null)
    
    $("body").addClass("bg-gradient-primary");

    const handleInputChange = (e) => {
        setUsuario({
        ...usuario,
        [e.target.name]: e.target.value,
        });
    };

    // Validacion de datos de registro
    const validateData = () => {
        var result = true;
        Object.values(usuario).forEach((item) => {
            if (item == null || item == "") {
                setError("Todos los campos son obligatorios");
                result = false;
                return;
            }
        });
        if (result && isNaN(usuario.Identificacion)) {
            setError("La identificacion debecontener solo números");
            result = false;
        }
        if (result && usuario.PasswordHash.length <= 6) {
            setError("La contraseña debe ser superior a 6 digitos");
            result = false;
        }
        if (result && usuario.PasswordHash != usuario.repeatPassword) {
            setError("Las contraseñas no coinciden");
            result = false;
        }
         return result;
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!validateData()){
            return;
        }

        try {
            await signup(usuario);
        } catch (error) {
            setError(error.response.data)
            console.log(error);
        }
    };

    return (
        <div className="container">
            <div className="card o-hidden border-0 shadow-lg my-5">
                <div className="card-body p-0">
                    {/* Nested Row within Card Body */}
                    <div className="row">
                        <div className="col-lg-5 d-none d-lg-block bg-register-image"></div>
                        <div className="col-lg-7">
                            <div className="p-5">
                                <div className="text-center">
                                    <h1 className="h4 text-gray-900 mb-4">Crear una cuenta!</h1>
                                    <p>{error}</p>
                                </div>
                                <form className="user" onSubmit={handleSubmit}>
                                    <div className="form-group row">
                                        <div className="col-sm-6 mb-3 mb-sm-0">
                                            <input type="text" className="form-control form-control-user" id="ipt-nombres"
                                                value={usuario.Nombres}
                                                onChange={handleInputChange}
                                                name="Nombres"
                                                placeholder="Nombres"
                                                
                                              />
                                        </div>
                                        <div className="col-sm-6">
                                            <input type="text" className="form-control form-control-user" id="ipt-apellidos"
                                                value={usuario.Apellidos}
                                                onChange={handleInputChange}
                                                name="Apellidos"
                                                placeholder="Apellidos"
                                                
                                                />
                                        </div>
                                    </div>
                                    <div className="form-group">
                                        <input type="email" className="form-control form-control-user" id="ipt-username"
                                            value={usuario.UserName}
                                            onChange={handleInputChange}
                                            name="UserName"
                                            placeholder="Correo electronico"
                                            
                                        />
                                    </div>
                                    <div className="form-group">
                                        <input type="text" className="form-control form-control-user" id="ipt-identificacion"
                                            value={usuario.Identificacion}
                                            onChange={handleInputChange}
                                            name="Identificacion"
                                            placeholder="Identificacion"
                                            
                                        />
                                    </div>
                                    <div className="form-group row">
                                        <div className="col-sm-6 mb-3 mb-sm-0">
                                            <input type="password" className="form-control form-control-user"
                                                id="ipt-password"
                                                value={usuario.PasswordHash}
                                                onChange={handleInputChange}
                                                name="PasswordHash"
                                                placeholder="Contraseña"
                                                
                                                 />
                                        </div>
                                        <div className="col-sm-6">
                                            <input type="password" className="form-control form-control-user"
                                                id="ipt-repeatPassword" 
                                                value={usuario.repeatPassword}
                                                onChange={handleInputChange}
                                                name="repeatPassword"
                                                placeholder="Confirmar Contraseña"
                                                
                                                 />
                                        </div>
                                    </div>
                                    <button type="submit" className="btn btn-primary btn-user btn-block">
                                        Registrar Cuenta
                                    </button>
                                </form>
                                <hr />
                                <div className="text-center">
                                    <Link className="small">Olvidaste tu contraseña?</Link>
                                </div>
                                <div className="text-center">
                                    <Link className="small" to={"/"}>Ya tienes una cuenta? Ingresar!</Link>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}