﻿// ****************************************************************************
// <copyright file="PreferredConstructor.cs" company="GalaSoft Laurent Bugnion">
// Copyright © GalaSoft Laurent Bugnion 2011
// </copyright>
// ****************************************************************************
// <author>Laurent Bugnion</author>
// <email>laurent@galasoft.ch</email>
// <date>10.4.2011</date>
// <project>GalaSoft.MvvmLight.Extras.Ioc</project>
// <web>http://www.galasoft.ch</web>
// <license>
// See license.txt in this project or http://www.galasoft.ch/license_MIT.txt
// </license>
// ****************************************************************************

namespace Jedzia.BackBock.ViewModel.MVVM.Ioc
{
    using System;

    /// <summary>
    /// When used with the <see cref="SimpleIoc"/> container, specifies which constructor
    /// should be used to instantiate when GetInstance is called.
    /// If there is only one constructor in the class, this attribute is
    /// not needed.
    /// </summary>
    //// [ClassInfo(typeof(SimpleIoc))]
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class PreferredConstructorAttribute : Attribute
    {
    }
}
