import React, { useState } from "react";
import Axios from 'axios';
import { initAxiosInterceptors } from '../../helpers/auth-helpers';
import Swal from 'sweetalert2';
import Form from './_form';
import RegisterTercero from './../Terceros/RegisterTercero';
import ModalClient from 'react-modal';
import { Link } from "react-router-dom";

initAxiosInterceptors();

export default function RegistrarVenta() {
    const [data, setData] = useState(null);
    const [showModalClient, setShowModalClient] = useState(false);

    const validateDataVenta = (dataVenta) => {
        var result = true;
        if (dataVenta.DetallesVenta.length === 0) {
            Swal.fire(
                '¡Error!',
                'No se encontro ningun producto en la venta.',
                'error'
            );
            result = false;
        }
        return result;
    }

    const handleSubmitVenta = (dataVenta) => {  
        return new Promise(async(resolve)=>{
            if (!validateDataVenta(dataVenta)) {
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
                await Axios.post('/Ventas/Register', dataVenta).then(async response => {
                    if (response.data.error) {
                        throw response;
                    }
                    await getProductsAndClients();
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

    const getProductsAndClients = async () => {
        Swal.fire({
            title: 'Cargando...',
            allowOutsideClick: false,
            allowEscapeKey: false,
            allowEnterKey: false,
            showConfirmButton: false
        })
        var response = await Axios.get("/Ventas/GetProductsAndClients");
        setData({productos: response.data.result, clientes: response.data.extraData});
        Swal.close();
    }

    const handleRegisterProduct = () => {
        getProductsAndClients();
        setShowModalClient(false);
    }

    if (data !== null) {
        return (
            <main>
                <div className="page-header row">
                    <h1 className="col-md-6 h3 mb-4 text-gray-800 col-md-6">Registrar Venta</h1>
                    <nav aria-label="breadcrumb" className="col-md-6">
                        <ol className="breadcrumb">
                            <li className="breadcrumb-item"><Link to="/">Dashboard</Link></li>
                            <li className="breadcrumb-item active" aria-current="page">Registrar Venta</li>
                        </ol>
                    </nav>
                </div>
                <hr className="sidebar-divider my-0" />
                <Form productos={data.productos} clientes={data.clientes} registerVenta={handleSubmitVenta.bind(this)} showModalClient={setShowModalClient} />
                <ModalClient isOpen={showModalClient} >
                    <RegisterTercero callback={handleRegisterProduct.bind(this)} />
                    <button className="btn btn-secondary float-right" onClick={()=>{setShowModalClient(false)}}>Cerrar</button>
                </ModalClient>
            </main>
        );
    } else {
        getProductsAndClients()
    }
}