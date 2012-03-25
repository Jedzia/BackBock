using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using Jedzia.BackBock.ViewModel;
using System;

namespace Jedzia.BackBock.ViewModel.MVVM.Validation
{
public class EditableViewModelBase : ViewModelBase, IEditableObject
    {
        protected object originalObject = null;
        protected bool isDirty = false;

        #region IEditableObject Members

        public virtual void BeginEdit()
        {
            if(originalObject == null)
                originalObject = this.MemberwiseClone();
            isDirty = true;
        }

        private bool canceling;
        public virtual void CancelEdit()
        {

            if (originalObject == null) return;
            canceling = true;
            Type objectType = originalObject.GetType();
            PropertyInfo[] objectProperties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            object currentInstance = this;

            foreach (var property in objectProperties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    object originalPropertyValue = property.GetValue(originalObject, null);
                    property.SetValue(currentInstance, originalPropertyValue, null);
                }
            }

            isDirty = false;
            canceling = false;
            originalObject = null;
        }

        public virtual void EndEdit()
        {
            if (canceling)
            {
                return;
            }
            this.originalObject = null;
            isDirty = false;
        }

        #endregion
    }
}
