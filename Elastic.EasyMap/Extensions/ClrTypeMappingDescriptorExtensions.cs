using System;
using System.Linq.Expressions;
using Nest;

namespace Elastic.EasyMap.Extensions
{
    public static class ClrTypeMappingDescriptorExtensions
    {
        public static ClrTypeMappingDescriptor<T> IndexTypeNameInferrer<T>(this ClrTypeMappingDescriptor<T> clr,
            string additionalSuffix = null) where T : class
        {
            return clr.IndexName(additionalSuffix + typeof(T).Name.ToLower().Replace("entity", ""));
        }

        public static ClrTypeMappingDescriptor<T> Map<T>(this ClrTypeMappingDescriptor<T> clr, Expression<Func<T, object>> selector) where T : class
        {
            if (!(selector.Body is MemberExpression body))
                body = ((UnaryExpression)selector.Body).Operand as MemberExpression;

            if (body == null) return clr;

            var name = body.Member.Name;
            PropertyInfoFactory.AddIncludedProperty<T>(name);

            return clr;
        }
    }
}