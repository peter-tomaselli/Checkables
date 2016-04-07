
namespace Checkables
{
    public struct Maybe<T> : ICheckable<T>
        where T : class
    {
        private T _value { get; set; }

        public bool HasValue
        {
            get
            {
                return _value != null;
            }
        }

        public T Value
        {
            get
            {
                if (HasValue)
                {
                    return _value;
                }
                else
                {
                    throw new CheckableException("value was null");
                }
            }
        }

        public Maybe(T value)
            : this()
        {
            _value = value;
        }
    }
}
