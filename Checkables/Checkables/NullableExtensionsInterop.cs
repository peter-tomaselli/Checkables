using System;

namespace Checkables
{
    public static class NullableExtensionsInterop
    {
        public static Nullable<U> FlatMapN<T, U>(this ICheckable<T> checkable, Func<T, Nullable<U>> func)
            where T : class
            where U : struct
        {
            if (checkable.HasValue)
            {
                return func(checkable.Value);
            }
            else
            {
                return new Nullable<U>();
            }
        }

        public static Nullable<U> MapN<T, U>(this ICheckable<T> checkable, Func<T, U> func)
            where T : class
            where U : struct
        {
            if (checkable.HasValue)
            {
                return new Nullable<U>(func(checkable.Value));
            }
            else
            {
                return new Nullable<U>();
            }
        }

        public static Maybe<U> FlatMapM<T, U>(this Nullable<T> nullable, Func<T, Maybe<U>> func)
            where T : struct
            where U : class
        {
            if (nullable.HasValue)
            {
                return func(nullable.Value);
            }
            else
            {
                return new Maybe<U>(null);
            }
        }

        public static Maybe<U> MapM<T, U>(this Nullable<T> nullable, Func<T, U> func)
            where T : struct
            where U : class
        {
            if (nullable.HasValue)
            {
                return new Maybe<U>(func(nullable.Value));
            }
            else
            {
                return new Maybe<U>(null);
            }
        }
    }

    public static class NullableExtensionsChaining
    {
        public static Nullable<U> FlatMap<T, U>(this Nullable<T> nullable, Func<T, Nullable<U>> func)
            where T : struct
            where U : struct
        {
            if (nullable.HasValue)
            {
                return func(nullable.Value);
            }
            else
            {
                return new Nullable<U>();
            }
        }

        public static Nullable<U> Map<T, U>(this Nullable<T> nullable, Func<T, U> func)
            where T : struct
            where U : struct
        {
            if (nullable.HasValue)
            {
                return new Nullable<U>(func(nullable.Value));
            }
            else
            {
                return new Nullable<U>();
            }
        }
    }
}
