using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    namespace YourProject.Domain.Common
    {
        public abstract class ValueObject
        {
            // كل Value Object لازم يحدد القيم اللي يتقارن بيها
            protected abstract IEnumerable<object> GetEqualityComponents();

            public override bool Equals(object obj)
            {
                if (obj is null || obj.GetType() != GetType())
                    return false;

                var valueObject = (ValueObject)obj;

                return GetEqualityComponents()
                        .SequenceEqual(valueObject.GetEqualityComponents());
            }

            public override int GetHashCode()
            {
                return GetEqualityComponents()
                        .Aggregate(1, (current, obj) =>
                        {
                            unchecked
                            {
                                return current * 23 + (obj?.GetHashCode() ?? 0);
                            }
                        });
            }

            public static bool operator ==(ValueObject left, ValueObject right)
            {
                if (left is null && right is null)
                    return true;

                if (left is null || right is null)
                    return false;

                return left.Equals(right);
            }

            public static bool operator !=(ValueObject left, ValueObject right)
            {
                return !(left == right);
            }
        }
    }

}
