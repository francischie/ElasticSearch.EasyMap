using System.Reflection;
using Nest;

namespace Elastic.EasyMap
{
    public class MappedPropertyVisitor<T> : NoopPropertyVisitor
    {
        public override bool SkipProperty(PropertyInfo propertyInfo, ElasticsearchPropertyAttributeBase attribute)
        {
            var list = PropertyInfoFactory.GetMappedProperties<T>();
            var name = char.ToLowerInvariant(propertyInfo.Name[0]) + propertyInfo.Name.Substring(1);
            return list.Contains(name) == false;
        }
    }
}