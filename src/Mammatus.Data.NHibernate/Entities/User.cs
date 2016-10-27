using System;
using System.Collections.Generic;

namespace Mammatus.Data.NHibernate.Entities
{
    public class User : Base.Entity<Guid>
    {
        public User()
        {
            Tokens = new HashSet<Token>();
        }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string FullName { get; set; }

        public virtual Token ActiveToken { get; set; }

        public virtual ISet<Token> Tokens { get; protected set; }

        public virtual void AddToken(Token token)
        {
            token.User = this;
            this.Tokens.Add(token);
        }

        public virtual void AssignNewGuidId()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
