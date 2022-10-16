import React, { useEffect, useState } from "react";
import Axios from 'axios';
import { Link } from "react-router-dom";
import Swal from 'sweetalert2';
import Table from "../../components/Table";

export default function Show() {
    const [Compras, setCompras] = useState(null);

    const getCompras = async function() {
        Swal.fire({
            title: 'Cargando...',
            allowOutsideClick: false,
            allowEscapeKey: false,
            allowEnterKey: false,
            showConfirmButton: false
        })
        try {
            await Axios.post("/Compras/GetCompras", {}).then(response => {
                if (response.data.error) {
                    throw response;
                }
                
                setCompras(response.data.result);
                Swal.close();
            });
        } catch(error) {
            Swal.fire(
                '¡Error!',
                error.data.mensaje,
                'error'
            )
        }
    }

    const Columnas = [
        {
            name:"Id",
            selector: row => row.idCompra,
            sortable: true,
        },
        {
            name:"Fecha Compra",
            selector:row=>row.fechaCompra,
            sortable: true,
        },
        {
            name:"Valor Total",
            selector:row=>row.total,
            sortable: true,
        },
        {
            name:"Estado",
            sortable: true,
            selector: function(row){
                
                if (row.estado) {
                    return (
                    <a href="/"
                        className="jsgrid-button btn btn-success">
                            Validada <i className="fas fa-check"></i>                    
                    </a>
                    )
                } else {
                    return (
                    <a href="/"
                        className="jsgrid-button btn btn-danger">
                            Cancelada <i className="fas fa-times"></i>                    
                    </a>
                    )
                }
            }
        },
        {
            name:"Acciones",
            center:true,
            selector: function(row){
                return (
                    <>
                        <a href="/" title="exportar a pdf" className="jsgrid-button jsgrid-edit-button"><i className="far fa-file-pdf"></i></a>
                        <a href="/" title="imprimir" className="jsgrid-button jsgrid-edit-button"><i className="fas fa-print"></i></a> 
                        <a href="/" title="revisar detalles" className="jsgrid-button jsgrid-edit-button"><i className="far fa-eye"></i></a>
                    </> 
                );
            }.bind(this)
        }
    ]

    if (Compras !== null) {

        return (
            <div className="content-wrapper">
                <div className="page-header row">
                    <h1 className="col-md-6 h3 mb-4 text-gray-800 col-md-6">Compras</h1>
                    <nav aria-label="breadcrumb" className="col-md-6">
                        <ol className="breadcrumb">
                            <li className="breadcrumb-item"><Link to="/">Dashboard</Link></li>
                            <li className="breadcrumb-item active" aria-current="page">Compras</li>
                        </ol>
                    </nav>
                </div>
                <div className="row">
                    <div className="col-lg-12 grid-margin stretch-card">
                            <div className="card-body">
                                <div className="card shadow mb-4">
                                    <div className="card-body">
                                        <div className="table-responsive">
                                           <Table Columns={Columnas} Data={Compras} Title={"Compras"} />
                                        </div>
                                    </div>
                                </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    } else {
        getCompras();
    }
}