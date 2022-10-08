import React, { useState } from "react";
import Axios from 'axios';
import { initAxiosInterceptors } from '../../helpers/auth-helpers';
import Swal from 'sweetalert2';

initAxiosInterceptors();

export default function RegisterProducto({callback}) {
    const [form, setForm] = useState({
		Nombre: '',
		Referencia: '',
        Descripcion: ''
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
            if ((form[property] == null || form[property] == "")) {
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
            await Axios.post('/Productos/Register', form).then(response => {
                if (response.data.error) {
                    throw response;
                }

                Swal.fire(
                    '¡Excelente!',
                    response.data.mensaje,
                    'success'
                )
                
                setForm({
                    Nombre: '',
                    Referencia: '',
                    Descripcion: ''
                });

                if (callback !== void(0)) {
                    callback();
                }
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
            <h1 className="h3 mb-4 text-gray-800">Registrar Producto</h1>
            <hr className="sidebar-divider my-0" />
            <div className='box-gray mt-3'>
                <h3>Datos del producto</h3>
                <form onSubmit={handleSubmit}>
                    <div className="form-row">
                        <div className="form-group col-md-6">
                            <label htmlFor="inputNombre" className='label-red'>Nombre del producto*</label>
                            <input type="text" className="form-control" name="Nombre" id="inputNombre"
                                    value={form.Nombre} onChange={handleInputChange} placeholder="Nombre" />
                        </div>
                        <div className="form-group col-md-6">
                            <label htmlFor="inputReferencia" className='label-red'>Referencia/Modelo del producto*</label>
                            <input type="text" className="form-control" name="Referencia" id="inputReferencia"
                                    value={form.Referencia} onChange={handleInputChange} placeholder="Referencia/Modelo" />
                        </div>
                    </div>
                    <div className="form-row">
                        <div className="form-group col-md-12">
                            <label htmlFor="inputDescripcion" className='label-red'>Descripción*</label>
                            <textarea type="text" className="form-control" id="inputDescripcion" name="Descripcion"
                            rows="3"
                            value={form.Descripcion} onChange={handleInputChange} placeholder="Descripcion"></textarea>
                        </div>
                    </div>
                    <button type="submit" className="btn btn-primary col-md-3">Registrar</button>
                </form>
            </div>
        </main>
    );
}