import React, { useState } from "react";
import { Link } from 'react-router-dom';
import $ from 'jquery';
import Swal from 'sweetalert2'

export default function Register ({signup}) {
    const [usuario, setUsuario] = useState({
        UserName: "",
        Nombres: "",
        Apellidos: "",
        Identificacion: "",
        PasswordHash: "",
        repeatPassword: ""
    });
    const [Alert, setAlert] = useState({isError: null, mensaje:"", show:false})
    
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
                setAlert({isError: true, mensaje: "Todos los campos son obligatorios", show:true})
                result = false;
                return;
            }
        });
        if (result && isNaN(usuario.Identificacion)) {
            setAlert({isError: true, mensaje: "La identificacion debecontener solo números", show:true})
            result = false;
        }
        if (result && usuario.PasswordHash.length <= 6) {
            setAlert({isError: true, mensaje: "La contraseña debe ser superior a 6 digitos", show:true})
            result = false;
        }
        if (result && usuario.PasswordHash != usuario.repeatPassword) {
            setAlert({isError: true, mensaje: "Las contraseñas no coinciden", show:true})
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
            Swal.fire({
                title: 'Cargando informacion ...',
                allowOutsideClick: false,
                allowEscapeKey: false,
                allowEnterKey: false,
                showConfirmButton: false
              })
            await signup(usuario);
        } catch (error) {
            Swal.close();
            setAlert({isError: error.data.error, mensaje: error.data.mensaje, show:true})
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
                                    {Alert.show && <div className={`alert ${Alert.isError ? "alert-danger" : "alert-success"} alert-dismissible fade show`} id="alert-register" role="alert">
                                        <strong>{Alert.isError ? ("Error!" ): "Excelente!"}</strong> {Alert.mensaje}
                                    </div>}
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
                                {/* <div className="text-center">
                                    <Link className="small">Olvidaste tu contraseña?</Link>
                                </div> */}
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