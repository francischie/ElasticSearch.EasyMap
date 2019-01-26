using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Elastic.EasyMap.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static Expression<Func<TModel, object>> GetGetAccessor<TModel>(this PropertyInfo propertyInfo, bool includeNonPublic = false)
        {
            var parameter = Expression.Parameter(typeof(TModel));
            var property = Expression.Property(parameter, propertyInfo);
            var conversion = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<TModel, object>>(conversion, parameter);
        }

    }
}