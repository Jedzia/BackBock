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
    
    public class DisplayModeViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The summary.
        /// </summary>
        private DisplayMode displayMode = DisplayMode.Standard;

        #endregion

        #region Properties

        public string DisplayAll
        {
            get
            {
                return this.IsAllDisplayMode ? "Visible" : "Collapsed";
            }
        }

        public string DisplayExpert
        {
            get
            {
                return this.IsExpertDisplayMode ? "Visible" : "Collapsed";
            }
        }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
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

        public bool IsAllDisplayMode
        {
            get
            {
                return this.DisplayMode == DisplayMode.All;
            }
        }

        public bool IsExpertDisplayMode
        {
            get
            {
                return this.DisplayMode == DisplayMode.Expert |
                       this.DisplayMode == DisplayMode.All;
            }
        }

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