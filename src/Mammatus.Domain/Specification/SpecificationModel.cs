﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Mammatus.Domain.Specification
{
    [DataContract]
    public class Filter
    {
        [DataMember]
        public string groupOp
        {
            get;
            set;
        }

        [DataMember]
        public Rule[] rules
        {
            get;
            set;
        }

        public static Filter Create(string jsonData)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(Filter));
                using (System.IO.StringReader reader = new System.IO.StringReader(jsonData))
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Encoding.Default.GetBytes(jsonData)))
                {
                    return serializer.ReadObject(ms) as Filter;
                }
            }
            catch
            {
                return null;
            }
        }
    }

    public static class LinqExtensions
    {
        public static ISpecification<T> AndAlso<T>(this ISpecification<T> query, string column, object value, string operation)
        where T : class
        {
            return query.AndAlso(CreateSpecification<T>(column, value, operation));
        }

        public static ISpecification<T> CreateSpecification<T>(string column, object value, string operation)
        where T : class
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");

            MemberExpression memberAccess = _GetMemberAccess<T>(column, parameter);

            if (memberAccess.Type == typeof(DateTime))
            {
                column += ".Date";
                memberAccess = _GetMemberAccess<T>(column, parameter);
            }

            if (memberAccess.Type == typeof(System.Nullable<DateTime>))
            {
                column += ".Value";
                memberAccess = _GetMemberAccess<T>(column, parameter);
            }

            // change param value type necessary to getting bool from string
            ConstantExpression filter = Expression.Constant(Convert.ChangeType(value, memberAccess.Type));

            Expression condition = null;
            LambdaExpression lambda = null;
            switch (operation)
            {
                // equal ==
                case "eq":
                    condition = Expression.Equal(memberAccess, filter);

                    lambda = Expression.Lambda(condition, parameter);
                    break;

                // not equal !=
                case "ne":
                    condition = Expression.NotEqual(memberAccess, filter);
                    lambda = Expression.Lambda(condition, parameter);
                    break;

                // string.Contains()
                case "cn":
                    condition = Expression.Call(
                                    memberAccess,
                                    typeof(string).GetMethod("Contains"),
                                    Expression.Constant(value));

                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case "bw":
                    condition = Expression.Call(
                                    memberAccess,
                                    typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                                    Expression.Constant(value));

                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case "bn":
                    condition = Expression.Call(
                                    memberAccess,
                                    typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                                    Expression.Constant(value));

                    condition = Expression.Not(condition);

                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case "ew":
                    condition = Expression.Call(
                                    memberAccess,
                                    typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                                    Expression.Constant(value));

                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case "en":
                    condition = Expression.Call(
                                    memberAccess,
                                    typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                                    Expression.Constant(value));

                    condition = Expression.Not(condition);

                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case "gt":
                    condition = Expression.GreaterThan(memberAccess, filter);

                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case "ge":
                    condition = Expression.GreaterThanOrEqual(memberAccess, filter);

                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case "lt":
                    condition = Expression.LessThan(memberAccess, filter);

                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case "le":
                    condition = Expression.LessThanOrEqual(memberAccess, filter);

                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case "nc":
                    condition = Expression.Call(
                                    memberAccess,
                                    typeof(string).GetMethod("Contains"),
                                    Expression.Constant(value));

                    condition = Expression.Not(condition);

                    lambda = Expression.Lambda(condition, parameter);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }

            Expression<Func<T, bool>> hLambda = Expression.Lambda<Func<T, bool>>(condition, parameter);

            return new DirectSpecification<T>(hLambda);
        }

        public static ISpecification<T> OrElse<T>(this ISpecification<T> query, string column, object value, string operation)
        where T : class
        {
            return query.OrElse(CreateSpecification<T>(column, value, operation));
        }

        public static ISpecification<T> ToSpecification<T>(this SpecificationModel specificationModel)
        where T : class
        {
            return specificationModel.ToSpecification<T>(null);
        }

        public static ISpecification<T> ToSpecification<T>(this SpecificationModel specificationModel, ISpecification<T> specification)
        where T : class
        {
            foreach (Rule rule in specificationModel.Where.rules)
            {
                if (rule.data != "")
                {
                    if (specification == null)
                    {
                        specification = LinqExtensions.CreateSpecification<T>(rule.field, rule.data, rule.op);
                    }
                    else
                    {
                        if (specificationModel.Where.groupOp.ToLower() == "and")
                        {
                            specification = specification.AndAlso(rule.field, rule.data, rule.op);
                        }
                        else
                        {
                            specification = specification.OrElse(rule.field, rule.data, rule.op);
                        }
                    }
                }
            }

            return specification;
        }

        private static MemberExpression _GetMemberAccess<T>(string column, ParameterExpression parameter)
        {
            Type inspectedType = typeof(T);
            MemberExpression memberAccess = null;
            foreach (string property in column.Split('.'))
            {
                PropertyInfo propertyInfo = inspectedType.GetProperty(property);
                if (propertyInfo == null && inspectedType.IsInterface)
                {
                    Type[] implementedInterfaces = inspectedType.GetInterfaces();
                    foreach (Type implementedInterface in implementedInterfaces)
                    {
                        propertyInfo = implementedInterface.GetProperty(property);
                        if (propertyInfo != null)
                        {
                            break;
                        }
                    }
                }

                if (propertyInfo == null)
                {
                    throw new MissingMemberException(typeof(T).FullName, column);
                }

                memberAccess = Expression.Property(memberAccess ?? (parameter as Expression), propertyInfo);

                inspectedType = propertyInfo.PropertyType;
            }

            return memberAccess;
        }
    }

    [DataContract]
    public class Rule
    {
        [DataMember]
        public string data
        {
            get;
            set;
        }

        [DataMember]
        public string field
        {
            get;
            set;
        }

        [DataMember]
        public string op
        {
            get;
            set;
        }
    }

    public class SpecificationModel
    {
        public string Field
        {
            get;
            set;
        }

        public bool IsSearch
        {
            get;
            set;
        }

        public string Operator
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public string SearchString
        {
            get;
            set;
        }

        public string SortColumn
        {
            get;
            set;
        }

        public string SortOrder
        {
            get;
            set;
        }

        public Filter Where
        {
            get;
            set;
        }
    }
}