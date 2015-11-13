using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Registration;
using GraphQL.Types;
using Should;
using Fixie;

namespace GraphQL.Tests.Registration
{

    public class PropertyRegistrationByConventionTests
    {
        [Test]
        public void SimpleComplexType_PropertyName()
        {
            // Arrange
            var property = GetProperty<SimpleComplexTypeDto>( "Text" );

            // Act
            ConventionGraphTypeHandler handler = new ConventionGraphTypeHandler();
            var result = handler.BindName( property );

            // Assert
            result.ShouldNotBeNull().ShouldEqual( "text", StringComparer.InvariantCulture );
        }

        private void Expected<T>( string propertyName )
        {
            var property = GetProperty<SimpleComplexTypeDto>( propertyName );
            var ctx = new GraphItemResolutionContext();

            // Act
            ConventionGraphTypeHandler handler = new ConventionGraphTypeHandler();
            ctx.IsNullable = handler.IsNullable( property );
            ctx.GraphType = handler.BindGraphType( property );

            // Assert
            ctx.BuildGraphType().ShouldNotBeNull().ShouldEqual( typeof( T ) );
        }

        [Test]
        public void SimpleComplexType_GraphType_String()
        {
            Expected<StringGraphType>( "Text" );
            Expected<NonNullGraphType<IntGraphType>>( "Number" );
            Expected<IntGraphType>( "Number2" );
            Expected<NonNullGraphType<FloatGraphType>>( "FloatingNumber" );
            Expected<FloatGraphType>( "FloatingNumber2" );
            Expected<NonNullGraphType<BooleanGraphType>>( "Flag" );
            Expected<BooleanGraphType>( "Flag2" );
            //Expected<ListGraphType<NonNullGraphType<IntGraphType>>>( "SimpleList" );
            Expected<ListGraphType<FloatGraphType>>( "SimpleList2" );
        }

        class SimpleComplexTypeDto
        {
            public string Text { get; set; }

            public int Number { get; set; }
            public int? Number2 { get; set; }

            public float FloatingNumber { get; set; }
            public float? FloatingNumber2 { get; set; }

            public bool Flag { get; set; }
            public bool? Flag2 { get; set; }

            public List<int> SimpleList { get; set; }
            public List<float?> SimpleList2 { get; set; }
        }

        ItemMetadata GetProperty<T>( string propertyName )
        {
            PropertyInfo propertyInfo = typeof( T ).GetProperty(propertyName);
            ItemMetadata property = new ItemMetadata
            {
                ItemType = propertyInfo.PropertyType,
                Name = propertyInfo.Name,
                Attributes = propertyInfo.GetCustomAttributes().OfType<IGraphAttribute>().ToArray()
            };
            return property;
        }
    }
}
