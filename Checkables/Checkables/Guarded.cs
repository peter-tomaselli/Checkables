using System;

namespace Checkables
{
    /// <summary>
    /// an instance of Guarded is a container for a value of type T. Callers only have access to this value if certain conditions are met
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Guarded<T> : ICheckable<T>
        where T : class
    {
        private IGuardedImpl<T> _impl { get; set; }

        /// <summary>
        /// HasValue returns false if the caller does not have access to the value contained in this instance, or if this instance does not contain a value
        /// </summary>
        public bool HasValue
        {
            get
            {
                return _impl.HasValue;
            }
        }

        /// <summary>
        /// get the value contained by this instance. If there is no value, or the caller does not have access to the value, this property throws an exception
        /// </summary>
        public T Value
        {
            get
            {
                return _impl.Value;
            }
        }

        /// <summary>
        /// construct an instance of Guarded from a 'factory' function and an optional check function. The function passed as `check` is evaluated anew every time this instance needs to determine if a caller has access. The function passed as `func` is evaluated only once, lazily, to generate the value contained inside this instance
        /// </summary>
        /// <param name="func">a function that produces a value of type T. This function is evaluated only once, lazily. This parameter must not be null</param>
        /// <param name="check">a function that evaluates to true if callers should be allowed access to this instance's value, and false if not. This function is evaluated multiple times. This parameter may be null</param>
        public Guarded(Func<T> func, Func<bool> check = null)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            Func<Maybe<T>> maybeFunc = () => new Maybe<T>(func());
            if (check == null)
            {
                _impl = new GuardedImplNoCheck<T>(maybeFunc);
            }
            else
            {
                _impl = new GuardedImplWithCheck<T>(maybeFunc, check);
            }
        }

        public Guarded(Func<Maybe<T>> func, Func<bool> check = null)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            if (check == null)
            {
                _impl = new GuardedImplNoCheck<T>(func);
            }
            else
            {
                _impl = new GuardedImplWithCheck<T>(func, check);
            }
        }

        private interface IGuardedImpl<A>
        {
            bool HasValue { get; }

            A Value { get; }
        }

        private class GuardedImplNoCheck<A> : IGuardedImpl<A>
            where A : class
        {
            private Lazy<Maybe<A>> _value { get; set; }

            public bool HasValue
            {
                get
                {
                    return _value.Value.HasValue;
                }
            }

            public A Value
            {
                get
                {
                    if (HasValue)
                    {
                        return _value.Value.Value;
                    }
                    else
                    {
                        throw new CheckableException("value was null");
                    }
                }
            }

            public GuardedImplNoCheck(Func<Maybe<A>> func)
            {
                if (func == null)
                {
                    throw new ArgumentNullException("func");
                }

                _value = new Lazy<Maybe<A>>(() => func(), false);
            }
        }

        private class GuardedImplWithCheck<A> : IGuardedImpl<A>
            where A : class
        {
            private Func<bool> _check { get; set; }

            private Lazy<Maybe<A>> _value { get; set; }

            public bool HasValue
            {
                get
                {
                    return _check() && _value.Value.HasValue;
                }
            }

            public A Value
            {
                get
                {
                    if (HasValue)
                    {
                        return _value.Value.Value;
                    }
                    else
                    {
                        throw new CheckableException("check failed or value was null");
                    }
                }
            }

            public GuardedImplWithCheck(Func<Maybe<A>> func, Func<bool> check)
            {
                if (check == null)
                {
                    throw new ArgumentNullException("check");
                }

                if (func == null)
                {
                    throw new ArgumentNullException("func");
                }

                _check = check;
                _value = new Lazy<Maybe<A>>(() => func(), false);
            }
        }
    }
}
