using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Core.Infrastructure
{
    [Serializable]
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.Id, default(int));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }
}

//http://www.loganfranken.com/blog/692/overriding-equals-in-c-part-2/
//A good implementation of GetHashCode
//Implement Equal, GetHashCode, ==, != together for convinience

//public override int GetHashCode()
//{
//    unchecked
//    {
//        // Choose large primes to avoid hashing collisions
//        const int HashingBase = (int)2166136261;
//        const int HashingMultiplier = 16777619;

//        int hash = HashingBase;
//        hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, AreaCode) ? AreaCode.GetHashCode() : 0);
//        hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, Exchange) ? Exchange.GetHashCode() : 0);
//        hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, SubscriberNumber) ? SubscriberNumber.GetHashCode() : 0);
//        return hash;
//    }
//}