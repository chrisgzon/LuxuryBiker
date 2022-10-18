import React, { useEffect, useRef, useState } from "react";
import Table from "../components/Table";
import Axios from 'axios';
import './../vendor/chart.js/Chart';

export default function Home() {    
    const [loading, setLoading]  = useState(true);
    const [Data, SetData] = useState(null);
    const targetComprasVentas = useRef(null);
    const targetVentasDiarias = useRef(null);

    useEffect(() => {
        setLoading(true);
        getData();        
    }, []);

    const getData = async () => {

        try {
            await Axios.get('/Compras/GetData').then((response) => {
                if (response.data.error) {
                    throw response;
                }
                setLoading(false);
                
                SetData(response.data.result)
            });
        } catch (error) {
            setLoading(false)
            SetData([])
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

    const columnsProductosMasVendidos = [
        {
            name:"Código",
            selector: row => row.codigo,
            sortable: true
        },
        {
            name:"Producto",
            selector: row => {
                return row.nombre
            },
            sortable: true,
            grow: 2
        },
        {
            name:"Stock",
            selector: row => row.stock,
            sortable: true
        },
        {
            name:"Cantidad vendida",
            selector:row=>row.cantidad,
            sortable: true,
        }
    ]

    const showGraphics = () => {    
            let labels = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
            let data = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            let dataVentas = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            data.map((valor, index) => {
                Data.comprasMes.forEach(compra=>
                {
                    if (index === compra.indexMes-1) {
                        data.splice(index, 1, compra.total);   
                    }
                })
            }) 
            dataVentas.map((valor, index) => {
                Data.ventasMes.forEach(venta=>
                {
                    if (index === venta.indexMes-1) {
                        dataVentas.splice(index, 1, venta.total);   
                    }
                })
            })
            new Chart(targetComprasVentas.current, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Compras',
                        data: data,
                        backgroundColor: '#0B94F7',
                        borderWidth:3
                    },
                    {
                        label: 'Ventas',
                        data: dataVentas,
                        backgroundColor: '#46c35f',
                        borderWidth: 3
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero:true,
                                callback: function(value, index, values) {
                                    return '$' + formatoNumero(value, 0, "", ".");
                                }
                            }
                        }]
                    },
                    tooltips: {
                        callbacks: {
                            label: function(tooltipItem, chart) {
                              var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                              return datasetLabel + ': $' + formatoNumero(tooltipItem.yLabel, 0, '', '.');
                            }
                        }
                    }
                }
            });

            let diaActual = new Date();
            let diaIni = new Date(new Date().setDate(diaActual.getDate() + (-15)));
            let labelsDia = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            let dataVentasDia = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

            if (Data.ventasDia.length === 15) {
                data.map((valor, index) => {
                    Data.ventasDia.forEach(venta=>
                    { 
                        labelsDia.splice(index, 1, "Dia: "+venta.dia);
                        dataVentasDia.splice(index, 1, venta.total);
                    })
                })
            }

            labelsDia.map((valor, index) => { 
                labelsDia.splice(index, 1, "Dia: "+(diaIni.getDate()+index+1));
            })

            dataVentasDia.map((valor, index) => {
                Data.ventasDia.forEach(venta=>
                { 
                    if (diaIni.getDate()+index+1 === parseInt(venta.dia)) {
                        dataVentasDia.splice(index, 1, venta.total);
                    }
                })
            })

            new Chart(targetVentasDiarias.current, {
                type: 'line',
                data: {
                    labels: labelsDia,
                    datasets: [{
                        label: 'Ventas',
                        data: dataVentasDia,
                        borderColor: '#46c35f',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero:true,
                                callback: function(value, index, values) {
                                    return '$' + formatoNumero(value, 0, "", ".");
                                }
                            }
                        }]
                    },
                    tooltips: {
                        callbacks: {
                            label: function(tooltipItem, chart) {
                              var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                              return datasetLabel + ': $' + formatoNumero(tooltipItem.yLabel, 0, '', '.');
                            }
                        }
                    }
                }
            });
    }

    if (!loading) {
        showGraphics()
    }

    return (
        <main>
            <div className="content-wrapper">
                <div className="page-header">
                    <h3 className="page-title">
                        Panel administrador
                    </h3>
                </div>
                <div className="row">
                    <div className="col-lg-12 col-xs-12 mb-2">
                        <div className="card border-left-success shadow h-100 py-2">
                            <div className="card-body">
                                <div className="row no-gutters align-items-center">
                                    <div className="col mr-2">
                                        <div className="text-m font-weight-bold text-success text-uppercase mb-1">
                                            Ventas (Hoy)</div>
                                        <div className="h5 mb-0 font-weight-bold text-gray-800">$ {Data && formatoNumero(Data.ventasHoy.total, 0, "", ".")} </div>
                                    </div>
                                    <div className="col-auto">
                                        <i className="fas fa-dollar-sign fa-4x text-gray-300"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-12 grid-margin stretch-card">
                        <div className="card shadow">
                            <div className="card-body">
                                <div className="row">
                                    <div className="col-lg-6 col-s-6">
                                        <div className="card border-bottom-primary shadow h-100 py-2">
                                            <div className="card-body">
                                                <div className="row no-gutters align-items-center">
                                                    <div className="col mr-2">
                                                        <div className="text-s font-weight-bold text-primary text-uppercase mb-1">
                                                            Compras ({Data && Data.totalCompraMes.mes})</div>
                                                        <div className="h5 mb-0 font-weight-bold text-gray-800">$ {Data && formatoNumero(Data.totalCompraMes.total, 0, "", ".")} </div>
                                                    </div>
                                                    <div className="col-auto">
                                                        <i className="fas fa-cart-arrow-down fa-4x text-gray-300"></i>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="col-lg-6 col-xs-6">
                                        <div className="card border-bottom-success shadow h-100 py-2">
                                            <div className="card-body">
                                                <div className="row no-gutters align-items-center">
                                                    <div className="col mr-2">
                                                        <div className="text-s font-weight-bold text-success text-uppercase mb-1">
                                                            Ventas ({Data && Data.totalVentaMes.mes})</div>
                                                        <div className="h5 mb-0 font-weight-bold text-gray-800">$ {Data && formatoNumero(Data.totalVentaMes.total, 0, "", ".")}</div>
                                                    </div>
                                                    <div className="col-auto">
                                                        <i className="fas fa-shopping-cart fa-4x text-gray-300"></i>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-12 grid-margin stretch-card">
                        <div className="card shadow">
                            <div className="card-body">                    
                                <div className="col-md-12">
                                    <div className="card shadow card-chart">
                                        <div className="card-header">
                                            <h4 className="text-center">Compras y Ventas(Mensual)</h4>
                                        </div>
                                        <div className="card-content">
                                            <div className="ct-chart">
                                                <canvas ref={targetComprasVentas} id="compras"></canvas>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-12 grid-margin stretch-card">
                        <div className="card shadow">
                            <div className="card-body">                    
                                <div className="row">
                                    <div className="col-md-12">
                                        <div className="card shadow card-chart">
                                            <div className="card-header">
                                                <h4 className="text-center">Ventas(ultimos 15 dias)</h4>
                                            </div>
                                            <div className="card-content">
                                                <div className="ct-chart">
                                                    <canvas ref={targetVentasDiarias} id="ventas_diarias" height="80"></canvas>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-12 grid-margin stretch-card">
                        <div className="card shadow">
                            <div className="card-body">
                                <div className="table-responsive">
                                    <Table Data={Data != null ? Data.productosMasVendidos : []} Columns={columnsProductosMasVendidos} Title={"Productos Mas Vendidos"} />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>  
        </main>
    );
}