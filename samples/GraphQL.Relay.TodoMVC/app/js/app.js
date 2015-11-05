import 'babel/polyfill';
import React from 'react';
import ReactDOM from 'react-dom';
import Relay from 'react-relay';
import TodoApp from './components/TodoApp';
import TodoAppHomeRoute from './routes/TodoAppHomeRoute';

ReactDOM.render(
  <Relay.RootContainer Component={TodoApp} route={new TodoAppHomeRoute()} />,
  document.getElementById('root')
);
