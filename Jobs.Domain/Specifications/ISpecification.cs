using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
    }
}
