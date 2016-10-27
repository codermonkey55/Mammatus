using Mammatus.Data.NHibernate.Entities;

namespace Mammatus.Data.NHibernate.FluentMappings.ClassMaps
{
    public class TokenMap : BaseMaps.AuditMap<Token>
    {
        public TokenMap()
        {
            this.Id(x => x.Id).Not.Nullable().Column("TokenId").GeneratedBy.Native();
            this.Map(x => x.AuthKey).Not.Nullable();

            this.References<User>(x => x.User).Column("UserId").Not.Nullable();

            this.HasMany(b => b.Users).LazyLoad()
                                      .AsSet()
                                      .KeyColumn("ActiveTokenId");

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