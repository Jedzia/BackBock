// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2012 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.Tasks
{
    using System;

    /// <summary>
    /// Specifies constants that define the importance of a build message.
    /// </summary>
    [Serializable]
    public enum MessageImportance
    {
        /// <summary>
        /// A high importance message.
        /// </summary>
        High,
        
        /// <summary>
        /// A normal importance message.
        /// </summary>
        Normal,
        
        /// <summary>
        /// A low importance message.
        /// </summary>
        Low
    }
}