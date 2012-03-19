// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlRegistrator.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Jedzia.BackBock.ViewModel.Util;
    using System.Text;

    public interface IInstanceLifetime
    {
        object CreateInstance(object initial);
    }

    /// <summary>
    /// SingletonInstance.
    /// </summary>
    [Serializable]
    public class SingletonInstance : IInstanceLifetime
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Instance.
        /// </summary>
        /// <value>The Instance.</value>
        //internal object Instance { get; private set; }
        private static Dictionary<Type, object> instances;
        #endregion
        #region Constructors

        static SingletonInstance()
        {
            instances = new Dictionary<Type, object>();
        }
        /// <summary>
        /// Initializes a new fully specified instance of the <see cref="SingletonInstance"/> class.
        /// </summary>
        /// <param name="Instance">The Instance</param>
        public SingletonInstance()
        {
        }
        #endregion

        #region IInstanceLifetime Members

        public object CreateInstance(object initial)
        {
            var instanceType = initial.GetType();
            if (!instances.ContainsKey(instanceType))
            {
                instances.Add(instanceType, initial);
            }
            return instances[instanceType];
        }

        #endregion
    }

    /// <summary>
    /// SingletonInstance.
    /// </summary>
    [Serializable]
    public class NormalInstance : IInstanceLifetime
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Instance.
        /// </summary>
        /// <value>The Instance.</value>
        //internal object Instance { get; private set; }
        #endregion
        #region Constructors

        /// <summary>
        /// Initializes a new fully specified instance of the <see cref="SingletonInstance"/> class.
        /// </summary>
        /// <param name="Instance">The Instance</param>
        public NormalInstance()
        {
            //Instance = instance;
        }
        #endregion

        #region IInstanceLifetime Members

        public object CreateInstance(object initial)
        {
            return initial;
        }

        #endregion
    }

    /// <summary>
    /// InstanceInfo.
    /// </summary>
    [Serializable]
    internal class InstanceInfo /*: IEquatable<InstanceInfo>*/
    {
        #region Properties
        /// <summary>
        /// Gets or sets the Instance.
        /// </summary>
        /// <value>The Instance.</value>
        internal Type InstanceType { get; private set; }
        /// <summary>
        /// Gets or sets the Lifetime.
        /// </summary>
        /// <value>The Lifetime.</value>
        internal IInstanceLifetime Lifetime { get; private set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceInfo"/> class.
        /// </summary>
        internal InstanceInfo() { }
        /// <summary>
        /// Initializes a new fully specified instance of the <see cref="InstanceInfo"/> class.
        /// </summary>
        /// <param name="Instance">The Instance</param>
        /// <param name="Lifetime">The Lifetime</param>
        internal InstanceInfo(Type instanceType, IInstanceLifetime lifetime)
        {
            this.InstanceType = instanceType;
            this.Lifetime = lifetime;
        }
        #endregion
   
        #region Methods
        /*
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            InstanceInfo other = obj as InstanceInfo;
            if (other != null)
                return Equals(other);
            return false;
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(InstanceInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return
              InstanceType == other.InstanceType &&
              Lifetime == other.Lifetime;
        }
        */

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("InstanceType = " + InstanceType + ";");
            sb.Append("Lifetime = " + Lifetime);
            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// Central point for registering controls, that can be requested by demand.
    /// </summary>
    public static class ControlRegistrator
    {
        #region Fields

        private static readonly Dictionary<Enum, InstanceInfo> RegisteredControlTypes = new Dictionary<Enum, InstanceInfo>();

        #endregion

        /// <summary>
        /// Gets an new instance of the specified type from the <see cref="ControlRegistrator"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key, that identifies the requested control.</param>
        /// <returns></returns>
        public static T GetInstanceOfType<T>(Enum key) where T : class
        {
            return GetInstanceOfType<T>(key, null);
        }

        /// <summary>
        /// Gets an new instance of the specified type from the <see cref="ControlRegistrator"/> with
        /// optional constructor parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key, that identifies the requested control.</param>
        /// <param name="parameters">The constructor parameters used to instantiate the new type.</param>
        /// <returns>A new instance of the requested type.</returns>
        public static T GetInstanceOfType<T>(Enum key, object[] parameters) where T : class
        {
            var iinfo = RegisteredControlTypes[key];
            var type = iinfo.InstanceType;
            var instance = (T)iinfo.Lifetime.CreateInstance(CreateInstanceFromType<T>(type, parameters));
            return instance;
        }

                /// <summary>
        /// Registers a control into the global <see cref="ControlRegistrator"/>.
        /// </summary>
        /// <param name="key">The key, that should identify the registered control.</param>
        /// <param name="type">The type to register.</param>
        /// <exception cref="NotSupportedException">Can't register type. The type is no instance of the
        /// registered type identified by <see cref="CheckTypeAttribute"/>.</exception>
        public static void RegisterControl(Enum key, Type type)
        {
            var lifetime = new NormalInstance();
            RegisterControl(key, type, lifetime);
        }

        /// <summary>
        /// Registers a control into the global <see cref="ControlRegistrator"/>.
        /// </summary>
        /// <param name="key">The key, that should identify the registered control.</param>
        /// <param name="type">The type to register.</param>
        /// <exception cref="NotSupportedException">Can't register type. The type is no instance of the
        /// registered type identified by <see cref="CheckTypeAttribute"/>.</exception>
        public static void RegisterControl(Enum key, Type type, IInstanceLifetime lifetime)
        {
            Gu.NotNull(() => key, key);
            Gu.NotNull(() => type, type);
            Gu.NotNull(() => lifetime, lifetime);

            // classSpecificationWindowType = type;
            // var xxx = Data.BackupItemViewModel.WindowTypes.TaskEditor.GetType();
            // var yyy = xxx.GetCustomAttributes(false);
            var kindType = key.GetType();
            var member = kindType.GetMembers().FirstOrDefault(e => e.Name == key.ToString());
            if (member != null)
            {
                var attrs = member.GetCustomAttributes(false);
                var ctattr = attrs.OfType<CheckTypeAttribute>();
                foreach(var item in ctattr)
                {
                    if (!type.IsSubclassOf(item.Type))
                    {
                        throw new NotSupportedException(
                            "Can't register type. The type "
                            + type + " is no instance of " + item.Type);
                    }

                    /*if (!type.IsInstanceOfType(item.Type))
                                        {
                                            throw new NotSupportedException("Can't register type. The type "
                                                + type.ToString() + " is no instance of " + item.Type.ToString());
                                        }*/
                }
            }

            // var values = Enum.GetValues(kindType);
            // var attrs = kindType.GetCustomAttributes(false);
            RegisteredControlTypes.Add(key, new InstanceInfo(type, lifetime));

            // var w = CreateInstanceFromType<Window>(type);
        }

        /// <summary>
        /// Resets this instance for test purposes.
        /// </summary>
        internal static void Reset()
        {
            RegisteredControlTypes.Clear();
        }

        private static T CreateInstanceFromType<T>(Type type, object[] parameters) where T : class
        {
            // Todo: parameter and checks for the type
            var types = new Type[0];
            if (parameters != null)
            {
                types = new Type[parameters.Length];

                for (var index = 0; index < parameters.Length; index++)
                {
                    types[index] = parameters[index].GetType();
                }
            }

            var cnstr = type.GetConstructor(types);
            // Todo: wrap in try...catch and feed a logger.
            var instance = cnstr.Invoke(parameters) as T;
            return instance;
        }
    }
}