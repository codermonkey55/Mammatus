using Mammatus.Data.NHibernate.Entities;

namespace Mammatus.Data.NHibernate.FluentMappings.ClassMaps
{
    public class UserMap : BaseMaps.AuditMap<User>
    {
        public UserMap()
        {
            this.Id(x => x.Id).Column("UserId").Not.Nullable().GeneratedBy.Assigned();

            this.Map(x => x.FirstName).Length(50).Not.Nullable();
            this.Map(x => x.LastName).Length(50).Not.Nullable();
            this.Map(x => x.FullName).Length(100)
                                     .Nullable()
                                     .Formula(string.Format("{0} + ' ' + {1}", "FirstName", "LastName"));

            this.References<Token>(x => x.ActiveToken).Column("ActiveTokenId").Nullable();

            this.HasMany(b => b.Tokens).LazyLoad()
                                       .AsSet()
                                       .KeyColumn("UserId");

            //this.DynamicInsert();
            //this.DynamicUpdate();

            //this.Map(x => x.CreateDate);
            //this.Map(x => x.EditDate);

            //this.Polymorphism.Explicit();
            //this.ImportType<BrochureDto>();
            //this.Component(x => x.AuditInfo);
        }
    }
}