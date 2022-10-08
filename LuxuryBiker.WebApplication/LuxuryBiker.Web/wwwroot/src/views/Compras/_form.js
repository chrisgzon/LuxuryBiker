import { data } from "jquery";
import React, { useState } from "react";
import Swal from 'sweetalert2';

export default function Form({productos, proveedores, registerCompra, showModalProvider, showModalProduct}) {
    const selectProvider = React.useRef();
    const selectProduct = React.useRef();
    const txtFechaCompra = React.useRef();

    const [detallesCompra, setDetallesCompra] = useState([]);
    const [total, setTotal] = useState(0);
    const [total_impuesto, setTotal_impuesto] = useState(0);
    const [total_pagar, setTotal_pagar] = useState(0);
    const [producto, setProducto] = useState({
        nombre: "",
        cantidad: "",
        ValorProducto: "",
        ProductoIdProducto: 0
    });

    const showFieldsProducts = { ProductoIdProducto: "Producto", ValorProducto: "Precio", cantidad: "Cantidad" };
    const impuesto = 19;
    var dataCompra = null;
    var subtotal = 0;
    var total_aux = 0;
    var total_impuesto_aux = 0;
    var total_pagar_aux = 0;

    const handleSubmitCompra = (e) => {
        e.preventDefault();

        dataCompra = {
            TerceroIdTercero: selectProvider.current.value,
            UsuarioIdUsuario: LuxuryBiker.Usuario.idUsuario,
            DetallesCompra: detallesCompra,
            FechaCompra: txtFechaCompra.current.value
        }

        registerCompra(dataCompra).then(result=>{
            if (result) {
                setDetallesCompra([])
                setTotal(0)
                setTotal_impuesto(0)
                setTotal_pagar(0)
            }
        });
    }

    const handleFieldProductoChange = (event) => {
        if (event.target.name === "ProductoIdProducto") {
             producto.nombre = selectProduct.current.options[selectProduct.current.selectedIndex].text
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
        let tCompras = detallesCompra;
        tProducto.totalCompraProducto = subtotal;
        tCompras.push(tProducto);
        setDetallesCompra(tCompras);
        setTotal(total_aux);
        setProducto({
            nombre:"",
            cantidad:"",
            ValorProducto:"",
            ProductoIdProducto:0 
        });
        totales();
    };
    
    const validateDataProductAdd = () => {
        var result = true;
        for (var property in producto) {
            if ((producto[property] === null || producto[property] === "" || producto[property] === 0) && property !== "nombre") {
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
        if (result && detallesCompra.find(x=>x.ProductoIdProducto === producto.ProductoIdProducto)) {
            Swal.fire(
                '¡Error!',
                'El producto <b>'+producto.nombre+'</b> ya se encuentra en la lista de compras, si desea modificar algun valor debe eliminarlo y volver a agregarlo',
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
    
    const eliminar = (index, compra) => {
        let tCompras = detallesCompra;
        total_aux = total - compra.totalCompraProducto;
        tCompras.splice(index, 1);
        setDetallesCompra(tCompras);
        setTotal(total_aux);
        totales();
    };
    
    function Fila ({index, compra}) {
        return (
            <tr className="selected" id={"fila"+index}>
                <td>
                    <button type="button" className="btn btn-danger btn-sm" onClick={eliminar.bind(this, index, compra)}>
                        <i className="fa fa-times"></i>
                    </button>
                </td>
                <td>
                    <input type="hidden" name="product_id[]" value={compra.ProductoIdProducto} />{compra.nombre}
                </td>
                <td>
                    <input type="hidden" id="price[]" name="price[]" value={formatoNumero(compra.ValorProducto, 0, "", ".")} />{formatoNumero(compra.ValorProducto, 0, "", ".")}
                </td>
                <td>
                    <input type="hidden" name="quantity[]" value={compra.cantidad} />{compra.cantidad}
                </td>
                <td align="right">${formatoNumero(compra.totalCompraProducto, 0, "", ".")}</td>
            </tr>
        );
    }

    return (
        <form className="mt-2" onSubmit={handleSubmitCompra}>
            <div name="box-gray">
                <div className="form-group">
                    <label htmlFor="FechaCompra">Fecha Compra</label>
                    <input ref={txtFechaCompra} type="date" id="FechaCompra" className="form-control" name="FechaCompra" />
                </div>
                <div className="form-group">
                    <label htmlFor="provider_id">Proveedor</label>
                    <button type="button" class="btn btn-success btn-sm btn-circle ml-2" onClick={()=>{showModalProvider(true)}}>
                        <i className="fas fa-solid fa-user-plus"></i>
                    </button>
                    <select ref={selectProvider} id="provider_id" className="form-control" name="provider_id">
                        <option value={""} hidden>Seleccionar...</option>
                        {
                            proveedores.map((proveedor, index) => {
                                return(<option key={index} value={proveedor.idTercero}>{proveedor.nombres + (proveedor.apellidos !== "" ? " " + proveedor.apellidos : "") + "("+proveedor.identificacion+")"}</option>);
                            })
                        }
                    </select>
                </div>
                <div className="form-group">
                    <label htmlFor="ProductoIdProducto">Producto</label>
                    <button type="button" class="btn btn-success btn-sm btn-circle ml-2" onClick={()=>{showModalProduct(true)}}>
                        <i class="fas fa-solid fa-square-plus"></i>
                    </button>
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
                    <label htmlFor="cantidad">Cantidad</label>
                    <input value={producto.cantidad} onChange={handleFieldProductoChange} id="cantidad" className="form-control" type="text" name="cantidad" />
                </div>
            </div>
            <div className="form-group mt-3 row">
                <h4 className="card-title col-md-6">Detalles de compra</h4>
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
                            {detallesCompra.map((compra, index)=>{
                                return (<Fila key={index} index={index} compra={compra} />);
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