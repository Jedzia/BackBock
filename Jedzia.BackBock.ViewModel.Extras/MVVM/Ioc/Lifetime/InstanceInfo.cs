using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc.Lifetime
{
    /// <summary>
    /// InstanceInfo.
    /// </summary>
    [Serializable]
    internal class InstanceInfo //: IEquatable<InstanceInfo>
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
        internal InstanceLifetime Lifetime { get; private set; }
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
        internal InstanceInfo(Type instanceType, Type setupType, InstanceLifetime lifetime)
        {
            this.InstanceType = instanceType;
            this.Lifetime = lifetime;
            this.Lifetime.SetType(setupType);
        }
        #endregion
        #region Methods
        /*/// <summary>
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
        }*/
        #endregion
    }

}
