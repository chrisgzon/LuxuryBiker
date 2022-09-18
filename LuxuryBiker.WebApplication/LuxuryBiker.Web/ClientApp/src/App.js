import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout/Layout';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout />
    );
  }
}