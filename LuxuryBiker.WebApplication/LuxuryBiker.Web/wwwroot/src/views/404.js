import React from 'react';
import { Link } from 'react-router-dom';

export default function PageNotFound () {
    return (
        <div className="text-center">
            <div className="error mx-auto" data-text="404">404</div>
            <p className="lead text-gray-800 mb-5">Pagina No Encontrada</p>
            <p className="text-gray-500 mb-0">parece que estas buscando algo que no existe...</p>
            <Link to="/">&larr;Volver al inicio</Link>
        </div>
    );
}