using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Registration;
using GraphQL.Types;
using Should;

namespace GraphQL.Tests.Registration
{
    public class RegistrationDiscovererTests
    {

        public class SimpleModel
        {
            public string Text { get; set; }
        }

        public class SimpleQuery
        {
            public List<SimpleModel> Models { get; set; }
        }

        [Test]
        public void type_registration_with_simple_dto()
        {
            var schema = new Schema();
            var d = new GraphTypeDiscoverer( new ConventionGraphTypeHandler() );
            d.Register( typeof( SimpleQuery ) );
            d.Register( typeof( SimpleModel ) );
            d.Build( schema, new SchemaOptions { QueryRootTypeName = "SimpleQuery" } );

            schema.Query.ShouldNotBeNull();
            schema.Query
                .Fields
                .FirstOrDefault()
                .ShouldNotBeNull()
                .Type
                .ShouldEqual(
                    typeof( ListGraphType<> ).MakeGenericType( schema.FindType( "SimpleModel" ).GetType() ) );
        }

        [Test]
        public void discoverer_should_register_graph_objects_to_registration_class()
        {
        }

        [Test]
        public void types_should_not_inherits_from_parents_types_attributes()
        {
        }

        [Test]
        public void discoverer_should_build_schema_with_the_registered_type()
        {
            var schema = new Schema();
            var discoverer = new GraphTypeDiscoverer();

            discoverer.Register( typeof( QueryRoot ) );
            discoverer.Register( typeof( AnnotableModel ) );
            discoverer.Build( schema, new SchemaOptions()
            {
                QueryRootTypeName = "Query",
                MutationRootTypeName = "Mutation"
            } );

            var graphType = schema.FindType( "AnnotableType" ).ShouldNotBeNull().ShouldBeType<ObjectGraphType>();
            graphType.Name.ShouldEqual( "AnnotableType" );
            graphType.Description.ShouldEqual( "It is a sample annoted model" );
            graphType.Fields.ShouldNotBeEmpty();

            schema.Query.ShouldNotBeNull().ShouldBeType<ObjectGraphType>().Fields.Count().ShouldEqual( 2 );

            var firstField = schema.Query.ShouldNotBeNull().ShouldBeType<ObjectGraphType>().Fields.First();
            firstField.Name.ShouldEqual( "annotable" );
            firstField.Arguments.Count.ShouldEqual( 1 );

            var arg = firstField.Arguments.First();
            arg.Name.ShouldEqual( "id" );
            arg.Type.ShouldEqual( typeof( NonNullGraphType<IntGraphType> ) );
        }


        [ObjectGraphType( Name = "Query" )]
        public class QueryRoot
        {
            [Name( "annotables" )]
            public IList<AnnotableModel> AnnotableModels { get; set; }

            [Name( "annotable" )]
            public AnnotableModel GetAnnotableModel( int id )
            {
                return null;
            }
        }

        [InterfaceGraphType]
        public interface IGraphItemInterface
        {
            string Id { get; set; }
            string Text { get; set; }
        }

        [ObjectGraphType( Name = "AnnotableType", Description = "It is a sample annoted model" )] // This model is a GrapType.
        //[GraphType( typeof( ObjectGraphType ), Name = "AnnotableType" )] // Equivalent as above
        public class AnnotableModel : IGraphItemInterface
        {
            [NonNull] // By Convention, string is nullable. When this atribute is used, made the field nullable
            [Name( "id" )] // By Convention, property will be pascalCaseName of the PropertyName.
            [Description( "The id of the model" )] // Description is optionnal. Adds a description.
            [Deprecated( "This is why it is deprecated..." )] // Deprecation is optionnal; By default, it is not deprecated
            //[Resolver( typeof( IResolver ) )] // The default resolver will be used if not specified
            [GraphType( typeof( StringGraphType ) )] // By default, a property with Name ID will be of GraphType IdGraphType. ??
            public string Id { get; set; }

            /// <summary>
            /// With no annotation, it will be registered as: Field&lt;StringGraphType&gt;( "text" );
            /// </summary>
            public string Text { get; set; }

            [Ignore] // This field is not a GraphType. By default, all public properties are considered a GraphType.
            public int PrivateThing { get; set; }
        }

        /// <summary>
        /// Minimalist type that is registerd by convention
        /// By convention, a type whom name ends with GraphType will be considered as a GraphType.
        /// </summary>
        public class AnnotableGraphType
        {
            public string Id { get; set; }
            public string Text { get; set; }
        }

        /// <summary>
        /// With or without the attribute, this is equivalent.
        /// </summary>
        [GraphType( typeof( InputObjectGraphType ) )]
        [InputObjectGraphType]
        public class SomeCommandInput
        {
            public string SomeProperty { get; set; }
        }

        //[GraphType( typeof( OutputGraphType ) )]
        //[OutputGraphType( typeof( SomeCommandInput ) )]
        public class SomeCommandPayload
        {
        }

        /// <summary>
        /// SuperCommand does NOT inherits attributes from SomeCommandPayload.
        /// Each type will be registered as-is in the schema.
        /// </summary>
        [GraphType( typeof( InputObjectGraphType ) )]
        [InputObjectGraphType]
        public class SuperCommand : SomeCommandInput
        {
        }

    }
}
