var root = './';

module.exports = {
    entry: {
        'bundle': root + 'app/js/app.js'
    },

  	output: {filename: 'app.js', path: root + 'wwwroot/js/'},
  
    resolve: {
        extensions: ['', '.js', '.json', '.jsx'],
    },
	module: {
		loaders: [
			{
				exclude: /node_modules/,
				loader: 'babel-loader',
				query: {stage: 0, plugins: ['./app/build/babelRelayPlugin']},
				test: /\.jsx?$/,
			}
		]
	}
};
