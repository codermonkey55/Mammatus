using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mammatus.Core.Attributes;
using Mammatus.Core.IoC;
using Mammatus.Interface.Contracts;
using Mammatus.Validation;
using Mammatus.Core.Extensions;
using System.Linq.Expressions;
using Mammatus.Helpers;

namespace Mammatus.Core.Application
{
    public abstract class ObjectBase : NotificationObject, IDirtyCapable
    {
        protected bool _IsDirty = false;
        protected IValidator _Validator = null;
        protected IEnumerable<ValidationError> _ValidationErrors = null;
        protected internal IInternalContainer InternalContainer { get; protected set; }

         Validation

        protected virtual IValidator GetValidator()
        {
            return null;
        }

        [NotNavigable]
        public IEnumerable<ValidationError> ValidationErrors
        {
            get { return _ValidationErrors; }
            set { }
        }

        public void Validate()
        {
            if (_Validator != null)
            {
                ValidationResult results = _Validator.Validate(this);
                _ValidationErrors = results.Errors;
            }
        }

        [NotNavigable]
        public virtual bool IsValid
        {
            get
            {
                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                    return false;
                else
                    return true;
            }
        }

        

         IDirtyCapable Members

        [NotNavigable]
        public virtual bool IsDirty
        {
            get { return _IsDirty; }
            protected set
            {
                _IsDirty = value;
                OnPropertyChanged("IsDirty", false);
            }
        }

        public virtual bool IsAnythingDirty()
        {
            bool isDirty = false;

            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                {
                    isDirty = true;
                    return true; // short circuit
                }
                else
                    return false;
            }, coll => { });

            return isDirty;
        }

        public List<IDirtyCapable> GetDirtyObjects()
        {
            List<IDirtyCapable> dirtyObjects = new List<IDirtyCapable>();

            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                    dirtyObjects.Add(o);

                return false;
            }, coll => { });

            return dirtyObjects;
        }

        /// <summary>
        /// Walks the object graph cleaning any dirty object.
        /// </summary>
        public void CleanAll()
        {
            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                    o.IsDirty = false;
                return false;
            }, coll => { });
        }

        

         Protected Methods

        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject,
                                       Action<IList> snippetForCollection,
                                       params string[] exemptProperties)
        {
            List<ObjectBase> visited = new List<ObjectBase>();
            Action<ObjectBase> walk = null;

            List<string> exemptions = new List<string>();
            if (exemptProperties != null)
                exemptions = exemptProperties.ToList();

            walk = (o) =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);

                    bool exitWalk = snippetForObject.Invoke(o);

                    if (!exitWalk)
                    {
                        PropertyInfo[] properties = o.GetBrowsableProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            if (!exemptions.Contains(property.Name))
                            {
                                if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                                {
                                    ObjectBase obj = (ObjectBase)(property.GetValue(o, null));
                                    walk(obj);
                                }
                                else
                                {
                                    IList coll = property.GetValue(o, null) as IList;
                                    if (coll != null)
                                    {
                                        snippetForCollection.Invoke(coll);

                                        foreach (object item in coll)
                                        {
                                            if (item is ObjectBase)
                                                walk((ObjectBase)item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            walk(this);
        }

        

         Property Change Notification

        protected override void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
        {
            string propertyName = PropertyInfoHelper.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName, makeDirty);
        }

        protected void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            base.OnPropertyChanged(propertyName);

            if (makeDirty)
                IsDirty = true;

            Validate();
        }

        
    }
}
