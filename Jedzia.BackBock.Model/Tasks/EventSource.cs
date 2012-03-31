// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSource.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace Jedzia.BackBock.Model.Tasks
{
    using Microsoft.Build.Framework;

    internal class EventSource : IEventSource
    {
        #region Fields

        private AnyEventHandler anyEventRaised;
        private BuildFinishedEventHandler buildFinished;
        private BuildStartedEventHandler buildStarted;
        private CustomBuildEventHandler customEventRaised;
        private BuildErrorEventHandler errorRaised;
        private BuildMessageEventHandler messageRaised;
        private ProjectFinishedEventHandler projectFinished;
        private ProjectStartedEventHandler projectStarted;
        private BuildStatusEventHandler statusEventRaised;
        private TargetFinishedEventHandler targetFinished;
        private TargetStartedEventHandler targetStarted;
        private TaskFinishedEventHandler taskFinished;
        private TaskStartedEventHandler taskStarted;
        private BuildWarningEventHandler warningRaised;

        #endregion

        #region Constructors

        public EventSource()
        {
            this.OnlyLogCriticalEvents = false;
        }

        #endregion

        #region Events

        public event AnyEventHandler AnyEventRaised
        {
            add
            {
                lock (this)
                    this.anyEventRaised += value;
            }

            remove
            {
                lock (this)
                    this.anyEventRaised -= value;
            }
        }

        public event BuildFinishedEventHandler BuildFinished
        {
            add
            {
                lock (this)
                    this.buildFinished += value;
            }

            remove
            {
                lock (this)
                    this.buildFinished -= value;
            }
        }

        public event BuildStartedEventHandler BuildStarted
        {
            add
            {
                lock (this)
                    this.buildStarted += value;
            }

            remove
            {
                lock (this)
                    this.buildStarted -= value;
            }
        }

        public event CustomBuildEventHandler CustomEventRaised
        {
            add
            {
                lock (this)
                    this.customEventRaised += value;
            }

            remove
            {
                lock (this)
                    this.customEventRaised -= value;
            }
        }

        public event BuildErrorEventHandler ErrorRaised
        {
            add
            {
                lock (this)
                    this.errorRaised += value;
            }

            remove
            {
                lock (this)
                    this.errorRaised -= value;
            }
        }

        public event BuildMessageEventHandler MessageRaised
        {
            add
            {
                lock (this)
                    this.messageRaised += value;
            }

            remove
            {
                lock (this)
                    this.messageRaised -= value;
            }
        }

        public event ProjectFinishedEventHandler ProjectFinished
        {
            add
            {
                lock (this)
                    this.projectFinished += value;
            }

            remove
            {
                lock (this)
                    this.projectFinished -= value;
            }
        }

        public event ProjectStartedEventHandler ProjectStarted
        {
            add
            {
                lock (this)
                    this.projectStarted += value;
            }

            remove
            {
                lock (this)
                    this.projectStarted -= value;
            }
        }

        public event BuildStatusEventHandler StatusEventRaised
        {
            add
            {
                lock (this)
                    this.statusEventRaised += value;
            }

            remove
            {
                lock (this)
                    this.statusEventRaised -= value;
            }
        }

        public event TargetFinishedEventHandler TargetFinished
        {
            add
            {
                lock (this)
                    this.targetFinished += value;
            }

            remove
            {
                lock (this)
                    this.targetFinished -= value;
            }
        }

        public event TargetStartedEventHandler TargetStarted
        {
            add
            {
                lock (this)
                    this.targetStarted += value;
            }

            remove
            {
                lock (this)
                    this.targetStarted -= value;
            }
        }

        public event TaskFinishedEventHandler TaskFinished
        {
            add
            {
                lock (this)
                    this.taskFinished += value;
            }

            remove
            {
                lock (this)
                    this.taskFinished -= value;
            }
        }

        public event TaskStartedEventHandler TaskStarted
        {
            add
            {
                lock (this)
                    this.taskStarted += value;
            }

            remove
            {
                lock (this)
                    this.taskStarted -= value;
            }
        }

        public event BuildWarningEventHandler WarningRaised
        {
            add
            {
                lock (this)
                    this.warningRaised += value;
            }

            remove
            {
                lock (this)
                    this.warningRaised -= value;
            }
        }

        #endregion

        #region Properties

        public bool OnlyLogCriticalEvents { get; set; }

        #endregion

        public void FireAnyEvent(object sender, BuildEventArgs bea)
        {
            if (this.anyEventRaised != null)
            {
                this.anyEventRaised(sender, bea);
            }
        }

        public void FireBuildFinished(object sender, BuildFinishedEventArgs bfea)
        {
            if (this.buildFinished != null)
            {
                this.buildFinished(sender, bfea);
            }

            this.FireAnyEvent(sender, bfea);
        }

        public void FireBuildStarted(object sender, BuildStartedEventArgs bsea)
        {
            if (this.buildStarted != null)
            {
                this.buildStarted(sender, bsea);
            }

            this.FireAnyEvent(sender, bsea);
        }

        public void FireCustomEventRaised(object sender, CustomBuildEventArgs cbea)
        {
            if (this.customEventRaised != null)
            {
                this.customEventRaised(sender, cbea);
            }

            this.FireAnyEvent(sender, cbea);
        }

        public void FireErrorRaised(object sender, BuildErrorEventArgs beea)
        {
            if (this.errorRaised != null)
            {
                this.errorRaised(sender, beea);
            }

            this.FireAnyEvent(sender, beea);
        }

        public void FireMessageRaised(object sender, BuildMessageEventArgs bmea)
        {
            if (this.messageRaised != null)
            {
                this.messageRaised(sender, bmea);
            }

            this.FireAnyEvent(sender, bmea);
        }

        public void FireProjectFinished(object sender, ProjectFinishedEventArgs pfea)
        {
            if (this.projectFinished != null)
            {
                this.projectFinished(sender, pfea);
            }

            this.FireAnyEvent(sender, pfea);
        }

        public void FireProjectStarted(object sender, ProjectStartedEventArgs psea)
        {
            if (this.projectStarted != null)
            {
                this.projectStarted(sender, psea);
            }

            this.FireAnyEvent(sender, psea);
        }

        public void FireTargetFinished(object sender, TargetFinishedEventArgs tfea)
        {
            if (this.targetFinished != null)
            {
                this.targetFinished(sender, tfea);
            }

            this.FireAnyEvent(sender, tfea);
        }

        public void FireTargetStarted(object sender, TargetStartedEventArgs tsea)
        {
            if (this.targetStarted != null)
            {
                this.targetStarted(sender, tsea);
            }

            this.FireAnyEvent(sender, tsea);
        }

        public void FireTaskFinished(object sender, TaskFinishedEventArgs tfea)
        {
            if (this.taskFinished != null)
            {
                this.taskFinished(sender, tfea);
            }

            this.FireAnyEvent(sender, tfea);
        }

        public void FireTaskStarted(object sender, TaskStartedEventArgs tsea)
        {
            if (this.taskStarted != null)
            {
                this.taskStarted(sender, tsea);
            }

            this.FireAnyEvent(sender, tsea);
        }

        public void FireWarningRaised(object sender, BuildWarningEventArgs bwea)
        {
            if (this.warningRaised != null)
            {
                this.warningRaised(sender, bwea);
            }

            this.FireAnyEvent(sender, bwea);
        }
    }
}