using System;

namespace Mammatus.Data.NHibernate.Entities.Base
{
    internal class Entity
    {
        public DateTime Created { get; set; }
        public object CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public object ModifiedBy { get; set; }
    }
}