using Mammatus.Resources;
using System;

namespace Mammatus.Security
{
    /// <summary>
    /// Class used to hold user name info.
    /// </summary>
    [Serializable]
    public class CoreIdentity : MarshalByRefObject, ICoreIdentity
    {
        // Fields
        private readonly string id;
        private readonly string name;
        private readonly string authenticationType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreIdentity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public CoreIdentity(string name)
            : this(name, "Unknown", name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreIdentity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="authenticationType"></param>
        public CoreIdentity(string name, string authenticationType)
            : this(name, authenticationType, name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreIdentity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="authenticationType"></param>
        /// <param name="id">The id.</param>
        public CoreIdentity(string name, string authenticationType, string id)
        {
            if (name == null)
            {
                throw new ArgumentNullException(Resource.NameCannotBeNull);
            }

            if (authenticationType == null)
            {
                throw new ArgumentNullException(Resource.TypeCannotBeNull);
            }

            if (id == null)
            {
                throw new ArgumentNullException(Resource.TypeCannotBeNull);
            }

            this.name = name;
            this.authenticationType = authenticationType;
            this.id = id;
        }

        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        /// <value></value>
        /// <returns>The type of authentication used to identify the user.</returns>
        public virtual string AuthenticationType
        {
            get
            {
                return this.authenticationType;
            }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public string Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        /// <value></value>
        /// <returns>true if the user was authenticated; otherwise, false.</returns>
        public virtual bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(this.name);
            }
        }

        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the user on whose behalf the code is running.</returns>
        public virtual string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}