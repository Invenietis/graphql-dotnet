using GraphQL.Tests;
using GraphQL.Types;
using System.Threading.Tasks;
using System.Web.Http;

namespace GraphQL.GraphiQL.Controllers
{
    public class GraphQLController : ApiController
    {
        private Schema _schema;

        StarWarsData _data;
        public GraphQLController()
        {
            _schema = new StarWarsSchema();
            _data = new StarWarsData();
        }

        public async Task<ExecutionResult> Post(GraphQLQuery query)
        {
            return await Execute(_schema, _data, query.Query);
        }

        public async Task<ExecutionResult> Execute<T>( Schema schema, T rootObject, string query, string operationName = null, Inputs inputs = null)
        {
            var executer = new DocumentExecuter();
            return await executer.ExecuteAsync(schema, rootObject, query, operationName);
        }

        protected override void Dispose( bool disposing )
        {
            _data.Dispose();
            base.Dispose( disposing );
        }
    }

    public class GraphQLQuery
    {
        public string Query { get; set; }
        public string Variables { get; set; }
    }
}
 