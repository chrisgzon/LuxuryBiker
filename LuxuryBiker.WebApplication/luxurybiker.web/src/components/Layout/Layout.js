import React from 'react';
import Sidebar from './_sidebar';
import Nav from './_nav';
import Footer from './_footer';

export default function Layout() {
    return (
        <div id='wrapper'>
            <Sidebar />
            <div id="content-wrapper" className="d-flex flex-column">
                {/* Main Content */}
                <div id="content">
                    <Nav />
                    {/* Begin Page Content  */}
                    <div className="container-fluid">
                    </div> 
                </div>
                <Footer />
                {/* End of Main Content  */}
            </div> 
        </div>
    );
}