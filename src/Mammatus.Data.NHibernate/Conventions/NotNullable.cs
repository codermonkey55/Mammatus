//----------------------------------------------------------------------------------------------
// <copyright file="NotNullable.cs" company="HexaSystems Inc">
// Copyright (c) HexaSystems Inc. Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
//-----------------------------------------------------------------------------------------------

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mammatus.Data.NHibernate.Conventions
{
    public class NotNullable : IPropertyConvention
    {
        public void Apply(IPropertyInstance target)
        {
            var attribute =
                Attribute.GetCustomAttribute(target.Property.MemberInfo, typeof(RequiredAttribute)) as
                RequiredAttribute;

            if (attribute != null)
            {
                target.Not.Nullable();
            }
        }
    }
}