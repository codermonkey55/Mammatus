using System;
using System.ComponentModel;
using System.Reflection;

namespace Mammatus.Library.Reflection
{
    /// <summary>
    /// Extension methods for inspecting and working with method parameters.
    /// </summary>
    public static class ParameterInfoExtensions
    {
        /// <summary>
        /// Determines whether null can be assigned to the given <paramref name="parameter"/>.
        /// </summary>
        /// <returns>True if null can be assigned, false otherwise.</returns>
        public static bool IsNullable(this ParameterInfo parameter)
        {
            return !parameter.ParameterType.IsValueType || parameter.ParameterType.IsSubclassOf(typeof(Nullable));
        }

        /// <summary>
        /// Determines whether the given <paramref name="parameter"/> has the given <paramref name="name"/>.
        /// The comparison uses OrdinalIgnoreCase and allows for a leading underscore in either name
        /// to be ignored.
        /// </summary>
        /// <returns>True if the name is considered identical, false otherwise. If either parameter
        /// is null an exception will be thrown.</returns>
        public static bool HasName(this ParameterInfo parameter, string name)
        {
            string parameterName = parameter.Name.Length > 0 && parameter.Name[0] == '_'
                                       ? parameter.Name.Substring(1)
                                       : parameter.Name;
            name = name.Length > 0 && name[0] == '_' ? name.Substring(1) : name;
            return parameterName.Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the given <paramref name="parameter"/> has an associated default value as
        /// supplied by an <see href="DefaultValueAttribute"/>. This method does not read the value of
        /// the attribute. It also does not support C# 4.0 default parameter specifications.
        /// </summary>
        /// <returns>True if the attribute was detected, false otherwise.</returns>
        public static bool HasDefaultValue(this ParameterInfo parameter)
        {
            var defaultValue = parameter.Attribute<DefaultValueAttribute>();
            return defaultValue != null;
        }

        /// <summary>
        /// Gets the default value associated with the given <paramref name="parameter"/>. The value is
        /// obtained from the <see href="DefaultValueAttribute"/> if present on the parameter. This method
        /// does not support C# 4.0 default parameter specifications.
        /// </summary>
        /// <returns>The default value if one could be obtained and converted into the type of the parameter,
        /// and null otherwise.</returns>
        public static object DefaultValue(this ParameterInfo parameter)
        {
            var defaultValue = parameter.Attribute<DefaultValueAttribute>();
            return defaultValue != null
                       ? Probing.TypeConverter.Get(parameter.ParameterType, defaultValue.Value)
                       : null;
        }
    }
}