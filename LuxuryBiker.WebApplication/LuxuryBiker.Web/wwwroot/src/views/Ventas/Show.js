import React, { useState } from "react";
import Axios from 'axios';
import { Link } from "react-router-dom";
import Swal from 'sweetalert2';
import Table from "../../components/Table";

export default function Show() {
    const [Ventas, setVentas] = useState(null);

    const getVentas = async function() {
        Swal.fire({
            title: 'Cargando...',
            allowOutsideClick: false,
            allowEscapeKey: false,
            allowEnterKey: false,
            showConfirmButton: false
        })
        try {
            await Axios.post("/Ventas/GetVentas", {}).then(response => {
                if (response.data.error) {
                    throw response;
                }
                
                setVentas(response.data.result);
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

    const formatoNumero = (numero, decimales, separadorDecimal, separadorMiles) => {
        var partes, array;
    
        if (!isFinite(numero) || isNaN(numero = parseFloat(numero))) {
            return "";
        }
        if (typeof separadorDecimal === "undefined") {
            separadorDecimal = ",";
        }
        if (typeof separadorMiles === "undefined") {
            separadorMiles = "";
        }
    
        // Redondeamos
        if (!isNaN(parseInt(decimales))) {
            if (decimales >= 0) {
                numero = numero.toFixed(decimales);
            } else {
                numero = (
                    Math.round(numero / Math.pow(10, Math.abs(decimales))) * Math.pow(10, Math.abs(decimales))
                ).toFixed();
            }
        } else {
            numero = numero.toString();
        }
    
        // Damos formato
        partes = numero.split(".", 2);
        array = partes[0].split("");
        for (var i = array.length - 3; i > 0 && array[i - 1] !== "-"; i -= 3) {
            array.splice(i, 0, separadorMiles);
        }
        numero = array.join("");
    
        if (partes.length > 1) {
            numero += separadorDecimal + partes[1];
        }
    
        return numero;
    };

    const handleOnClickStatusVenta = async (venta, e) => {

        let mensaje = venta.estado 
        ? "Si cancela la venta se aumentara la cantidad de cada producto vendido al stock actual." 
        : "Si valida la venta se descontara la cantidad de cada producto vendido del stock actual."
        Swal.fire({
            title: '¿Esta seguro de actualizar el estado?',
            text: mensaje,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, actualizar!',
            cancelButtonText: 'Cancelar'
          }).then(async (result) => {
            if (result.isConfirmed) {

                await changeStatusVenta(venta);
            }
          })
    }

    const changeStatusVenta = async function(venta) {

        try {
            await Axios.post('/Ventas/ChangeStatus', venta).then((response) => {
                if (response.data.error) {
                    throw response;
                }

                venta.estado = !venta.estado;
                setVentas([...Ventas])
                Swal.fire(
                    '¡Excelente!',
                    "Se actualizo el estado correctamente.",
                    'success'
                )
            });
        } catch(error) {
            console.log(error);
            Swal.fire(
                '¡Error!',
                error.data.mensaje,
                'error'
            )
        }
    }

    const Columnas = [
        {
            name:"Código",
            selector: row => row.codVenta,
            sortable: true,
            grow:0.5
        },
        {
            name:"Cliente",
            selector: row => {
                if (row.tercero !== null) {
                    return row.tercero.nombres + (row.tercero.apellidos !== "" ? " " + row.tercero.apellidos : "")
                } else {
                    return "No registra"
                }
            },
            sortable: true,
            grow: 2
        },
        {
            name:"Cant. productos",
            selector: row => row.cantidadProductos,
            sortable: true,
            grow:0.5
        },
        {
            name:"Fecha Venta",
            selector:row=>
            {
                var opciones = { year: 'numeric', month: 'short', day: 'numeric' };
                return new Date(row.fechaVenta).toLocaleDateString('es',opciones)
                .replace(/ /g,'-')
                .replace('.','')
                .replace(/-([a-z])/, function (x) {return '-' + x[1].toUpperCase()});
            },
            sortable: true,
        },
        {
            name:"Valor Total",
            selector:row=> "$ " + formatoNumero(row.total, 0, "", "."),
            sortable: true,
        },
        {
            name:"Estado",
            selector: function(row){
                if (row.estado) {
                    return (
                    <a href="#" title="Cancelar Venta" onClick={handleOnClickStatusVenta.bind(this, row)} className="btn btn-sm btn-success btn-icon-split">
                        <span className="icon text-white-50">
                            <i className="fas fa-check"></i>
                        </span>
                        <span className="text">Valida</span>
                    </a>
                    )
                } else {
                    return (
                        <a href="#" title="Validar Venta" onClick={handleOnClickStatusVenta.bind(this, row)} className="btn btn-sm btn-danger btn-icon-split">
                            <span className="icon text-white-50">
                                <i className="fas fa-times"></i>
                            </span>
                            <span className="text">Cancelada</span>
                        </a>
                    )
                }
            }
        }
    ]

    if (Ventas !== null) {

        return (
            <div className="content-wrapper">
                <div className="page-header row">
                    <h1 className="col-md-6 h3 mb-4 text-gray-800 col-md-6">Ventas</h1>
                    <nav aria-label="breadcrumb" className="col-md-6">
                        <ol className="breadcrumb">
                            <li className="breadcrumb-item"><Link to="/">Dashboard</Link></li>
                            <li className="breadcrumb-item active" aria-current="page">Ventas</li>
                        </ol>
                    </nav>
                </div>
                <div className="row">
                    <div className="col-lg-12 grid-margin stretch-card">
                            <div className="card-body">
                                <div className="card shadow mb-4">
                                    <div className="card-body">
                                        <div className="table-responsive">
                                           <Table Columns={Columnas} Data={Ventas} Title={"Ventas"} />
                                        </div>
                                    </div>
                                </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    } else {
        getVentas();
    }
}