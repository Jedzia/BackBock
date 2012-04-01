// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.ViewModel.Commands
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using Jedzia.BackBock.ViewModel.Util;

    /// <summary>
    /// Describes the binding of <see cref="ICommand"/> properties with <see cref="KeyGesture"/>'s.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class CommandKeyGestureAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandKeyGestureAttribute"/> class with
        /// the specified System.Windows.Input.Key and target filter.
        /// </summary>
        /// <param name="key">The key associated with this gesture..</param>
        /// <param name="target">The filtering target type. Specify a backing field of the instance to 
        /// tag the gesture to only types stored in this field.</param>
        public CommandKeyGestureAttribute(Key key, string target)
        {
            this.Key = new KeyGesture(key);
            this.Target = target;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandKeyGestureAttribute"/> class with
        /// the specified System.Windows.Input.Key.
        /// </summary>
        /// <param name="key">The key associated with this gesture..</param>
        public CommandKeyGestureAttribute(Key key)
            : this(key, null)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CommandKeyGestureAttribute"/> class with.
        /// the specified System.Windows.Input.Key, filter and System.Windows.Input.ModifierKeys.
        /// </summary>
        /// <param name="key">The key associated with this gesture..</param>
        /// <param name="modifiers">The modifier keys associated with the gesture.</param>
        /// <param name="target">The filtering target type. Specify a backing field of the instance to
        /// tag the gesture to only types stored in this field.</param>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">modifiers is not a valid 
        /// <see cref="System.Windows.Input.ModifierKeys"/> -or- key is not a valid 
        /// <see cref="System.Windows.Input.Key"/>.</exception>
        /// <exception cref="System.NotSupportedException">key and modifiers do not form a valid System.Windows.Input.KeyGesture.</exception>
        public CommandKeyGestureAttribute(Key key, ModifierKeys modifiers, string target)
        {
            this.Key = new KeyGesture(key, modifiers);
            this.Target = target;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandKeyGestureAttribute"/> class with.
        /// the specified System.Windows.Input.Key and System.Windows.Input.ModifierKeys.
        /// </summary>
        /// <param name="key">The key associated with this gesture..</param>
        /// <param name="modifiers">The modifier keys associated with the gesture.</param>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">modifiers is not a valid
        /// <see cref="System.Windows.Input.ModifierKeys"/> -or- key is not a valid <see cref="System.Windows.Input.Key"/>.</exception>
        /// <exception cref="System.NotSupportedException">key and modifiers do not form a valid System.Windows.Input.KeyGesture.</exception>
        public CommandKeyGestureAttribute(Key key, ModifierKeys modifiers)
        {
            this.Key = new KeyGesture(key, modifiers);
            this.Target = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandKeyGestureAttribute"/> class with.
        /// the specified System.Windows.Input.Key, System.Windows.Input.ModifierKeys.
        /// input filter and display string.
        /// </summary>
        /// <param name="key">The key associated with this gesture..</param>
        /// <param name="modifiers">The modifier keys associated with the gesture.</param>
        /// <param name="displayString">A string representation of the System.Windows.Input.KeyGesture.</param>
        /// <param name="target">The filtering target type. Specify a backing field of the instance to
        /// tag the gesture to only types stored in this field. Specify a null target if you wan't to
        /// ignore filtering.</param>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">modifiers is not a valid
        ///   <see cref="System.Windows.Input.ModifierKeys"/> -or- key is not a valid
        ///   <see cref="System.Windows.Input.Key"/>.</exception>
        /// <exception cref="System.NotSupportedException">key and modifiers do not form a valid System.Windows.Input.KeyGesture.</exception>
        /// <exception cref="System.ArgumentNullException">displayString is null.</exception>
        public CommandKeyGestureAttribute(Key key, ModifierKeys modifiers, string displayString, string target)
        {
            this.Key = new KeyGesture(key, modifiers, displayString);
            this.Target = target;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandKeyGestureAttribute"/> class
        /// with the specified System.Windows.Input.Key and filter target.
        /// </summary>
        /// <param name="key">The key associated with this gesture..</param>
        /// <param name="target">The filtering target type. Specify a backing field of the instance to
        /// tag the gesture to only types stored in this field.</param>
        public CommandKeyGestureAttribute(KeyGesture key, string target)
        {
            Guard.NotNull(() => key, key);
            this.Key = key;
            this.Target = target;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandKeyGestureAttribute"/> class
        /// with the specified System.Windows.Input.Key.
        /// </summary>
        /// <param name="key">The key associated with this gesture..</param>
        public CommandKeyGestureAttribute(KeyGesture key)
            : this(key, null)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the associated KeyGesture.
        /// </summary>
        public KeyGesture Key { get; private set; }

        /// <summary>
        /// Gets the target of the KeyGesture.
        /// </summary>
        public string Target { get; private set; }

        #endregion

        /// <summary>
        /// Applies key gestures to properties marked with the CommandKeyGesture attribute.
        /// </summary>
        /// <param name="type">The type to search for CommandKeyGesture attributes on public 
        /// properties with the ICommand type.</param>
        /// <param name="inputBinder">The target to bind the key to.</param>
        /// <param name="instance">The instance with the command to bind to.</param>
        public static void ApplyKeyGestures(Type type, ICanInputBind inputBinder, object instance)
        {
            //var properties = type.GetMembers();

            // get all public properties of the parameter type
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in propertyInfos)
            {
                if (!item.PropertyType.IsAssignableFrom(typeof(ICommand)))
                {
                    // this is no ICommand, so move to the next.
                    continue;
                }

                // search for the custom attribute.
                var attributes = item.GetCustomAttributes(typeof(CommandKeyGestureAttribute), true);
                var command = item.GetValue(instance, null) as ICommand;
                if (command == null)
                {
                    // the ICommand is not set, move to the next.
                    continue;
                }

                foreach (var attribute in attributes.OfType<CommandKeyGestureAttribute>())
                {
                    if (!string.IsNullOrEmpty(attribute.Target))
                    {
                        var field = instance.GetType().GetField(
                            attribute.Target, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        if (field != null)
                        {
                            // When there is a target for this binder, then
                            // skip all fields that are not stored in the attributes target.
                            var fieldValue = field.GetValue(instance);
                            if (inputBinder is InputBindWrapper)
                            {
                                var ibunwrap = (InputBindWrapper)inputBinder;
                                if (!ReferenceEquals(fieldValue, ibunwrap.wrapTarget))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (!ReferenceEquals(fieldValue, inputBinder))
                                {
                                    continue;
                                }
                            }
                        }
                    }

                    // create a key binding and attach it to the window, control, etc.
                    var keybinding = new KeyBinding(command, attribute.Key);
                    if (command is RelayCommand)
                    {
                        ((RelayCommand)command).InputGestures.Add(attribute.Key);
                    }
                    inputBinder.InputBindings.Add(keybinding);
                }
            }
        }


        private sealed class InputBindWrapper : ICanInputBind
        {
            public readonly UIElement wrapTarget;
            public InputBindWrapper(UIElement wrapTarget)
            {
                this.wrapTarget = wrapTarget;
            }

            #region ICanInputBind Members

            public InputBindingCollection InputBindings
            {
                get { return this.wrapTarget.InputBindings; }
            }

            #endregion
        }

        /// <summary>
        /// Applies key gestures to properties marked with the CommandKeyGesture attribute.
        /// </summary>
        /// <param name="type">The type to search for CommandKeyGesture attributes on public 
        /// properties with the ICommand type.</param>
        /// <param name="inputBinder">The target to bind the key to.</param>
        /// <param name="instance">The instance with the command to bind to.</param>
        public static void ApplyKeyGestures(Type type, UIElement inputBinder, object instance)
        {
            ApplyKeyGestures(type, new InputBindWrapper(inputBinder), instance);
        }
    }
}