using System;
using System.Runtime.InteropServices;

namespace Jam.Core
{

    [StructLayout(LayoutKind.Auto)]
    public struct TypeNamePair : IEquatable<TypeNamePair>
    {
        private readonly Type _type;
        private readonly string _name;

        public Type Type => _type;

        public string Name => _name;

        public TypeNamePair(Type type)
            : this(type, string.Empty)
        {
        }

        public TypeNamePair(Type type, string name)
        {
            if (type == null)
            {
                throw new Exception("Type is invalid.");
            }

            _type = type;
            _name = name ?? string.Empty;
        }

        public override string ToString()
        {
            if (_type == null)
            {
                throw new Exception("Type is invalid.");
            }

            string typeName = _type.FullName;
            return string.IsNullOrEmpty(_name) ? typeName : Util.Text.Format("{0}.{1}", typeName, _name);
        }

        public override int GetHashCode()
        {
            return _type.GetHashCode() ^ _name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is TypeNamePair && Equals((TypeNamePair)obj);
        }

        public bool Equals(TypeNamePair value)
        {
            return _type == value._type && _name == value._name;
        }

        public static bool operator ==(TypeNamePair a, TypeNamePair b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TypeNamePair a, TypeNamePair b)
        {
            return !(a == b);
        }
    }

}