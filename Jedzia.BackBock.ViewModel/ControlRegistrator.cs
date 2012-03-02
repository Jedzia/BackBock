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

    /// <summary>
    /// Central point for registering controls, that can be requested by demand.
    /// </summary>
    public static class ControlRegistrator
    {
        #region Fields

        private static readonly Dictionary<Enum, Type> RegisteredControlTypes = new Dictionary<Enum, Type>();

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
            var t = RegisteredControlTypes[key];
            var instance = CreateInstanceFromType<T>(t, parameters);
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
            Guard.NotNull(() => key, key);
            Guard.NotNull(() => type, type);

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
            RegisteredControlTypes.Add(key, type);

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
            var instance = cnstr.Invoke(parameters) as T;
            return instance;
        }
    }
}