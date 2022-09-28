import React from "react";
import Nav from './_Nav';
import Sidebar from './_Sidebar';
import Footer from './_Footer';

export default function Layout({children, usuario, logout}) {
    return (
        <div id='wrapper'>
            <Sidebar usuario={usuario} />
            <div id="content-wrapper" className="d-flex flex-column">
                {/* Main Content */}
                <div id="content">
                    <Nav usuario={usuario} logout={logout} />
                    {/* Begin Page Content  */}
                    <div className="container-fluid">
                        {children}
                    </div>
                </div>
                {/* End of Main Content  */}
                <Footer />
            </div>
        </div>
    );
}