using System;
using System.Collections.Generic;
using System.Linq;
using Elastic.EasyMap.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Elastic.EasyMap
{
    public class MappedPropertyResolver : CamelCasePropertyNamesContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            var mapped = PropertyInfoFactory.GetMappedProperties(type);
            if (mapped == null || mapped.Any() == false) return properties;
            properties = properties.Where(p => mapped.Contains(p.PropertyName)).ToList();
            return properties;
        }
    }
}