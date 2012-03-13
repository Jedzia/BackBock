namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Essex
{
    using System;

    public class ComponentName
    {
        public ComponentName(string name, bool setByUser)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            Name = name;
            SetByUser = setByUser;
        }

        public string Name { get; private set; }
        public bool SetByUser { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        internal void SetName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            Name = value;
            SetByUser = true;
        }

        /// <summary>
        ///   Gets the default name for component implemented by <paramref name = "componentType" /> which will be used in case when user does not provide one explicitly.
        /// </summary>
        /// <param name = "componentType"></param>
        /// <returns></returns>
        public static ComponentName DefaultFor(Type componentType)
        {
            return new ComponentName(DefaultNameFor(componentType), false);
        }

        /// <summary>
        ///   Gets the default name for component implemented by <paramref name = "componentType" /> which will be used in case when user does not provide one explicitly.
        /// </summary>
        /// <param name = "componentType"></param>
        /// <returns></returns>
        public static string DefaultNameFor(Type componentType)
        {
            return componentType.FullName;
        }
    }
}