import React, { Component } from 'react';
import $ from 'jquery';

export default function Nav({logout}) {
    const usuario = LuxuryBiker.Usuario;
    
    // Toggle the side navigation
    const onClickSidebar = () => {
        $("body").toggleClass("sidebar-toggled");
        $(".sidebar").toggleClass("toggled");
    }
    
    return(
        /* Topbar */
        <nav className="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">
        {/* Sidebar Toggle (Topbar) */}
        <button onClick={onClickSidebar} id="sidebarToggleTop" className="btn btn-link d-md-none rounded-circle mr-3">
            <i className="fa fa-bars"></i>
        </button>

        {/*Topbar Search*/}
        {/* <form className="d-none d-sm-inline-block form-inline mr-auto ml-md-3 my-2 my-md-0 mw-100 navbar-search">
            <div className="input-group">
                <input type="text" className="form-control bg-light border-0 small" placeholder="Realizar busqueda..." aria-label="Search" aria-describedby="basic-addon2" />
                <div className="input-group-append">
                    <button className="btn btn-primary" type="button">
                        <i className="fas fa-search fa-sm"></i>
                    </button>
                </div>
            </div>
        </form> */}

        {/* Topbar Navbar  */}
        <ul className="navbar-nav ml-auto">

            {/* Nav Item - Search Dropdown (Visible Only XS)  */}
            <li className="nav-item dropdown no-arrow d-sm-none">
                <a className="nav-link dropdown-toggle" href="#" id="searchDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i className="fas fa-search fa-fw"></i>
                </a>
                {/* Dropdown - Messages  */}
                <div className="dropdown-menu dropdown-menu-right p-3 shadow animated--grow-in" aria-labelledby="searchDropdown">
                    <form className="form-inline mr-auto w-100 navbar-search">
                        <div className="input-group">
                            <input type="text" className="form-control bg-light border-0 small" placeholder="Search for..." aria-label="Search" aria-describedby="basic-addon2" />
                            <div className="input-group-append">
                                <button className="btn btn-primary" type="button">
                                    <i className="fas fa-search fa-sm"></i>
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
            </li>

            <div className="topbar-divider d-none d-sm-block"></div>

            {/* Nav Item - User Information  */}
            <li className="nav-item dropdown no-arrow">
                <a className="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <span className="mr-2 d-none d-lg-inline text-gray-600 small">{usuario.nombres} {usuario.apellidos}</span>
                    <img className="img-profile rounded-circle" src={window.location.origin + '/Images/undraw_profile.svg'} />
                </a>
                {/* Dropdown - User Information  */}
                <div className="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="userDropdown">
                    <div className="dropdown-divider"></div>
                    <a href='#' className="dropdown-item" onClick={logout} data-toggle="modal" data-target="#logoutModal">
                        <i className="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i>
                        Cerrar sesión
                    </a>
                </div>
            </li>
        </ul>
    </nav>
    /* End of Topbar  */
    );
}