// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableViewModelBase.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.ViewModel.MVVM
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Base class of a editable <c>ViewModel</c> implementing <see cref="IEditableObject"/>.
    /// </summary>
    public class EditableViewModelBase : ViewModelBase, IEditableObject
    {
        #region Fields

        /// <summary>
        /// Indicates, that this instance has uncommitted changes.
        /// </summary>
        protected bool isDirty;

        /// <summary>
        /// The initial object before editing.
        /// </summary>
        protected object originalObject;

        private bool canceling;

        #endregion

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public virtual void BeginEdit()
        {
            if (this.originalObject == null)
            {
                this.originalObject = MemberwiseClone();
            }

            this.isDirty = true;
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public virtual void CancelEdit()
        {
            if (this.originalObject == null)
            {
                return;
            }

            this.canceling = true;
            Type objectType = this.originalObject.GetType();
            PropertyInfo[] objectProperties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            object currentInstance = this;

            foreach(var property in objectProperties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    object originalPropertyValue = property.GetValue(this.originalObject, null);
                    property.SetValue(currentInstance, originalPropertyValue, null);
                }
            }

            this.isDirty = false;
            this.canceling = false;
            this.originalObject = null;
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public virtual void EndEdit()
        {
            if (this.canceling)
            {
                return;
            }

            this.originalObject = null;
            this.isDirty = false;
        }
    }
}