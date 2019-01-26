using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elastic.EasyMap
{
    public class PropertyInfoFactory
    {
        private static readonly Lazy<PropertyInfoFactory> Lazy = new Lazy<PropertyInfoFactory>(() => new PropertyInfoFactory());
        private static PropertyInfoFactory Instance => Lazy.Value;

        private Dictionary<string, List<PropertyInfo>> TypeProperties { get; }
        private Dictionary<string, HashSet<string>> IncludedProperties { get; }

        public PropertyInfoFactory()
        {
            TypeProperties = new Dictionary<string, List<PropertyInfo>>();
            IncludedProperties = new Dictionary<string, HashSet<string>>();
        }


        public static List<PropertyInfo> GetProperties<T>() where T : class
        {
            var name = typeof(T).FullName;
            if (Instance.TypeProperties.TryGetValue(name, out var properties)) return properties;

            properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            Instance.TypeProperties.Add(name, properties);

            return properties;
        }

        public static void AddIncludedProperty<T>(string propertyName)
        {
            var name = typeof(T).FullName;

            Instance.IncludedProperties.TryGetValue(name, out var properties);

            if (properties == null)
            {
                properties = new HashSet<string>();
                Instance.IncludedProperties.Add(name, properties);
            }
            properties.Add(char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1));
        }

        public static HashSet<string> GetMappedProperties<T>()
        {
            return GetMappedProperties(typeof(T));
        }

        public static HashSet<string> GetMappedProperties(Type type)
        {
            var name = type.FullName;
            Instance.IncludedProperties.TryGetValue(name, out var properties);
            return properties;
        }


    }
}