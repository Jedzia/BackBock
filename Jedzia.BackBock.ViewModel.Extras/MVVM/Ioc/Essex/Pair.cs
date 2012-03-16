namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    /// <summary>
    /// General purpose class to represent a standard pair of values. 
    /// </summary>
    /// <typeparam name="TFirst">Type of the first value</typeparam>
    /// <typeparam name="TSecond">Type of the second value</typeparam>
    public class Pair<TFirst, TSecond> : IEquatable<Pair<TFirst, TSecond>>
    {
        private readonly TFirst first;
        private readonly TSecond second;

        /// <summary>
        /// Constructs a pair with its values
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public Pair(TFirst first, TSecond second)
        {
            this.first = first;
            this.second = second;
        }

        public TFirst First
        {
            get { return first; }
        }

        public TSecond Second
        {
            get { return second; }
        }

        public override string ToString()
        {
            return first + " " + second;
        }

        public bool Equals(Pair<TFirst, TSecond> other)
        {
            if (other == null)
            {
                return false;
            }
            return Equals(first, other.first) && Equals(second, other.second);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return Equals(obj as Pair<TFirst, TSecond>);
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() + 29 * second.GetHashCode();
        }
    }
}