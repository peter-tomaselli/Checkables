using System;

namespace Checkables
{
    public static class ICheckableExtensionsActions
    {
        public static Action Apply<T>(ICheckable<T> first, Action<T> action)
        {
            return () =>
            {
                if (first.HasValue)
                {
                    action(first.Value);
                }
            };
        }

        public static void Do<T>(this ICheckable<T> first, Action<T> action)
        {
            Apply(first, action)();
        }

        public static Action<T> Apply<T, U>(ICheckable<U> last, Action<T, U> action)
        {
            return t =>
            {
                if (last.HasValue)
                {
                    action(t, last.Value);
                }
            };
        }

        public static void Do<T, U>(this ICheckable<T> first, ICheckable<U> second, Action<T, U> action)
        {
            Apply(first, Apply(second, action))();
        }

        public static Action<T, U> Apply<T, U, V>(ICheckable<V> last, Action<T, U, V> action)
        {
            return (t, u) =>
            {
                if (last.HasValue)
                {
                    action(t, u, last.Value);
                }
            };
        }

        public static void Do<T, U, V>(this ICheckable<T> first, ICheckable<U> second, ICheckable<V> third, Action<T, U, V> action)
        {
            Apply(first, Apply(second, Apply(third, action)))();
        }

        public static Action<T, U, V> Apply<T, U, V, W>(ICheckable<W> last, Action<T, U, V, W> action)
        {
            return (t, u, v) =>
            {
                if (last.HasValue)
                {
                    action(t, u, v, last.Value);
                }
            };
        }

        public static void Do<T, U, V, W>(this ICheckable<T> first, ICheckable<U> second, ICheckable<V> third, ICheckable<W> fourth, Action<T, U, V, W> action)
        {
            Apply(first, Apply(second, Apply(third, Apply(fourth, action))))();
        }
    }
}
