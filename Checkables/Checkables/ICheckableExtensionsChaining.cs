using System;

namespace Checkables
{
    public static class ICheckableExtensionsChaining
    {
        public static Maybe<U> FlatMap<T, U>(this ICheckable<T> checkable, Func<T, Maybe<U>> func)
            where T : class
            where U : class
        {
            if (checkable.HasValue)
            {
                return func(checkable.Value);
            }
            else
            {
                return new Maybe<U>(null);
            }
        }

        public static Maybe<U> Map<T, U>(this ICheckable<T> checkable, Func<T, U> func)
            where T : class
            where U : class
        {
            if (checkable.HasValue)
            {
                return new Maybe<U>(func(checkable.Value));
            }
            else
            {
                return new Maybe<U>(null);
            }
        }
    }
}
