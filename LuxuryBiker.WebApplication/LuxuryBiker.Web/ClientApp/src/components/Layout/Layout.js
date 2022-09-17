import React, {Component} from 'react';
import { Container } from 'reactstrap';
import { Sidebar } from './_sidebar';
import { Nav } from './_nav';

export class Layout extends Component {
    static displayName = Layout.name;
  
    render () {
      return (
        <div id='wrapper'>
            <Sidebar />
            <div id="content-wrapper" class="d-flex flex-column">
                {/* Main Content */}
                <div id="content">
                    <Nav />
                    {/* Begin Page Content  */}
                    <div class="container-fluid">
                        <Container>
                            {this.props.children}
                        </Container>
                    </div> 
                </div>
                {/* End of Main Content  */}
            </div> 
        </div>
      );
    }
  }
