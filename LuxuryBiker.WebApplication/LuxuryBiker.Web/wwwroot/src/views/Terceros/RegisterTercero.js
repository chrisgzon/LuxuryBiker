import React, { useState } from "react";
import Axios from 'axios';
import { initAxiosInterceptors } from '../../helpers/auth-helpers';
import Swal from 'sweetalert2';

initAxiosInterceptors();

export default function RegisterTercero() {
    const [form, setForm] = useState({
		Nombres: '',
		Apellidos: '',
        Celular: '',
        Identificacion: '',
        Email: '',
        Direccion: '',
        TipoIdTipo: ''
	});

	const handleInputChange = (event) => {
		setForm({
			...form,
			[event.target.name]: event.target.value,
		});
	};

    // Validacion de datos de registro
    const validateData = () => {
        var result = true;
        for (var property in form) {
            if ((form[property] == null || form[property] == "") && property != "Apellidos" && property != "Email") {
                Swal.fire(
                    '¡Error!',
                    'El campo <b>' + property + '</b> no puede estar vacio',
                    'error'
                )
                result = false;
                return;
            }
        }
        return result;
    }

	const handleSubmit = async (e) => {
		e.preventDefault();
        
        if (!validateData()) {
            return;
        }

        Swal.fire({
            title: 'Cargando...',
            allowOutsideClick: false,
            allowEscapeKey: false,
            allowEnterKey: false,
            showConfirmButton: false
        })
        try {
            await Axios.post('/Terceros/Register', form).then(response => {
                if (response.data.error) {
                    throw response;
                }

                Swal.fire(
                    '¡Excelente!',
                    response.data.mensaje,
                    'success'
                )
                
                setForm({
                    Nombres: '',
                    Apellidos: '',
                    Celular: '',
                    Identificacion: '',
                    Email: '',
                    Direccion: '',
                    TipoIdTipo: ''
                });
            });

        } catch(error) {
            Swal.fire(
                '¡Error!',
                error.data.mensaje,
                'error'
            )
        }
	};

    return (
        <main>
            <h1 className="h3 mb-4 text-gray-800">Registrar Tercero <span className='text-gray-400'>(Cliente/Distribuidor)</span></h1>
            <hr className="sidebar-divider my-0" />
            <div className='box-gray mt-3'>
                <h3>Datos del tercero</h3>
                <form onSubmit={handleSubmit}>
                    <div className="form-row">
                        <div className="form-group col-md-6">
                            <label htmlFor="inputNombres" className='label-red'>Nombres*</label>
                            <input type="text" className="form-control" name="Nombres" id="inputNombres"
                                    value={form.Nombres} onChange={handleInputChange} placeholder="Nombres"  />
                        </div>
                        <div className="form-group col-md-6">
                            <label htmlFor="inputApellidos" className='label-red'>Apellidos</label>
                            <input type="text" className="form-control" id="inputApellidos" name="Apellidos"
                                    value={form.Apellidos} onChange={handleInputChange} placeholder="Apellidos" />
                        </div>
                    </div>
                    <div className="form-row">
                        <div className="form-group col-md-6">
                            <label htmlFor="inputCelular" className='label-red'>Celular*</label>
                            <input type="text" className="form-control" id="inputCelular" name="Celular"
                            value={form.Celular} onChange={handleInputChange} placeholder="Celular"  />
                        </div>
                        <div className="form-group col-md-6">
                            <label htmlFor="inputIdentificacion" className='label-red'>Identificación*</label>
                            <input type="text" className="form-control" id="inputIdentificacion" name="Identificacion"
                            value={form.Identificacion} onChange={handleInputChange} placeholder="Identificación"  />
                        </div>
                    </div>
                    <div className="form-row">
                        <div className="form-group col-md-6">
                            <label htmlFor="inputEmail" className='label-red'>Correo Electronico</label>
                            <input type="email" className="form-control" id="inputEmail" name="Email"
                            value={form.Email} onChange={handleInputChange} placeholder="Email" />
                        </div>
                        <div className="form-group col-md-6">
                            <label htmlFor="inputDireccion" className='label-red'>Dirección*</label>
                            <input type="text" className="form-control" id="inputDireccion" name="Direccion"
                            value={form.Direccion} onChange={handleInputChange} placeholder="Dirección/Barrio/Ciudad" />
                        </div>
                    </div>
                    <div className="form-row">
                        <div className="form-group col-md-12">
                            <label htmlFor="inputTipo" className='label-red'>Tipo*</label>
                            <select className="form-control" id="inputTipo" name="TipoIdTipo" value={form.TipoIdTipo} onChange={handleInputChange}>
                                <option value="" hidden>Seleccionar...</option>
                                <option value="1">Distribuidor</option>
                                <option value="2">Cliente</option>
                            </select>
                        </div>
                    </div>
                    <button type="submit" className="btn btn-primary col-md-3">Registrar</button>
                </form>
            </div>
        </main>
        );
}