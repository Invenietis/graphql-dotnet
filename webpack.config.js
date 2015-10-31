var output = './src/GraphQL.GraphiQL/public';

module.exports = {
    entry: {
        'bundle': './src/GraphQL.GraphiQL/app/app.jsx'
    },

    output: {
        path: output,
        filename: '[name].js'
    },

    resolve: {
        extensions: ['', '.js', '.json', '.jsx'],
    },

    module: {
        loaders: [
          { test: /\.js/, loader: 'babel', exclude: /node_modules/ },
          { test: /\.jsx$/, loader: 'jsx-loader?harmony' }
        ]
    },

};
