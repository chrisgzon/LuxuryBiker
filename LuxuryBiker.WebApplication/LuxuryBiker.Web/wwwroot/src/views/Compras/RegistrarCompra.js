import React, { useState } from "react";
import Axios from 'axios';
import { initAxiosInterceptors } from '../../helpers/auth-helpers';
import Swal from 'sweetalert2';
import Form from './_form';
import RegisterTercero from './../Terceros/RegisterTercero';
import RegisterProduct from './../Productos/RegisterProduct'
import ModalProvider from 'react-modal';
import ModalProducto from 'react-modal';

initAxiosInterceptors();

export default function RegistrarCompra() {
    const [data, setData] = useState(null);
    const [showModalProvider, setShowModalProvider] = useState(false);
    const [showModalProducto, setShowModalProducto] = useState(false);

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

    const getProductosAndProviders = async () => {
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

    const handleRegisterProviderOrProduct = () => {
        getProductosAndProviders();
        setShowModalProvider(false);
        setShowModalProducto(false);
    }

    if (data !== null) {
        return (
            <main>
                <h1 className="h3 mb-4 text-gray-800 col-md-6">Registrar Compra</h1>
                <hr className="sidebar-divider my-0" />
                <Form productos={data.productos} proveedores={data.proveedores} registerCompra={handleSubmitCompra.bind(this)} showModalProduct={setShowModalProducto} showModalProvider={setShowModalProvider} />
                <ModalProvider isOpen={showModalProvider} >
                    <RegisterTercero callback={handleRegisterProviderOrProduct.bind(this)} />
                    <button className="btn btn-secondary float-right" onClick={()=>{setShowModalProvider(false)}}>Cerrar</button>
                </ModalProvider>
                <ModalProducto isOpen={showModalProducto} >
                    <RegisterProduct callback={handleRegisterProviderOrProduct.bind(this)} />
                    <button className="btn btn-secondary float-right" onClick={()=>{setShowModalProducto(false)}}>Cerrar</button>
                </ModalProducto>
            </main>
        );
    } else {
        getProductosAndProviders()
    }
}