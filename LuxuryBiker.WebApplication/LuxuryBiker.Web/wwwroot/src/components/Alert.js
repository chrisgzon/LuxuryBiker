import React from 'react';
import Swal from 'sweetalert2'

const Alert = ({ mensaje, typeAlert }) => {

  if (!mensaje) {
    return null;
  }

  Swal.fire({
    icon: 'error',
    title: 'Oops...',
    text: mensaje
  })
}

export default Alert;