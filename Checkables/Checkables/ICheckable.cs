using System;

namespace Checkables
{
    public interface ICheckable<T>
    {
        bool HasValue { get; }

        T Value { get; }
    }
}
