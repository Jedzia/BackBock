// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayModeViewModel.cs" company="EvePanix">
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

    /// <summary>
    /// Base class for a <see cref="ViewModel"/> with <see cref="DisplayMode"/>'s.
    /// </summary>
    public class DisplayModeViewModel : ViewModelBase
    {
        #region Fields

        private DisplayMode displayMode = DisplayMode.Standard;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Visibility string for <see cref="ViewModel.DisplayMode.All"/>.
        /// </summary>
        public string DisplayAll
        {
            get
            {
                return this.IsAllDisplayMode ? "Visible" : "Collapsed";
            }
        }

        /// <summary>
        /// Gets the Visibility string for <see cref="ViewModel.DisplayMode.Expert"/>.
        /// </summary>
        public string DisplayExpert
        {
            get
            {
                return this.IsExpertDisplayMode ? "Visible" : "Collapsed";
            }
        }

        /// <summary>
        /// Gets or sets the display mode.
        /// </summary>
        /// <value>
        /// The display mode.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException"><c>value</c> is out of range.</exception>
        public DisplayMode DisplayMode
        {
            get
            {
                return this.displayMode;
            }

            set
            {
                if (this.displayMode == value)
                {
                    return;
                }

                var oldValue = this.displayMode;
                this.displayMode = value;
                RaisePropertyChanged("DisplayMode");

                /* Transition Logic
                 * ================
                 * Standard -> Expert : DisplayExpert
                 * Standard -> All    : DisplayExpert, DisplayAll
                 * Expert -> All      : DisplayAll
                 * All -> Expert      : DisplayAll
                 * All -> Standard    : DisplayExpert, DisplayAll
                 * Expert -> Standard : DisplayExpert
                 */
                switch (oldValue)
                {
                    case DisplayMode.Standard:
                        switch (value)
                        {
                            case DisplayMode.Expert:
                                RaisePropertyChanged("DisplayExpert");
                                break;
                            case DisplayMode.All:
                                RaisePropertyChanged("DisplayExpert");
                                RaisePropertyChanged("DisplayAll");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("value");
                        }

                        break;
                    case DisplayMode.Expert:
                        switch (value)
                        {
                            case DisplayMode.Standard:
                                RaisePropertyChanged("DisplayExpert");
                                break;
                            case DisplayMode.All:
                                RaisePropertyChanged("DisplayAll");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("value");
                        }

                        break;
                    case DisplayMode.All:
                        switch (value)
                        {
                            case DisplayMode.Standard:
                                RaisePropertyChanged("DisplayAll");
                                RaisePropertyChanged("DisplayExpert");
                                break;
                            case DisplayMode.Expert:
                                RaisePropertyChanged("DisplayAll");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("value");
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is in display mode  <see cref="ViewModel.DisplayMode.All"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in All display mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllDisplayMode
        {
            get
            {
                return this.DisplayMode == DisplayMode.All;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is in display mode  <see cref="ViewModel.DisplayMode.Expert"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is Expert display mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpertDisplayMode
        {
            get
            {
                return this.DisplayMode == DisplayMode.Expert |
                       this.DisplayMode == DisplayMode.All;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is in display mode  <see cref="ViewModel.DisplayMode.Standard"/>.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is Standard display mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsStandardDisplayMode
        {
            get
            {
                return this.DisplayMode == DisplayMode.Standard |
                       this.DisplayMode == DisplayMode.Expert |
                       this.DisplayMode == DisplayMode.All;
            }
        }

        #endregion
    }
}