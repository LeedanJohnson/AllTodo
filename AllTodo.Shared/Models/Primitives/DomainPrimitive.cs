using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AllTodo.Shared.Models.Primitives
{
    public abstract class DomainPrimitive<TDomainPrimitive> : IEquatable<TDomainPrimitive>
    where TDomainPrimitive : DomainPrimitive<TDomainPrimitive>
    {
        public abstract override bool Equals(Object other);

        public bool Equals(TDomainPrimitive other)
        {
            return this.Equals((Object)other);
        }

        public static bool operator ==(DomainPrimitive<TDomainPrimitive> x, DomainPrimitive<TDomainPrimitive> y)
        {
            if (System.Object.ReferenceEquals(x, y))
                return true;

            if (((object)x == null) || ((object)y == null))
                return false;

            return x.Equals(y);
        }

        public static bool operator !=(DomainPrimitive<TDomainPrimitive> x, DomainPrimitive<TDomainPrimitive> y)
        {
            return !(x == y);
        }

        public static bool operator ==(DomainPrimitive<TDomainPrimitive> x, Object y)
        {
            if (System.Object.ReferenceEquals(x, y))
                return true;

            if (((object)x == null) || ((object)y == null))
                return false;

            return x.Equals(y);
        }

        public static bool operator !=(DomainPrimitive<TDomainPrimitive> x, Object y)
        {
            return !(x == y);
        }
    }
}
