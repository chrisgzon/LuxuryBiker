import React from "react";
import Nav from './_Nav';
import Sidebar from './_Sidebar';
import Footer from './_Footer';

export default function Layout({children, logout}) {
    return (
        <div id='wrapper'>
            <Sidebar />
            <div id="content-wrapper" className="d-flex flex-column">
                {/* Main Content */}
                <div id="content">
                    <Nav logout={logout} />
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