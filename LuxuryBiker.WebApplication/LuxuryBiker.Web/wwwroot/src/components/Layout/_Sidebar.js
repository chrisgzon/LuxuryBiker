import React, { Component, useState } from 'react';
import { Link } from 'react-router-dom';
import $ from 'jquery';

export default function Sidebar() {
    const usuario = LuxuryBiker.Usuario;
    // Toggle the side navigation
    const onClickSidebar = () => {
        $("body").toggleClass("sidebar-toggled");
        $(".sidebar").toggleClass("toggled");
    }

    // Close any open menu accordions when window is resized below 768px
    $(window).on('resize', function() {        
        // Toggle the side navigation when window is resized below 480px
        if ($(window).width() < 480 && !$(".sidebar").hasClass("toggled")) {
        $("body").addClass("sidebar-toggled");
        $(".sidebar").addClass("toggled");
        };
    });

    // Prevent the content wrapper from scrolling when the fixed side navigation hovered over
    $('body.fixed-nav .sidebar').on('mousewheel DOMMouseScroll wheel', function(e) {
        if ($(window).width() > 768) {
        var e0 = e.originalEvent,
            delta = e0.wheelDelta || -e0.detail;
        this.scrollTop += (delta < 0 ? 1 : -1) * 30;
        e.preventDefault();
        }
    });
  
    // Scroll to top button appear
    $(document).on('scroll', function() {
        var scrollDistance = $(this).scrollTop();
        if (scrollDistance > 100) {
        $('.scroll-to-top').fadeIn();
        } else {
        $('.scroll-to-top').fadeOut();
        }
    });
    
    // Smooth scrolling using jQuery easing
    $(document).on('click', 'a.scroll-to-top', function(e) {
        var $anchor = $(this);
        $('html, body').stop().animate({
        scrollTop: ($($anchor.attr('href')).offset().top)
        }, 1000, 'easeInOutExpo');
        e.preventDefault();
    });

    return (
        <ul className="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">
            {/*Sidebar - Brand*/}
            <Link className="sidebar-brand d-flex align-items-center justify-content-center" to="/">
                <div className="sidebar-brand-icon rotate-n-15">
                    <i className="fas fa-solid fa-motorcycle"></i>
                </div>
                <div className="sidebar-brand-text mx-3">Luxury<sup>Biker</sup></div>
            </Link>

            {/*Divider*/}
            <hr className="sidebar-divider my-0" />

            {/*Nav Item - Dashboard*/}
            <li className="nav-item active">
                <Link className="nav-link" to="/">
                    <i className="fas fa-fw fa-tachometer-alt"></i>
                    <span>Dashboard</span>
                </Link>
            </li>

            {/*Divider*/}
            <hr className="sidebar-divider" />

            {/*Heading*/}
            <div className="sidebar-heading">Usuarios</div>

            {/*Nav Item - Pages Collapse Menu*/}
            {(!!usuario && (usuario.roles.includes('Administrador') || usuario.isAdministrador)) ? (
            <li className="nav-item">
                <a className="nav-link collapsed" href="#" data-toggle="collapse" data-target="#collapseTerceros" aria-expanded="true" aria-controls="collapseTwo">
                <i className="fas fa-solid fa-user-tie"></i>
                    <span>Terceros</span>
                </a>
                <div id="collapseTerceros" className="collapse" aria-labelledby="headingTwo" data-parent="#accordionSidebar">
                    <div className="bg-white py-2 collapse-inner rounded">
                        <h6 className="collapse-header">Clientes/Distribuidores:</h6>
                        <Link className="collapse-item" to="/Terceros/Registrar">Registrar Tercero</Link>
                    </div>
                </div>
            </li>) : ""}

            {/*Divider*/}
            <hr className="sidebar-divider" />

            {/*Heading*/}
            <div className="sidebar-heading">Productos</div>

            {(!!usuario && (usuario.roles.includes('Administrador') || usuario.isAdministrador)) ? (
            <li className="nav-item">
                <a className="nav-link collapsed" href="#" data-toggle="collapse" data-target="#collapseProductos" aria-expanded="true" aria-controls="collapsePages">
                <i className="fas fa-solid fa-boxes-stacked"></i>
                    <span>Productos</span>
                </a>
                <div id="collapseProductos" className="collapse" aria-labelledby="headingPages" data-parent="#accordionSidebar">
                    <div className="bg-white py-2 collapse-inner rounded">
                        <Link className="collapse-item" to="/Productos/Registrar">Registrar Producto</Link>
                    </div>
                </div>
            </li>) : ""}

            {(!!usuario && (usuario.roles.includes('Administrador') || usuario.isAdministrador)) ? (
            <li className="nav-item">
                <a className="nav-link collapsed" href="#" data-toggle="collapse" data-target="#collapseCompras" aria-expanded="true" aria-controls="collapsePages">
                <i className="fas fa-solid fa-cart-arrow-down"></i>
                    <span>Compras</span>
                </a>
                <div id="collapseCompras" className="collapse" aria-labelledby="headingPages" data-parent="#accordionSidebar">
                    <div className="bg-white py-2 collapse-inner rounded">
                        <Link className="collapse-item" to="/Compras/Show">Ver Compras</Link>
                        <Link className="collapse-item" to="/Compras/Registrar">Registrar Compra</Link>
                    </div>
                </div>
            </li>) : ""}

            {(!!usuario && (usuario.roles.includes('Administrador') || usuario.isAdministrador)) ? (
            <li className="nav-item">
                <a className="nav-link collapsed" href="#" data-toggle="collapse" data-target="#collapseVentas" aria-expanded="true" aria-controls="collapsePages">
                <i className="fas fa-solid fa-tags"></i>
                    <span>Ventas</span>
                </a>
                <div id="collapseVentas" className="collapse" aria-labelledby="headingPages" data-parent="#accordionSidebar">
                    <div className="bg-white py-2 collapse-inner rounded">
                        <Link className="collapse-item" to="/Ventas/Show">Ver Ventas</Link>
                        <Link className="collapse-item" to="/Ventas/Registrar">Registrar Venta</Link>
                    </div>
                </div>
            </li>) : ""}

            {/*Divider*/}
            <hr className="sidebar-divider d-none d-md-block" />

            {/*Sidebar Toggler (Sidebar)*/}
            <div className="text-center d-none d-md-inline">
                <button className="rounded-circle border-0" id="sidebarToggle" onClick={onClickSidebar}></button>
            </div>    
        </ul>
    )
}