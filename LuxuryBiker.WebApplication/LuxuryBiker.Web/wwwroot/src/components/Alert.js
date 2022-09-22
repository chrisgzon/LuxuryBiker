import React from 'react';
import Swal from 'sweetalert2'

const Alert = ({ mensaje, typeAlert, ocultarAlert }) => {

  if (!mensaje) {
    return null;
  }

  Swal.fire({
    icon: 'error',
    title: 'Oops...',
    text: mensaje
  }).then((result)=>{
    ocultarAlert();
  });
}

export default Alert;