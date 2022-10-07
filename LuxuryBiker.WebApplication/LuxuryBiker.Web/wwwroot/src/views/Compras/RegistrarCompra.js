import React, { useState } from "react";
import Axios from 'axios';
import { initAxiosInterceptors } from '../../helpers/auth-helpers';
import Swal from 'sweetalert2';
import Form from './_form';

initAxiosInterceptors();

export default function RegistrarCompra() {
    const [data, setData] = useState(null);

    const validateDataCompra = (dataCompra) => {
        var result = true;
        if (dataCompra.DetallesCompra.length === 0) {
            Swal.fire(
                '¡Error!',
                'No se encontro ningun producto en la compra.',
                'error'
            );
            result = false;
        }
        if (result && dataCompra.TerceroIdTercero === "") {
            Swal.fire(
                '¡Error!',
                'Por favor seleccione un proveedor.',
                'error'
            );
            result = false;
        }
        if (result && dataCompra.FechaCompra === "" || new Date(dataCompra.FechaCompra) > new Date()) {
            Swal.fire(
                '¡Error!',
                'La fecha ingresada es invalida.',
                'error'
            );
            result = false;
        }
        return result;
    }

    const handleSubmitCompra = (dataCompra) => {  
        return new Promise(async(resolve, reject)=>{
            if (!validateDataCompra(dataCompra)) {
                resolve(false);
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
                await Axios.post('/Compras/Register', dataCompra).then(response => {
                    if (response.data.error) {
                        throw response;
                    }
    
                    Swal.fire(
                        '¡Excelente!',
                        response.data.mensaje,
                        'success'
                    )
                    
                    resolve(true);
                });
    
            } catch(error) {
                Swal.fire(
                    '¡Error!',
                    error.data.mensaje,
                    'error'
                )
                resolve(false);
            }
        });
    }

    const getCategoriesAndCompras = async () => {
        Swal.fire({
            title: 'Cargando...',
            allowOutsideClick: false,
            allowEscapeKey: false,
            allowEnterKey: false,
            showConfirmButton: false
        })
        var response = await Axios.get("/Compras/GetProductsAndProviders");
        setData({productos: response.data.result, proveedores: response.data.extraData});
        Swal.close();
    }

    if (data !== null) {
        return (
            <main>
                <h1 className="h3 mb-4 text-gray-800">Registrar Compra</h1>
                <hr className="sidebar-divider my-0" />
                <Form productos={data.productos} proveedores={data.proveedores} registerCompra={handleSubmitCompra.bind(this)} />
            </main>
        );
    } else {
        getCategoriesAndCompras();
    }
    
}