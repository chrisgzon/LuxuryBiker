import React, { useState } from "react";
import Swal from 'sweetalert2';

export default function Form({productos, clientes, registerVenta, showModalClient}) {
    const selectCliente = React.useRef();
    const selectProduct = React.useRef();

    const [detallesVenta, setDetallesVenta] = useState([]);
    const [total, setTotal] = useState(0);
    const [total_impuesto, setTotal_impuesto] = useState(0);
    const [total_pagar, setTotal_pagar] = useState(0);
    const [producto, setProducto] = useState({
        nombre: "",
        cantidad: "",
        ValorProducto: "",
        ProductoIdProducto: 0,
        cantidadMaxima: 0
    });

    const showFieldsProducts = { ProductoIdProducto: "Producto", ValorProducto: "Precio", cantidad: "Cantidad" };
    const impuesto = 19;
    var dataVenta = null;
    var subtotal = 0;
    var total_aux = 0;
    var total_impuesto_aux = 0;
    var total_pagar_aux = 0;

    const handleSubmitVenta = (e) => {
        e.preventDefault();

        dataVenta = {
            TerceroIdTercero: selectCliente.current.value === "" ? null : selectCliente.current.value,
            UsuarioIdUsuario: LuxuryBiker.Usuario.idUsuario,
            DetallesVenta: detallesVenta
        }

        registerVenta(dataVenta).then(result=>{
            if (result) {
                setDetallesVenta([])
                setTotal(0)
                setTotal_impuesto(0)
                setTotal_pagar(0)
            }
        });
    }

    const handleFieldProductoChange = (event) => {
        if (event.target.name === "ProductoIdProducto") {
            let nombre = selectProduct.current.options[selectProduct.current.selectedIndex].text
            let idProducto = selectProduct.current.value;
            let precio = productos.find(p=>p.idProducto === parseInt(event.target.value))?.valorProducto;
            let cantidad = productos.find(p=>p.idProducto === parseInt(event.target.value))?.stock;
            setProducto({...producto, ValorProducto: precio, cantidadMaxima: cantidad, nombre: nombre, ProductoIdProducto: idProducto});
            return;
        }
        setProducto({
            ...producto,
			[event.target.name]: event.target.value,
        });
    }
    
    const HandleAgregar = () => {
        if (!validateDataProductAdd()) {
            return;
        }
        
        producto.ValorProducto = parseInt(producto.ValorProducto.toString().replace(".", "").replace(",", ""));
        producto.cantidad = parseFloat(producto.cantidad);
        subtotal = producto.ValorProducto * producto.cantidad;
        total_aux = total + subtotal; 
        let tProducto = producto; 
        let tVentas = detallesVenta;
        tProducto.totalVentaProducto = subtotal;
        tVentas.push(tProducto);
        setDetallesVenta(tVentas);
        setTotal(total_aux);
        setProducto({
            nombre:"",
            cantidad:"",
            ValorProducto:"",
            ProductoIdProducto:0,
            cantidadMaxima: 0
        });
        totales();
    };
    
    const validateDataProductAdd = () => {
        var result = true;
        var productoData = productos.find(x=>x.idProducto==producto.ProductoIdProducto);
        for (var property in producto) {
            if ((producto[property] === null || producto[property] === "" || producto[property] === 0) && property !== "nombre" && property !== "cantidadMaxima") {
                Swal.fire(
                    '¡Error!',
                    'El campo <b>' + showFieldsProducts[property] + '</b> no puede estar vacio',
                    'error'
                );
                result = false;
            }
        }
        if (result && (isNaN(producto.ValorProducto) || producto.ValorProducto < 1)) {
            Swal.fire(
                '¡Error!',
                'El valor digitado en el campo <b>Precio</b> es invalido',
                'error'
            );
            result = false;
        }
        if (result && (isNaN(producto.cantidad) || producto.cantidad < 1)) {
            Swal.fire(
                '¡Error!',
                'El valor digitado en el campo <b>Cantidad</b> es invalido',
                'error'
            );
            result = false;
        }
        if (result && producto.cantidad > productoData.stock) {
            Swal.fire(
                '¡Error!',
                'El valor digitado en el campo <b>Cantidad</b> es superior al stock actual del producto',
                'error'
            );
            result = false;
        }
        if (result && detallesVenta.find(x=>x.ProductoIdProducto === producto.ProductoIdProducto)) {
            Swal.fire(
                '¡Error!',
                'El producto <b>'+producto.nombre+'</b> ya se encuentra en la lista de ventas, si desea modificar algun valor debe eliminarlo y volver a agregarlo',
                'error'
            );
            result = false;
        }
        return result;
    };

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
    
    const totales = () => {
        total_impuesto_aux = total_aux * (impuesto / 100);
        total_pagar_aux = total_aux + total_impuesto_aux 
        setTotal_impuesto(total_impuesto_aux);
        setTotal_pagar(total_pagar_aux);
    };
    
    const eliminar = (index, venta) => {
        let tVentas = detallesVenta;
        total_aux = total - venta.totalVentaProducto;
        tVentas.splice(index, 1);
        setDetallesVenta(tVentas);
        setTotal(total_aux);
        totales();
    };
    
    function Fila ({index, venta}) {
        return (
            <tr className="selected" id={"fila"+index}>
                <td>
                    <button type="button" className="btn btn-danger btn-sm" onClick={eliminar.bind(this, index, venta)}>
                        <i className="fa fa-times"></i>
                    </button>
                </td>
                <td>
                    <input type="hidden" name="product_id[]" value={venta.ProductoIdProducto} />{venta.nombre}
                </td>
                <td>
                    <input type="hidden" id="price[]" name="price[]" value={formatoNumero(venta.ValorProducto, 0, "", ".")} />{formatoNumero(venta.ValorProducto, 0, "", ".")}
                </td>
                <td>
                    <input type="hidden" name="quantity[]" value={venta.cantidad} />{venta.cantidad}
                </td>
                <td align="right">${formatoNumero(venta.totalVentaProducto, 0, "", ".")}</td>
            </tr>
        );
    }

    return (
        <form className="mt-2" onSubmit={handleSubmitVenta}>
            <div name="box-gray">
                <div className="form-group">
                    <label htmlFor="client_id">Cliente</label>
                    <button type="button" className="btn btn-success btn-sm btn-circle ml-2" onClick={()=>{showModalClient(true)}}>
                        <i className="fas fa-solid fa-user-plus"></i>
                    </button>
                    <select ref={selectCliente} id="client_id" className="form-control" name="client_id">
                        <option value={""} hidden>Seleccionar...</option>
                        {
                            clientes.map((cliente, index) => {
                                return(<option key={index} value={cliente.idTercero}>{cliente.nombres + (cliente.apellidos !== "" ? " " + cliente.apellidos : "") + "("+cliente.identificacion+")"}</option>);
                            })
                        }
                    </select>
                </div>
                <div className="form-group">
                    <label htmlFor="ProductoIdProducto">Producto</label>
                    <select ref={selectProduct} value={producto.ProductoIdProducto} onChange={handleFieldProductoChange} id="ProductoIdProducto" className="form-control" name="ProductoIdProducto">
                    <option value={""} hidden>Seleccionar...</option>
                        {
                            productos.map((producto, index) => {
                                return(<option key={index} value={producto.idProducto}>{producto.nombre + "(" + producto.codigo+")"}</option>);
                            })
                        }
                    </select>
                </div>
                <div className="form-group">
                    <label htmlFor="ValorProducto">Precio</label>
                    <input value={producto.ValorProducto} onChange={handleFieldProductoChange} id="ValorProducto" className="form-control" type="text" name="ValorProducto" />
                </div>
                <div className="form-group">
                    <label htmlFor="tax">Impuesto (%)</label>
                    <input id="tax" onChange={handleFieldProductoChange} className="form-control" value="19" type="text" readOnly name="tax" />
                </div>
                <div className="form-group">
                    <label htmlFor="cantidad">Cantidad</label>{producto.cantidadMaxima != "" ? <i>Cant. maxima posible: {producto.cantidadMaxima}</i> : ""}
                    <input value={producto.cantidad} onChange={handleFieldProductoChange} id="cantidad" className="form-control" type="text" name="cantidad" />
                </div>
            </div>
            <div className="form-group mt-3 row">
                <h4 className="card-title col-md-6">Detalles de venta</h4>
                <div className="col-md-6" style={{ textAlign: "end" }}>
                    <button className="btn btn-success" onClick={HandleAgregar} type="button">Agregar Producto</button>
                </div>
            </div>
            <div className="form-group">
                <div className="table-responsive col-md-12">
                    <table id="detalles" className="table table-striped">
                        <thead>
                            <tr>
                                <th>Eliminar</th>
                                <th>Producto</th>
                                <th>Precio(COP)</th>
                                <th>Cantidad</th>
                                <th>SubTotal(COP)</th>
                            </tr>
                        </thead>
                        <tbody>
                            {detallesVenta.map((venta, index)=>{
                                return (<Fila key={index} index={index} venta={venta} />);
                            })}
                        </tbody>
                        <tfoot>
                            <tr>
                                <th colSpan={4}>
                                    <p align="right">VALOR:</p>
                                </th>
                                <th>
                                    <p align="right"><span id="total">COP {formatoNumero(total, 0, "", ".")}</span> </p>
                                </th>
                            </tr>
                            <tr>
                                <th colSpan={4}>
                                    <p align="right">IVA(19%):</p>
                                </th>
                                <th>
                                    <p align="right"><span id="total_impuesto">COP {formatoNumero(total_impuesto, 0, "", ".")}</span></p>
                                </th>
                            </tr>
                            <tr>
                                <th colSpan={4}>
                                    <p align="right">TOTAL PAGAR:</p>
                                </th>
                                <th>
                                    <p align="right"><span align="right" id="total_pagar_html">COP {formatoNumero(total_pagar, 0, "", ".")}</span> <input type="hidden"
                                        name="total" id="total_pagar" value={formatoNumero(total_pagar, 0, "", ".")} /></p>
                                </th>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
            {(total > 0) ? <button type="submit" className="btn btn-primary col-md-3" id="guardar">Registrar</button> : ""}
        </form>
    );
}