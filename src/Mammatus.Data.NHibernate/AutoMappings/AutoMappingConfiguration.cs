using FluentNHibernate.Automapping;
using Mammatus.Data.NHibernate.Entities.Components;
using System;

namespace Mammatus.Data.NHibernate.AutoMappings
{
    /// <summary>
    /// This is an example automapping configuration. You should create your own that either
    /// implements IAutomappingConfiguration directly, or inherits from DefaultAutomappingConfiguration.
    /// Overriding methods in this class will alter how the automapper behaves.
    /// </summary>
    public class AutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            // specify the criteria that types must meet in order to be mapped
            // any type for which this method returns false will not be mapped.

            string typeNamespace = type.Namespace;

            bool mapType = typeNamespace == "Mammatus.Data.NHibernate.Entities" || typeNamespace == "Mammatus.Data.NHibernate.Entities.Components";

            return mapType;
        }

        public override bool IsComponent(Type type)
        {
            // override this method to specify which types should be treated as components
            // if you have a large list of types, you should consider maintaining a list of them
            // somewhere or using some form of conventional and/or attribute design

            bool mapComponent = type == typeof(AuditInfo);

            return mapComponent;
        }
    }
}
