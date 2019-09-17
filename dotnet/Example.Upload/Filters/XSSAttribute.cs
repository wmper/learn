using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text.RegularExpressions;

namespace Example.Upload.Filters
{
    public class XSSAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var paramters = context.ActionDescriptor.Parameters;
            foreach (var paramter in paramters)
            {
                if (context.ActionArguments[paramter.Name] == null) continue;

                if (paramter.ParameterType == typeof(string))
                {
                    var value = context.ActionArguments[paramter.Name].ToString();

                    // Filter XSS
                    value = Regex.Replace(value, @"<[^>]*>", string.Empty);

                    context.ActionArguments[paramter.Name] = value;
                }
                else if (paramter.ParameterType.IsClass)
                {
                    PostModelProcess(paramter.ParameterType, context.ActionArguments[paramter.Name]);
                }
            }
        }

        private static object PostModelProcess(Type type, object obj)
        {
            if (obj == null) return obj;

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.GetValue(obj) == null) continue;

                if (property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(obj).ToString();

                    // Filter XSS
                    value = Regex.Replace(value, @"<[^>]*>", string.Empty);

                    property.SetValue(obj, value);
                }
                else if (property.PropertyType.IsClass)
                {
                    property.SetValue(obj, PostModelProcess(property.PropertyType, property.GetValue(obj)));
                }
            }

            return obj;
        }
    }
}
