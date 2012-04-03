// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestTask.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Model.Tests.Tasks.Logging
{
    using Microsoft.Build.Framework;

    public class TestTask : ITask
    {
        #region Fields

        public static bool HasExecuted;
        public static int Instantiated;
        public static IBuildEngine MyBuildEngine;
        public static ITaskHost MyHostObject;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TestTask"/> class.
        /// </summary>
        public TestTask()
        {
            Instantiated++;
        }

        #endregion

        #region Properties

        public IBuildEngine BuildEngine
        {
            get
            {
                return MyBuildEngine;
            }

            set
            {
                MyBuildEngine = value;
            }
        }

        public ITaskHost HostObject
        {
            get
            {
                return MyHostObject;
            }

            set
            {
                MyHostObject = value;
            }
        }

        #endregion

        public static bool SExecute()
        {
            HasExecuted = true;
            return true;
        }

        public bool Execute()
        {
            return SExecute();
        }
    }
}