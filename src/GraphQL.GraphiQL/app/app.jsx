/// <Reference path="app.js" />

var React = require('react')
var GraphiQL = require('graphiql')
var fetch = require('isomorphic-fetch')

function graphQLFetcher(graphQLParams) {
  return fetch(window.location.origin + '/api/graphql', {
    method: 'post',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(graphQLParams),
  }).then(response => response.json());
}

class App extends React.Component {
    render() {
        return (
          <GraphiQL fetcher={graphQLFetcher} />
    );
    }
}

React.render(<GraphiQL fetcher={graphQLFetcher } />, document.body);
