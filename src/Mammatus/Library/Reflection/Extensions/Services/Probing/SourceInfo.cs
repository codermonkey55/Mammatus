using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mammatus.Library.Reflection.Probing
{
    internal class SourceInfo
    {
        #region Fields
        private readonly Type type;
        private readonly bool[] paramKinds;
        private readonly string[] paramNames;
        private readonly Type[] paramTypes;
        private MemberGetter[] paramValueReaders;
        #endregion

        #region Constructors
        public SourceInfo(Type type, string[] names, Type[] types)
        {
            this.type = type;
            paramNames = names;
            paramTypes = types;
            paramKinds = new bool[names.Length];
            // this overload assumes that all names refer to fields on the given type
            for (int i = 0; i < paramKinds.Length; i++)
            {
                paramKinds[i] = true;
            }
        }

        public SourceInfo(Type type, string[] names, Type[] types, bool[] kinds)
        {
            this.type = type;
            paramNames = names;
            paramTypes = types;
            paramKinds = kinds;
        }

        public static SourceInfo CreateFromType(Type type)
        {
            IList<MemberInfo> members = type.Members(MemberTypes.Field | MemberTypes.Property, Flags.InstanceAnyVisibility);
            var names = new List<string>(members.Count);
            var types = new List<Type>(members.Count);
            var kinds = new List<bool>(members.Count);
            for (int i = 0; i < members.Count; i++)
            {
                MemberInfo mi = members[i];
                bool include = mi is FieldInfo && mi.Name[0] != '<'; // exclude auto-generated backing fields
                include |= mi is PropertyInfo && (mi as PropertyInfo).CanRead; // exclude write-only properties
                if (include)
                {
                    names.Add(mi.Name);
                    bool isField = mi is FieldInfo;
                    kinds.Add(isField);
                    types.Add(isField ? (mi as FieldInfo).FieldType : (mi as PropertyInfo).PropertyType);
                }
            }
            return new SourceInfo(type, names.ToArray(), types.ToArray(), kinds.ToArray());
        }
        #endregion

        #region Properties
        public Type Type
        {
            get { return type; }
        }

        public string[] ParamNames
        {
            get { return paramNames; }
        }

        public Type[] ParamTypes
        {
            get { return paramTypes; }
        }

        public bool[] ParamKinds
        {
            get { return paramKinds; }
        }

        public MemberGetter[] ParamValueReaders
        {
            get
            {
                InitializeParameterValueReaders();
                return paramValueReaders;
            }
        }
        #endregion

        #region Parameter Value Access
        public object[] GetParameterValues(object source)
        {
            InitializeParameterValueReaders();
            var paramValues = new object[paramNames.Length];
            for (int i = 0; i < paramNames.Length; i++)
            {
                paramValues[i] = paramValueReaders[i](source);
            }
            return paramValues;
        }

        internal MemberGetter GetReader(string memberName)
        {
            int index = Array.IndexOf(paramNames, memberName);
            MemberGetter reader = paramValueReaders[index];
            if (reader == null)
            {
                reader = paramKinds[index] ? type.DelegateForGetFieldValue(memberName) : type.DelegateForGetPropertyValue(memberName);
                paramValueReaders[index] = reader;
            }
            return reader;
        }

        private void InitializeParameterValueReaders()
        {
            if (paramValueReaders == null)
            {
                paramValueReaders = new MemberGetter[paramNames.Length];
                for (int i = 0; i < paramNames.Length; i++)
                {
                    string name = paramNames[i];
                    paramValueReaders[i] = paramKinds[i] ? type.DelegateForGetFieldValue(name) : type.DelegateForGetPropertyValue(name);
                }
            }
        }
        #endregion

        #region Equals + GetHashCode
        public override bool Equals(object obj)
        {
            var other = obj as SourceInfo;
            if (other == null) return false;
            if (other == this) return true;

            if (type != other.Type || paramNames.Length != other.ParamNames.Length)
                return false;
            for (int i = 0; i < paramNames.Length; i++)
            {
                if (paramNames[i] != other.ParamNames[i] || paramTypes[i] != other.ParamTypes[i] || paramKinds[i] != other.ParamKinds[i])
                    return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            int hash = type.GetHashCode();
            for (int i = 0; i < paramNames.Length; i++)
                hash += (i + 31) * paramNames[i].GetHashCode() ^ paramTypes[i].GetHashCode();
            return hash;
        }
        #endregion
    }
}